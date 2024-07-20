using ReactiveUI;
using System.Reactive;
using SystemObslugiBiblioteki.DTO;
using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.Service;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private MainWindowViewModel _mainWindowViewModel;
        private UserService _userService;
        private AuthService _authService;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }


        public LoginViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _userService = new UserService(appDbContext);
            _authService = new AuthService(_userService);
            _mainWindowViewModel = mainWindowViewModel;

            LoginCommand = ReactiveCommand.Create(LoginUser);
            RegisterCommand = ReactiveCommand.Create(RegisterUser);
        }

        private void LoginUser()
        {
            var loginData = new LoginDto()
            {
                Name = _username,
                Password = _password
            };

            var result = _authService.Login(loginData);

            if (result != null)
            {
                _mainWindowViewModel.CurrentUser = result;
                _mainWindowViewModel.ShowLibrary();
            }
            else
            {
                ErrorMessage = "Invalid login data!";
            }
        }

        private void RegisterUser()
        {
            _mainWindowViewModel.ShowRegisterView();
        }
    }
}
