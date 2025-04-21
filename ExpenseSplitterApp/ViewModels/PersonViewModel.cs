using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExpenseSplitterApp.Services;
using ExpenseSplitterApp.Enums;

namespace ExpenseSplitterApp.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        private readonly ServiceUnitOfWork _service;

        #region ModelsForView
        public ObservableCollection<PersonModel> People { get; set; } = new();

        private PersonModel _selectedPerson = new();
        public PersonModel SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }
        #endregion

        #region Bindable Properties

        private AppState _state = AppState.Observe;
        public AppState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public string ActionButtonText => SelectedPerson?.Id == 0 ? "Add" : "Update";

        #endregion
        #region Commands
        public ICommand ActionCommand { get; }
        public ICommand ShowAddEntryCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        public PersonViewModel(ServiceUnitOfWork service)
        {
            _service = service;

            ActionCommand = new Command(async () => await OnActionAsync());
            ShowAddEntryCommand = new Command(ShowAddEntry);
            EditCommand = new Command<PersonModel>(OnEdit);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command<PersonModel>(async (person) => await OnDeleteAsync(person));

            _ = LoadPeopleAsync();
        }

        #region Private Methods
        private async Task LoadPeopleAsync()
        {
            var people = await _service.Person.GetAllAsync();
            People.Clear();
            foreach (var person in people)
                People.Add(person);
        }

        private void ShowAddEntry()
        {
            SelectedPerson = new PersonModel();
            State = AppState.Action;
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private void OnEdit(PersonModel person)
        {
            SelectedPerson = new PersonModel { Id = person.Id, Name = person.Name };
            State = AppState.Action;
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private async Task OnActionAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedPerson.Name)) return;

            if (SelectedPerson.Id == 0)
            {
                var newPerson = new PersonModel { Name = SelectedPerson.Name };
                await _service.Person.AddAsync(newPerson);
            }
            else
            {
                await _service.Person.UpdateAsync(SelectedPerson);
            }

            await LoadPeopleAsync();

            State = AppState.Observe;
            SelectedPerson = new PersonModel();
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private async Task OnDeleteAsync(PersonModel person)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Are you sure to delete {person.Name}?", "Yes", "No");
            if (!confirm) return;


            await _service.Person.DeleteAsync(person);
            People.Remove(person);
        }

        private void OnCancel()
        {
            SelectedPerson = new PersonModel(); // Clear entry
            State = AppState.Observe;
        }
        #endregion
        
    }
}
