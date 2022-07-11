/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.Model
*   文件名称 ：SettingColor.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/23/2019 4:59:47 PM 
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
    using System.Windows.Media;

    public class SettingColor : ObservableObject
    {
        public SettingColor()
        {
        }

        public SettingColor(string key, string text)
            : this(key, text, FormatType.ConvertToColor(text))
        {
        }

        public SettingColor(string key, string text, Color color)
        {
            this.Key = key;
            this.Text = text;
            this.Color = color;
        }

        private string _key;
        public string Key { get => _key; set => SetProperty(ref _key, value); }

        private string _text;
        public string Text { get => _text; set => SetProperty(ref _text, value); }

        private Color _color;
        public Color Color { get => _color; set => SetProperty(ref _color, value); }
    }
}
