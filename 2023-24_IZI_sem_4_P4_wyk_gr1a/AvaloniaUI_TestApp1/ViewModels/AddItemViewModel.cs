
// ViewModels/AddItemViewModel.cs

using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.ViewModels
{
    class AddItemViewModel : ViewModelBase
    {
        string description;

        public AddItemViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                x => x.Description,
                x => !string.IsNullOrWhiteSpace(x));

            Ok = ReactiveCommand.Create(
                () => new TodoItem { Description = description },
                okEnabled);
            Cancel = ReactiveCommand.Create(() => { });
        }
        public string Description 
        {
            get => description;
            set => this.RaiseAndSetIfChanged(ref description, value); 
        }

        public ReactiveCommand<Unit, TodoItem> Ok { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
    }
}
