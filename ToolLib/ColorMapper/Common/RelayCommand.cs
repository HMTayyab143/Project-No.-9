/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper
*   文件名称 ：RelayCommand.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/20/2019 5:36:53 PM 
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

    public class RelayCommand : CommandBase
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public bool CanExecute() { return CanExecute(null); }

        public override void OnExecute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }
        public void Execute() { OnExecute(null); }
    }

    public class RelayCommand<T> : CommandBase

    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public bool CanExecute(T parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public override void OnExecute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            _execute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        public void Execute() { OnExecute(null); }

        public void Execute(T parameter)
        {
            if (!CanExecute(parameter)) return;
            _execute(parameter);
        }
    }
}
