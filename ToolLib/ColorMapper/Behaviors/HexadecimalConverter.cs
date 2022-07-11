/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.Behaviors
*   文件名称 ：HexadecimalConverter.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/23/2019 2:51:07 PM 
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
    using System.Windows.Data;

    public class HexadecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var r = this.ToInt(value);
            if (r != 0)
            {
                return (r >= 16)
                    ? System.Convert.ToString(r, 16)
                    : string.Format("0{0}", System.Convert.ToString(r, 16));
            }
            else
            {
                return "00";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "00";
        }

        private int ToInt(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            int data;
            int.TryParse(obj.ToString().Trim(), out data);

            return data;
        }
    }
}
