using System.Windows;
using System.Windows.Controls;

namespace SimpleNotes
{
    public static class WatermarkService
    {
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkService), new PropertyMetadata(default(string), OnWatermarkChanged));

        public static void SetWatermark(UIElement element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        public static string GetWatermark(UIElement element)
        {
            return (string)element.GetValue(WatermarkProperty);
        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus += (sender, args) =>
                {
                    if (textBox.Text == (string)e.NewValue)
                    {
                        textBox.Text = string.Empty;
                        textBox.Foreground = SystemColors.ControlTextBrush;
                    }
                };

                textBox.LostFocus += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Text = (string)e.NewValue;
                        textBox.Foreground = SystemColors.GrayTextBrush;
                    }
                };

                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = (string)e.NewValue;
                    textBox.Foreground = SystemColors.GrayTextBrush;
                }
            }
        }
    }
}
