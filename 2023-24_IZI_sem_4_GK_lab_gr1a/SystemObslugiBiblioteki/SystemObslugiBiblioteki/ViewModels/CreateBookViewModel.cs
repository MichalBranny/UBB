using ReactiveUI;
using System.Reactive;
using SystemObslugiBiblioteki.Persistence;
using SystemObslugiBiblioteki.Service;
using SystemObslugiBiblioteki.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemObslugiBiblioteki.ViewModels;

namespace SystemObslugiBiblioteki.ViewModels
{
    public class CreateBookViewModel : ViewModelBase
    {
        private string? _bookName;
        private string? _bookAuthor;
        private int _categoryId;
        private string _errorMessage;

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly BookService _bookService;
        public List<BookCategory> BookCategories { get; }
        public ReactiveCommand<Unit, Unit> AddBookCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public string BookName
        {
            get => _bookName;
            set => this.RaiseAndSetIfChanged(ref _bookName, value);
        }

        public string BookAuthor
        {
            get => _bookAuthor;
            set => this.RaiseAndSetIfChanged(ref _bookAuthor, value);
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

        public CreateBookViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _bookService = new BookService(appDbContext);
            BookCategories = Enum.GetValues(typeof(BookCategory)).Cast<BookCategory>().Where(x => x != BookCategory.All).ToList();
            AddBookCommand = ReactiveCommand.Create(HandleCreateCommand);
            BackCommand = ReactiveCommand.Create(NavigateBack);
        }

        private async void HandleCreateCommand()
        {
            if (_bookName != null && _bookAuthor != null)
            {
                await _bookService.CreateBook(_bookName, _bookAuthor, _categoryId, true);
                _mainWindowViewModel.ShowLibrary();
            }
            else
            {
                ErrorMessage = "Empty name or author!";
            }
        }

        private void NavigateBack()
        {
            _mainWindowViewModel.ShowLibrary();
        }
    }
}
