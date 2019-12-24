/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.Service
*   文件名称 ：IDispatcherService.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/20/2019 5:56:00 PM 
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
    using System.Windows.Threading;

    public interface IDispatcherService
    {
        void Invoke(Action callback, DispatcherPriority priority = DispatcherPriority.ContextIdle);

        TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority = DispatcherPriority.ContextIdle);

        void InvokeAsync(Action callback, DispatcherPriority priority = DispatcherPriority.ContextIdle);

        DispatcherTimer CreateDispatcherTimer(TimeSpan interval, EventHandler callback, DispatcherPriority priority = DispatcherPriority.ContextIdle);
    }
}
