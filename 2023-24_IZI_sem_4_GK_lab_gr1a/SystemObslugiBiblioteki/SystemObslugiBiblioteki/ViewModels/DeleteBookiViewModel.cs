using SystemObslugiBiblioteki.Model;
using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.Service;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class DeleteBookViewModel : ViewModelBase
    {
        private ObservableCollection<Books> _books;
        private Books _selectedBookToDelete;
        private int _categoryId;

        private MainWindowViewModel _mainWindowViewModel;

        private BookService _bookService;
        private CheckOutService _checkOutService;
        public ObservableCollection<Books> AllBooks { get; set; }

        public Books? SelectedBook
        {
            get => _selectedBookToDelete;
            set => this.RaiseAndSetIfChanged(ref _selectedBookToDelete, value);
        }

        public ReactiveCommand<Unit, Unit> DeleteBookCommand { get; }
        public ReactiveCommand<Unit, Unit> ReturnToLibraryCommand { get; }

        public DeleteBookViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _bookService = new BookService(appDbContext);
            _checkOutService = new CheckOutService(mainWindowViewModel, appDbContext, _bookService);
            AllBooks = new ObservableCollection<Books>();

            DeleteBookCommand = ReactiveCommand.Create(DeleteBook);
            ReturnToLibraryCommand = ReactiveCommand.Create(NavigateBack);
            LoadBooks();
        }

        private async void DeleteBook()
        {
            if (SelectedBook != null)
            {
                await _bookService.DeleteBook(SelectedBook.Id);
            }
            LoadBooks();
        }

        private void LoadBooks()
        {
            var allBooks = _bookService.GetBooks().ToList();

            AllBooks.Clear();

            foreach (var book in allBooks)
            {
                AllBooks.Add(book);
            }
        }

        private void NavigateBack()
        {
            _mainWindowViewModel.ShowLibrary();
        }
    }
}
