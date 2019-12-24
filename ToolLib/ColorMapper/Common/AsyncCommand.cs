/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper
*   文件名称 ：AsyncCommand.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/20/2019 5:27:35 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修改者   ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
namespace ColorMapper
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;

    public abstract class AsyncCommandBase : CommandBase
    {
        private readonly object _syncRoot = new object();
        private readonly bool _allowConcurrentExecutions;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private CancellationTokenSource _cts;
        private int _concurrentExecutions;

        protected AsyncCommandBase(bool allowConcurrentExecutions = false)
        {
            _allowConcurrentExecutions = allowConcurrentExecutions;
        }

        public bool IsRunning => _concurrentExecutions > 0;

        protected CancellationToken CancelToken => _cts.Token;

        protected abstract bool CanExecuteImpl(object parameter);

        protected abstract Task ExecuteAsyncImpl(object parameter);

        public void Cancel()
        {
            lock (_syncRoot)
            {
                if (_cts == null)
                {
                    _logger.Trace("AsyncCommand : Attempt to cancel a task that is not running");
                }
                else
                {
                    _cts.Cancel();
                }
            }
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public override bool CanExecute(object parameter)
        {
            if (!_allowConcurrentExecutions && IsRunning)
                return false;
            else
                return CanExecuteImpl(parameter);
        }

        public override async void OnExecute(object parameter)
        {
            try
            {
                await ExecuteAsync(parameter, true).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.Trace("AsyncCommand : exception executing task : {0}", e.Message);
                throw;
            }
        }

        public void Execute()
        {
            Execute(null);
        }

        protected async Task ExecuteAsync(object parameter, bool hideCanceledException)
        {
            if (CanExecuteImpl(parameter))
            {
                await ExecuteConcurrentAsync(parameter, hideCanceledException).ConfigureAwait(false);
            }
        }

        private async Task ExecuteConcurrentAsync(object parameter, bool hideCanceledException)
        {
            bool started = false;
            try
            {
                lock (_syncRoot)
                {
                    if (_concurrentExecutions == 0)
                    {
                        InitCancellationTokenSource();
                    }
                    else if (!_allowConcurrentExecutions)
                    {
                        _logger.Trace("AsyncCommand : execute ignored, already running.");
                        return;
                    }
                    _concurrentExecutions++;
                    started = true;
                }
                if (!_allowConcurrentExecutions)
                {
                    RaiseCanExecuteChanged();
                }
                if (!CancelToken.IsCancellationRequested)
                {
                    try
                    {
                        // With configure await false, the CanExecuteChanged raised in finally clause might run in another thread.
                        // This should not be an issue as long as ShouldAlwaysRaiseCECOnUserInterfaceThread is true.
                        await ExecuteAsyncImpl(parameter).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException e)
                    {
                        _logger.Trace("AsyncCommand : OperationCanceledException");
                        //Rethrow if the exception does not come from the current cancellation token
                        if (!hideCanceledException || e.CancellationToken != CancelToken)
                        {
                            throw;
                        }
                    }
                }
            }
            finally
            {
                if (started)
                {
                    lock (_syncRoot)
                    {
                        _concurrentExecutions--;
                        if (_concurrentExecutions == 0)
                        {
                            ClearCancellationTokenSource();
                        }
                    }
                    if (!_allowConcurrentExecutions)
                    {
                        RaiseCanExecuteChanged();
                    }
                }
            }
        }

        private void ClearCancellationTokenSource()
        {
            if (_cts == null)
            {
                _logger.Trace("AsyncCommand : Unexpected ClearCancellationTokenSource, no token available!");
            }
            else
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        private void InitCancellationTokenSource()
        {
            if (_cts != null)
            {
                _logger.Trace("AsyncCommand : : Unexpected InitCancellationTokenSource, a token is already available!");
            }
            _cts = new CancellationTokenSource();
        }
    }

    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<bool> _canExecute;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = (cancellationToken) => execute();
            _canExecute = canExecute;
        }

        public AsyncCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        protected override Task ExecuteAsyncImpl(object parameter)
        {
            return _execute(CancelToken);
        }

        public static AsyncCommand<T> CreateCommand<T>(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new AsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public static AsyncCommand<T> CreateCommand<T>(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new AsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public async Task ExecuteAsync(object parameter = null)
        {
            await base.ExecuteAsync(parameter, false).ConfigureAwait(false);
        }
    }

    public class AsyncCommand<T> : AsyncCommandBase
    {
        private readonly Func<T, CancellationToken, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = (p, c) => execute(p);
            _canExecute = canExecute;
        }

        public AsyncCommand(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public Task ExecuteAsync(T parameter)
        {
            return ExecuteAsync(parameter, false);
        }

        public void Execute(T parameter)
        {
            base.Execute(parameter);
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return _canExecute == null || _canExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        protected override Task ExecuteAsyncImpl(object parameter)
        {
            return _execute((T)typeof(T).MakeSafeValueCore(parameter), CancelToken);
        }
    }
}
