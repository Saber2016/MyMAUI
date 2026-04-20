using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCommandTest.ViewModels
{
    public partial class PersonCollectionViewModel:ObservableObject
    {
        [ObservableProperty]
        public partial PersonViewModel? PersonEdit { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NewCommand))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
        public partial bool IsEditing {  get; set; }

        public IList<PersonViewModel> Persons { get; } = new ObservableCollection<PersonViewModel>();

        [RelayCommand(CanExecute = nameof(CanNew))]
        private void New()
        {
            PersonEdit = new PersonViewModel();
            IsEditing = true;
        }

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private void Submit()
        {
            Persons.Add(PersonEdit!);
            PersonEdit = null;
            IsEditing = false;
        }
        [RelayCommand(CanExecute = nameof(CanCancel))]
        private void Cancel()
        {
            PersonEdit = null;
            IsEditing = false;
        }

        partial void OnPersonEditChanged(PersonViewModel? oldValue, PersonViewModel? newValue)
        {
            if(oldValue != null)
            {
                oldValue.PropertyChanged -= PersonEdit_PropertyChanged;
            }
            if(newValue != null)
            {
                newValue.PropertyChanged += PersonEdit_PropertyChanged;
            }
        }

        private void PersonEdit_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(PersonViewModel.Name) || e.PropertyName == nameof(PersonViewModel.Age))
                SubmitCommand.NotifyCanExecuteChanged();
        }

        private bool CanNew() => !IsEditing;
        private bool CanSubmit() => PersonEdit != null && PersonEdit.Name != null && PersonEdit.Name.Length > 0 && PersonEdit.Age>0;
        private bool CanCancel() => IsEditing;
    }
}
