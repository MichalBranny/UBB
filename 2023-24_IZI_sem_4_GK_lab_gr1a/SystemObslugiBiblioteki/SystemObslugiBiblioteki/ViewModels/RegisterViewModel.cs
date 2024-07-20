using ReactiveUI;
using System.Reactive;
using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.Service;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isAdmin;

        private MainWindowViewModel _mainWindowViewModel;
        private UserService _userService;

        public string? Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string? Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => this.RaiseAndSetIfChanged(ref _isAdmin, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public RegisterViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _userService = new UserService(appDbContext);

            RegisterCommand = ReactiveCommand.Create(RegisterUser);
            BackCommand = ReactiveCommand.Create(GoBackToLoginPage);
        }

        private async void RegisterUser()
        {
            if (Username != null && Password != null)
            {
                var user = _userService.GetByName(Username);
                if (user == null)
                {
                    var createdUser = await _userService.CreateUser(Username, Password, IsAdmin);
                    _mainWindowViewModel.CurrentUser = createdUser;
                    _mainWindowViewModel.ShowLibrary();
                }
                else
                {
                    ErrorMessage = "User already exist!";
                }
            }
            else
            {
                ErrorMessage = "Empty login or password!";
            }
        }

        private void GoBackToLoginPage()
        {
            _mainWindowViewModel.ShowLoginView();
        }
    }
}
