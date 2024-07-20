using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.Service;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class LibraryViewModel : ViewModelBase
    {
        private readonly BookService _bookService;
        private readonly CheckOutService _checkOutService;
        private MainWindowViewModel _mainWindowViewModel;
        private string _errorMessage;

        public ReactiveCommand<Unit, Task> BorrowBooksCommand { get; }
        public ReactiveCommand<Unit, Task> AddBookCommand { get; }
        public ReactiveCommand<Unit, Task> LogOutCommand { get; }
        public ReactiveCommand<Unit, Task> RemoveBookCommand { get; }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public LibraryViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _bookService = new BookService(appDbContext);
            _checkOutService = new CheckOutService(mainWindowViewModel, appDbContext, _bookService);
            _mainWindowViewModel = mainWindowViewModel;

            BorrowBooksCommand = ReactiveCommand.Create(BorrowBooks);
            AddBookCommand = ReactiveCommand.Create(AddBooks);
            LogOutCommand = ReactiveCommand.Create(LogOut);
            RemoveBookCommand = ReactiveCommand.Create(RemoveBook);
        }

        private Task BorrowBooks()
        {
            _mainWindowViewModel.ShowBooks();
            return Task.CompletedTask;
        }

        private Task AddBooks()
        {
            if (_mainWindowViewModel.CurrentUser.IsAdmin)
            {
                _mainWindowViewModel.AddBooks();
                return Task.CompletedTask;
            }
            else
            {
                ErrorMessage = "No permission!";
                return Task.CompletedTask;
            }
        }

        private Task RemoveBook()
        {
            if (_mainWindowViewModel.CurrentUser.IsAdmin)
            {
                _mainWindowViewModel.DeleteBooks();
                return Task.CompletedTask;
            }
            else
            {
                ErrorMessage = "No permission!";
                return Task.CompletedTask;
            }
        }

        private Task LogOut()
        {
            _mainWindowViewModel.ShowLoginView();
            return Task.CompletedTask;
        }
    }
}
