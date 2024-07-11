using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SimpleNotes.Models;

namespace SimpleNotes
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Note> notes;
        private Note selectedNote;
        private string selectedNoteContent;
        private string connectionString = "Data Source=DESKTOP-GONFCJC;Initial Catalog=SimpleNotesDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public ObservableCollection<Note> Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }

        public string SelectedNoteContent
        {
            get { return selectedNoteContent; }
            set
            {
                selectedNoteContent = value;
                OnPropertyChanged("SelectedNoteContent");
                if (SelectedNote != null)
                {
                    SelectedNote.Content = value;
                }
            }
        }

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                if (selectedNote != null)
                {
                    SelectedNoteContent = selectedNote.Content;
                }
                else
                {
                    SelectedNoteContent = string.Empty;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Notes = new ObservableCollection<Note>();
            LoadNotes();
        }

        private void LoadNotes()
        {
            using (var db = new AppDbContext(connectionString))
            {
                var allNotes = db.GetNotes();
                foreach (var note in allNotes)
                {
                    Notes.Add(note);
                }
            }
        }

        private void CreateNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var createNoteWindow = new CreateNoteWindow();
            if (createNoteWindow.ShowDialog() == true)
            {
                var newNote = createNoteWindow.Note;
                using (var db = new AppDbContext(connectionString))
                {
                    db.InsertNote(newNote);
                }
                Notes.Add(newNote);
            }
        }

        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem is Note selectedNote)
            {
                var confirmationWindow = new NoteDeleteConfirmation();
                if (confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed)
                {
                    using (var db = new AppDbContext(connectionString))
                    {
                        db.DeleteNoteById(selectedNote.Id);
                    }
                    Notes.Remove(selectedNote);
                    SelectedNoteContent = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Please select a note to delete.");
            }
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedNote = NotesListBox.SelectedItem as Note;
        }

        private void ViewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem is Note selectedNote)
            {
                var noteDetailsWindow = new NoteDetailsWindow(selectedNote);
                noteDetailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a note to view.");
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNote != null)
            {
                SelectedNote.ModificationDate = DateTime.Now;
                using (var db = new AppDbContext(connectionString))
                {
                    db.UpdateNoteContent(SelectedNote);
                }
                MessageBox.Show("Changes saved successfully.");
            }
            else
            {
                MessageBox.Show("Please select a note to save changes.");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
