/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.ViewModel
*   文件名称 ：OpenViewModel.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/23/2019 2:04:11 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修改者   ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ColorMapper
{
    public class OpenViewModel : ObservableObject
    {
        private readonly ILogService _log;

        public ICommand OpenAsyncCommand { get; private set; }

        public ILogService Log => _log;

        public OpenViewModel(ILogService log)
        {
            _log = log;
            OpenAsyncCommand = new AsyncCommand(OpenAsync, CanOpenAsync);
        }

        protected virtual Task OpenAsync() { return Task.FromResult(0); }
        protected virtual bool CanOpenAsync() { return true; }

        protected async Task DoOpenAsync(string name, Func<string, Task<string>> open)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = Properties.Resources.Filter,
                DefaultExt = "xaml",
                Multiselect = false,
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var results = new StringBuilder();

                try
                {
                    foreach (var fileName in dialog.FileNames)
                    {
                        if (results.Length != 0)
                            results.Append(Environment.NewLine);
                        results.Append("-");
                        results.Append(Path.GetFileName(fileName));
                        results.Append(": ");

                        var resInfo = await open(fileName);
                        if (string.IsNullOrEmpty(resInfo))
                        {
                            results.Append("Success!");
                        }
                        else
                        {
                            results.Append(resInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    results.Append("Error: ");
                    results.Append(ex.Message);
                    _log.Error("Error, message:{0}, stacktrace:{1}", ex.Message, ex.StackTrace);
                }
                finally
                {
                    _log.Info(results.ToString());
                }
            }
        }
    }
}
