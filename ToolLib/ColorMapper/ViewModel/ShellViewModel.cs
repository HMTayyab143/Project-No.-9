/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper
*   文件名称 ：ShellViewModel.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/20/2019 6:30:52 PM 
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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ColorMapper
{
    public class ShellViewModel : OpenViewModel
    {
        private IDispatcherService _dispatcher = null;

        public ShellViewModel(ILogService log, IDispatcherService dispatcher) : base(log)
        {
            _dispatcher = dispatcher;
            this.Source = new ObservableRangeCollection<SettingColor>();
        }

        ObservableRangeCollection<SettingColor> _source = null;
        public ObservableRangeCollection<SettingColor> Source { get => _source; set => SetProperty(ref _source, value); }

        private string _colorValue = null;
        public string ColorValue { get => _colorValue; set => SetProperty(ref _colorValue, value); }

        private string _fileName = null;
        public string FileName { get => _fileName; set => SetProperty(ref _fileName, value); }

        private SettingColor _selectedColor = null;
        public SettingColor SelectedColor
        {
            get => _selectedColor;
            set
            {
                SetProperty(ref _selectedColor, value);
                this.ColorValue = value?.Text;
            }
        }

        protected async override Task OpenAsync()
        {
            await this.DoOpenAsync("Pase XAML colors.", path =>
            {
                this.FileName = path;

                if (string.IsNullOrEmpty(this.FileName))
                    return Task.FromResult<string>("File path is null.");

                List<SettingColor> list = null;
                Task task = Task.Run(() =>
                {
                    //list = new List<SettingColor>();
                    //list.Add(new SettingColor("#FFB67044"));
                    list = this.GetColors();
                });
                task.ContinueWith(t =>
                {
                    _dispatcher.InvokeAsync(() =>
                    {
                        this.Source.Clear();
                        this.Source.AddRange(list);
                    });
                });

                return Task.FromResult<string>("");
            });
        }

        private List<SettingColor> GetColors()
        {
            var list = new List<SettingColor>();

            using (StreamReader sr = new StreamReader(this.FileName))
            {
                Regex regex = new Regex("Color=\"\\S*\"");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var matcheList = regex.Matches(line);
                    if (matcheList != null && matcheList.Count > 0)
                    {
                        foreach (Match item in matcheList)
                        {
                            //Console.WriteLine(item.Value);

                            var split = item.Value.Split('\"');
                            list.Add(new SettingColor(split[1]));
                        }
                    }
                }
            }

            return list;
        }
    }
}
