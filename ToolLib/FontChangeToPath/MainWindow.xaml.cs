using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FontChangeToPath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.txtFontFamily.Text = "Razer Header Light";
            this.txtFontSize.Text = "50";
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtWord.Text))
            {
                this.txtPath.Text = "";
                return;
            }

            //"Microsoft YaHei",Broadway,Algerian,Wide Latin,Bodoni MT Black,Razer Header Light
            //Razer Header Regular,Curlz MT
            this.txtPath.Text = this.GetTextPath(
                this.txtWord.Text,
                this.txtFontFamily.Text.Trim(),
                this.ToInt(this.txtFontSize.Text.Trim()));
        }

        public string GetTextPath(string word, string fontFamily, int fontSize)
        {
            Typeface typeface = new Typeface(
                new FontFamily(fontFamily), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            return GetTextPath(word, typeface, fontSize);
        }

        public string GetTextPath(string word, Typeface typeface, int fontSize)
        {
            if (string.IsNullOrEmpty(word))
                return string.Empty;

            var text = new FormattedText(
                word,
                new System.Globalization.CultureInfo("zh-cn"),//zh-cn
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.Black);

            Geometry geometry = text.BuildGeometry(new Point(0, 0));
            PathGeometry path = geometry.GetFlattenedPathGeometry();

            return path.ToString();
        }

        public int ToInt(object value)
        {
            if (value == null)
            {
                return 0;
            }

            var prm = value.ToString().Trim();
            if (prm.Contains("."))
            {
                try
                {
                    return Convert.ToInt32(Convert.ToDecimal(prm));
                }
                catch { }
            }

            int result;
            int.TryParse(value.ToString().Trim(), out result);

            return result;
        }
    }
}
