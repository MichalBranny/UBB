using System;
using System.Windows;
using SimpleNotes.Models;

namespace SimpleNotes
{
    public partial class NoteDetailsWindow : Window
    {
        public Note Note { get; private set; }

        public NoteDetailsWindow(Note note)
        {
            InitializeComponent();
            Note = note;
            DataContext = this;
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            Note.ModificationDate = DateTime.Now;
            using (var db = new AppDbContext("Data Source=DESKTOP-GONFCJC;Initial Catalog=SimpleNotesDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;"))
            {
                db.UpdateNoteContent(Note); // Assuming the same update method works for both content and category
            }
            MessageBox.Show("Changes saved successfully.");
            DialogResult = true;
        }
    }
}
