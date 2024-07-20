using ReactiveUI;
using SystemObslugiBiblioteki.Model.Dto;
using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;
        private IAppDbContext _appDbContext;
        private UserDto _currentUser;
        public string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RB2_Gr1;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public object CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public UserDto CurrentUser
        {
            get => _currentUser;
            set => this.RaiseAndSetIfChanged(ref _currentUser, value);
        }

        public MainWindowViewModel()
        {
            _appDbContext = new AppDbContext(ConnectionString);
            CurrentView = new LoginViewModel(this, _appDbContext);
        }

        public void ShowBooks()
        {
            CurrentView = new BooksViewModel(this, _appDbContext);
        }

        public void ShowLibrary()
        {
            CurrentView = new LibraryViewModel(this, _appDbContext);
        }

        public void AddBooks()
        {
            CurrentView = new CreateBookViewModel(this, _appDbContext);
        }
        public void DeleteBooks()
        {
            CurrentView = new DeleteBookViewModel(this, _appDbContext);
        }
        public void ShowRegisterView()
        {
            CurrentView = new RegisterViewModel(this, _appDbContext);
        }

        public void ShowLoginView()
        {
            CurrentView = new LoginViewModel(this, _appDbContext);
        }
    }
}
