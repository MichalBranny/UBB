using System;
using System.Windows;
using SimpleNotes.Models;

namespace SimpleNotes
{
    public partial class CreateNoteWindow : Window
    {
        public Note Note { get; private set; } = new Note();

        public CreateNoteWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Note = new Note
            {
                Title = TitleTextBox.Text,
                Content = ContentTextBox.Text,
                Category = CategoryTextBox.Text,
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now
            };
            DialogResult = true;
        }
    }
}
