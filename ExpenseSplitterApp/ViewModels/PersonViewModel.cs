using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ExpenseSplitterApp.ViewModels
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly IUnitOfWork _unitOfWork;

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

        #region Bindable Properties
        private bool _isEditVisible;
        public bool IsEditVisible
        {
            get => _isEditVisible;
            set { _isEditVisible = value; OnPropertyChanged(nameof(IsEditVisible)); }
        }

        private bool _isPlusVisible = true;
        public bool IsPlusVisible
        {
            get => _isPlusVisible;
            set { _isPlusVisible = value; OnPropertyChanged(nameof(IsPlusVisible)); }
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

        public PersonViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            ActionCommand = new Command(async () => await OnActionAsync());
            ShowAddEntryCommand = new Command(() => ShowAddEntry());
            EditCommand = new Command<PersonModel>(OnEdit);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command<PersonModel>(async (person) => await OnDeleteAsync(person));

            _ = LoadPeopleAsync();
        }
        #region Private Methods
        private async Task LoadPeopleAsync()
        {
            var people = await _unitOfWork.People.GetAllAsync();
            People.Clear();
            foreach (var person in people)
                People.Add(person);
        }

        private void ShowAddEntry()
        {
            SelectedPerson = new PersonModel();
            IsEditVisible = true;
            IsPlusVisible = false;
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private void OnEdit(PersonModel person)
        {
            SelectedPerson = new PersonModel { Id = person.Id, Name = person.Name };
            IsEditVisible = true;
            IsPlusVisible = false;
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private async Task OnActionAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedPerson.Name)) return;

            if (SelectedPerson.Id == 0)
            {
                var newPerson = new PersonModel { Name = SelectedPerson.Name };
                await _unitOfWork.People.AddAsync(newPerson);
            }
            else
            {
                var trackedPerson = await _unitOfWork.People.GetByIdAsync(SelectedPerson.Id);
                trackedPerson.Name = SelectedPerson.Name;
                _unitOfWork.People.Update(trackedPerson);
            }

            await _unitOfWork.SaveAsync();
            await LoadPeopleAsync();

            IsEditVisible = false;
            IsPlusVisible = true;
            SelectedPerson = new PersonModel();
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private async Task OnDeleteAsync(PersonModel person)
        {
            var trackedPerson = await _unitOfWork.People.GetByIdAsync(person.Id);
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Are you sure to delete {person.Name}?", "Yes", "No");
            if (!confirm) return;


            _unitOfWork.People.Remove(trackedPerson);
            await _unitOfWork.SaveAsync();
            People.Remove(person);
        }

        private void OnCancel()
        {
            SelectedPerson = new PersonModel(); // Clear entry
            IsEditVisible = false;
            IsPlusVisible = true;
        }
        #endregion
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
