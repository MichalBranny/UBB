using System.Windows;

namespace SimpleNotes
{
    public partial class NoteDeleteConfirmation : Window
    {
        public bool IsConfirmed { get; private set; }

        public NoteDeleteConfirmation()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            DialogResult = true;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            DialogResult = false;
        }
    }
}
