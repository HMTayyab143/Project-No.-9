/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper
*   文件名称 ：AppBootstrapper.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/20/2019 5:50:26 PM 
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
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class AppBootstrapper
    {
        private static readonly ILogService Log = new NlogService();
        internal static IServiceProvider ServiceProvider;

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Log.Info("init and configure ioc-container.");
                var container = new ServiceCollection();
                ConfigureContainer(container);
                Log.Info("build ioc-container.");
                ServiceProvider = container.BuildServiceProvider();
                Log.Info("ioc-container init success,start run application.");
                new App().Run(ServiceProvider.GetRequiredService<ShellView>());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "appliction crash in fatal exception.");
            }
        }

        private static void ConfigureContainer(IServiceCollection container)
        {
            //global ioc-container provider.
            container.AddSingleton<IServiceProvider>(provider => provider);
            //global logger.
            container.AddSingleton<ILogService>(provider => Log);
            //global mainwindow.
            container.AddSingleton<ShellView>();
            //global mainviewmodel.
            container.AddSingleton<ShellViewModel>();
            //global dispatcher
            container.AddSingleton<IDispatcherService, WpfDispatcherService>();
        }
    }
}
