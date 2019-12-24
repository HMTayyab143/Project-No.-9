using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorMapper
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        HexadecimalConverter _hex;

        public ShellView(ShellViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            _hex = new HexadecimalConverter();
        }

        private void ColorPicker_ColorChanged(
            object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.SetColor.Text = e.NewValue.ToString();
            }));
        }

        private void BtnColor_Click(object sender, RoutedEventArgs e)
        {
            colorPicker.Color = GetColor();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DR.Text = "";
                DG.Text = "";
                DB.Text = "";

                this.SetColor.Text = "#00000000";
                colorPicker.Color = GetColor();
            }));
        }

        private void H_TextChanged(object sender, TextChangedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var alpha = _hex.Convert(colorPicker.Alpha, null, null, CultureInfo.CurrentCulture);
                this.SetColor.Text =
                    string.Format("#{0}{1}{2}{3}", alpha, HR.Text, HG.Text, HB.Text).ToUpper();
            }));
        }

        private Color GetColor()
        {
            var color = System.Drawing.ColorTranslator.FromHtml(
                this.SetColor.Text.ToString().Trim());
            Color col = new Color();
            col.A = color.A;
            col.R = color.R;
            col.G = color.G;
            col.B = color.B;

            return col;
        }
    }
}
