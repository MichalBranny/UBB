using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using SystemObslugiBiblioteki.Model;
using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.Service;
using System.Linq;
using System.Threading.Tasks;
using SystemObslugiBiblioteki.Enums;
using System;
using System.Collections.Generic;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {

        private ObservableCollection<Books> _books;
        private Books? _selectedAvailableBooks;
        private Books? _selectedBorrowedBooks;
        private int _categoryId;
        private string _errorMessage;


        private MainWindowViewModel _mainWindowViewModel;


        private BookService _bookService;
        private CheckOutService _checkOutService;
        public ObservableCollection<Books> AvailableBooks { get; set; }
        public ObservableCollection<Books> BorrowedBooks { get; set; }
        public List<BookCategory> BookCategories { get; }

        public ObservableCollection<Books> Books
        {
            get => _books;
            set => this.RaiseAndSetIfChanged(ref _books, value);
        }

        public Books SelectedAvailableBooks
        {
            get => _selectedAvailableBooks;
            set => this.RaiseAndSetIfChanged(ref _selectedAvailableBooks, value);
        }

        public Books SelectedBorrowedBooks
        {
            get => _selectedBorrowedBooks;
            set => this.RaiseAndSetIfChanged(ref _selectedBorrowedBooks, value);
        }
        public int SelectedBookCategory
        {
            get => _categoryId;
            set => this.RaiseAndSetIfChanged(ref _categoryId, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> BorrowBookCommand { get; }
        public ReactiveCommand<Unit, Unit> ReturnBookCommand { get; }
        public ReactiveCommand<Unit, Unit> ReturnToLibraryCommand { get; }
        public ReactiveCommand<Unit, Unit> SortByCommand { get; }


        public BooksViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _bookService = new BookService(appDbContext);
            _checkOutService = new CheckOutService(mainWindowViewModel, appDbContext, _bookService);
            AvailableBooks = new ObservableCollection<Books>();
            BorrowedBooks = new ObservableCollection<Books>();

            BookCategories = Enum.GetValues(typeof(BookCategory)).Cast<BookCategory>().ToList();

            BorrowBookCommand = ReactiveCommand.Create(BorrowBook);
            ReturnBookCommand = ReactiveCommand.Create(ReturnBook);
            ReturnToLibraryCommand = ReactiveCommand.Create(NavigateBack);
            SortByCommand = ReactiveCommand.Create(ReloadView);
            ReactiveCommand.Create(LoadBooks);
        }

        private async void BorrowBook()
        {
            if (_selectedAvailableBooks != null)
            {
                await _checkOutService.AddCheckOut(_selectedAvailableBooks.Id);
                await LoadBooks();
                ReloadView();
            }
            else
            {
                ErrorMessage = "Choose book!";
            }

        }

        private async void ReturnBook()
        {
            if (_selectedBorrowedBooks != null)
            {
                await _checkOutService.ReturnBook(_selectedBorrowedBooks.Id);
                await LoadBooks();
                ReloadView();
            }
            else
            {
                ErrorMessage = "Choose book!";
            }
        }

        private async void ReloadView()
        {
            ErrorMessage = "";
            await LoadBooks();
        }

        private async Task LoadBooks()
        {
            var allBooks = _bookService.GetBooks(_categoryId).ToList();
            var borrowedBooksIds = _checkOutService.GetCheckedOutBooks().Select(s => s.BookId).ToList();

            AvailableBooks.Clear();
            BorrowedBooks.Clear();

            foreach (var book in allBooks)
            {
                if (book.IsAvailable)
                {
                    AvailableBooks.Add(book);
                }
                if (borrowedBooksIds.Contains(book.Id))
                {
                    BorrowedBooks.Add(book);
                }
            }
        }

        private void NavigateBack()
        {
            _mainWindowViewModel.ShowLibrary();
        }
    }
}
