
// Views/MainWindow.axaml.cs

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Todo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}