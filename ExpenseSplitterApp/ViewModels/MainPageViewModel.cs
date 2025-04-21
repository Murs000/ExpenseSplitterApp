using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Enums;
using ExpenseSplitterApp.Models;
using ExpenseSplitterApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ExpenseSplitterApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ServiceUnitOfWork _service;
        #region ModelsForView
        public ObservableCollection<PersonModel> People { get; set; } = new();
        public ObservableCollection<ExpenceModel> Expences { get; set; } = new();


        private ExpenceModel _selectedExpence = new();
        public ExpenceModel SelectedExpence
        {
            get => _selectedExpence;
            set
            {
                _selectedExpence = value;
                OnPropertyChanged(nameof(SelectedExpence));
            }
        }
        #endregion
        #region Bindable Properties
        private List<Loan> _result = [];
        public List<Loan> Result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(nameof(Result)); }
        }
        
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
        public string ActionButtonText => SelectedExpence?.Id == 0 ? "Add" : "Update";
        #endregion
        #region Commands
        public ICommand AddExpenseCommand { get; }
        public ICommand ActionCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ResultCancelCommand { get; }
        public ICommand ActionCancelCommand { get; }
        public ICommand ShowEntryCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        #endregion
        public MainPageViewModel(ServiceUnitOfWork service)
        {
            _service = service;

            ActionCommand = new Command(async () => await OnActionAsync());
            CalculateCommand = new Command(async () => await OnCalculateAsync());
            ResultCancelCommand = new Command(OnResultCancel);
            ActionCancelCommand = new Command(OnActionCancel);
            ShowEntryCommand = new Command(OnShowEntry);
            EditCommand = new Command<ExpenceModel>(OnEdit);
            DeleteCommand = new Command<ExpenceModel>(async (expense) => await OnDeleteAsync(expense));

            LoadData();
        }
        public void LoadData()
        {
            _ = LoadExpencesAsync();
            _ = LoadPeopleAsync();
        }
        #region PrivateMethods
        private async Task OnCalculateAsync()
        {
            Result = await _service.SplitCalculatorService.CalculateAsync();

            State = AppState.Result;
        }
        private void OnEdit(ExpenceModel expence)
        {
            var matchedPerson = People.FirstOrDefault(p => p.Id == expence.PersonId);

            SelectedExpence = new ExpenceModel 
            { 
                Id = expence.Id, 
                Description = expence.Description, 
                PersonId = expence.PersonId,
                ExpenceAmount = expence.ExpenceAmount,
                Person =  matchedPerson
            };
            State = AppState.Action;
            OnPropertyChanged(nameof(ActionButtonText));
        }
        private void OnShowEntry()
        {
            State = AppState.Action;

            _ = LoadPeopleAsync();
        }

        private async Task LoadExpencesAsync()
        {
            var expenses = await _service.Expense.GetAllAsync();
            Expences.Clear();
            foreach (var expence in expenses)
                Expences.Add(expence);
        }

        private async Task LoadPeopleAsync()
        {
            var people = await _service.Person.GetAllAsync();
            People.Clear();
            foreach (var person in people)
                People.Add(person);
        }

        private async Task OnActionAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedExpence.Description)) return;
            if (SelectedExpence.Person == null) return;
            if (SelectedExpence.ExpenceAmount == 0) return;


            if (SelectedExpence.Id == 0)
            {
                await _service.Expense.AddAsync(SelectedExpence);
            }
            else
            {
                await _service.Expense.UpdateAsync(SelectedExpence);
            }

            await LoadExpencesAsync();
            await LoadPeopleAsync();

            SelectedExpence = new ExpenceModel();
            State = AppState.Observe;
            OnPropertyChanged(nameof(ActionButtonText));
        }
        private async Task OnDeleteAsync(ExpenceModel expence)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Are you sure to delete {expence.Person.Name} => {expence.Description}?", "Yes", "No");
            if (!confirm) return;

            _service.Expense.DeleteAsync(expence);
            Expences.Remove(expence);
        }
        private void OnResultCancel()
        {
            State = AppState.Observe;
            Result = [];
        }
        private void OnActionCancel()
        {
            State = AppState.Observe;
            SelectedExpence = new ExpenceModel();
        }
        #endregion
    }
}
