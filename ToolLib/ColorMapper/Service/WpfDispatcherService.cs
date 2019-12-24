/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.Service
*   文件名称 ：WpfDispatcherService.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/20/2019 5:55:49 PM 
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
    using System.Windows;
    using System.Windows.Threading;
    using System;

    public sealed class WpfDispatcherService : IDispatcherService
    {
        public void Invoke(Action action, DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            Application.Current?.Dispatcher?.Invoke(action, priority);
        }

        public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            if (Application.Current != null && Application.Current.Dispatcher != null)
                return Application.Current.Dispatcher.Invoke(callback, priority);
            return default;
        }

        public void InvokeAsync(Action callback, DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            Application.Current?.Dispatcher?.InvokeAsync(callback, priority);
        }

        public DispatcherTimer CreateDispatcherTimer(TimeSpan interval, EventHandler callback,
            DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            return new DispatcherTimer(interval, priority, callback, Application.Current.Dispatcher);
        }
    }
}
