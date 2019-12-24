/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.Common
*   文件名称 ：FormatType.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/23/2019 5:05:10 PM 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorMapper
{
    public class FormatType
    {
        public static Color ConvertToColor(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new Color();

            var color = System.Drawing.ColorTranslator.FromHtml(text.Trim());
            Color col = new Color();
            col.A = color.A;
            col.R = color.R;
            col.G = color.G;
            col.B = color.B;

            return col;
        }
    }
}
