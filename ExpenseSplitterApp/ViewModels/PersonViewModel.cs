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

        public ICommand ActionCommand { get; }
        public ICommand ShowAddEntryCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public PersonViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            ActionCommand = new Command(async () => await OnActionAsync());
            ShowAddEntryCommand = new Command(() => ShowAddEntry());
            EditCommand = new Command<PersonModel>(OnEdit);
            DeleteCommand = new Command<PersonModel>(async (person) => await OnDeleteAsync(person));

            _ = LoadPeopleAsync();
        }

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
                await _unitOfWork.People.AddAsync(SelectedPerson);
            }
            else
            {
                _unitOfWork.People.Update(SelectedPerson);
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
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Are you sure to delete {person.Name}?", "Yes", "No");
            if (!confirm) return;

            _unitOfWork.People.Remove(person);
            await _unitOfWork.SaveAsync();
            People.Remove(person);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
