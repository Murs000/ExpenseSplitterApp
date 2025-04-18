using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using ExpenseSplitterApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ExpenseSplitterApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IUnitOfWork _unitOfWork;
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

        private List<Loan> _result = [];
        public List<Loan> Result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(nameof(Result)); }
        }

        private bool _isResultVisible = false;
        public bool IsResultVisible
        {
            get => _isResultVisible;
            set { _isResultVisible = value; OnPropertyChanged(nameof(IsResultVisible)); }
        }

        private bool _isEntryVisible = false;
        public bool IsEntryVisible
        {
            get => _isEntryVisible;
            set { _isEntryVisible = value; OnPropertyChanged(nameof(IsEntryVisible)); }
        }
        private bool _isPlusVisible = true;
        public bool IsPlusVisible
        {
            get => _isPlusVisible;
            set { _isPlusVisible = value; OnPropertyChanged(nameof(IsPlusVisible)); }
        }

        public string ActionButtonText => SelectedExpence?.Id == 0 ? "Add" : "Update";

        // Commands
        public ICommand AddExpenseCommand { get; }
        public ICommand ActionCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ResultCancelCommand { get; }
        public ICommand ActionCancelCommand { get; }
        public ICommand ShowEntryCommand { get; }

        public MainPageViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // Example people for testing

            ActionCommand = new Command(async () => await OnActionAsync());
            CalculateCommand = new Command(async () => await OnCalculateAsync());
            ResultCancelCommand = new Command(OnResultCancel);
            ActionCancelCommand = new Command(OnActionCancel);
            ShowEntryCommand = new Command(OnShowEntry);

            _ = LoadExpencesAsync();
            _ = LoadPeopleAsync();
        }
        private async Task OnCalculateAsync()
        {
            var calculator = new SplitCalculatorService(_unitOfWork);
            Result = await calculator.CalculateAsync();

            IsPlusVisible = false;
            IsResultVisible = true;
            IsEntryVisible = false;
        }
        private void OnShowEntry()
        {
            IsPlusVisible = false;
            IsEntryVisible = true;
            IsResultVisible = false;

            _ = LoadPeopleAsync();
        }

        private async Task LoadExpencesAsync()
        {
            var expenses = await _unitOfWork.Expenses.GetAllAsync("Person");
            Expences.Clear();
            foreach (var expence in expenses)
                Expences.Add(expence);
        }

        private async Task LoadPeopleAsync()
        {
            var people = await _unitOfWork.People.GetAllAsync();
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
                var newExpence = new ExpenceModel 
                { 
                    Description = SelectedExpence.Description,
                    ExpenceAmount = SelectedExpence.ExpenceAmount,
                    PersonId = SelectedExpence.Person.Id
                };
                await _unitOfWork.Expenses.AddAsync(newExpence);
            }
            else
            {
                var trackedExpence = await _unitOfWork.Expenses.GetByIdAsync(SelectedExpence.Id);

                trackedExpence.Description = SelectedExpence.Description;
                trackedExpence.ExpenceAmount = SelectedExpence.ExpenceAmount;
                trackedExpence.PersonId = SelectedExpence.PersonId;

                _unitOfWork.Expenses.Update(trackedExpence);
            }

            await _unitOfWork.SaveAsync();
            await LoadExpencesAsync();
            await LoadPeopleAsync();

            SelectedExpence = new ExpenceModel();
            IsEntryVisible = false;
            IsPlusVisible = true;
            OnPropertyChanged(nameof(ActionButtonText));
        }

        private void OnResultCancel()
        {
            IsResultVisible = false;
            IsPlusVisible = true;
            Result = [];
        }
        private void OnActionCancel()
        {
            IsEntryVisible = false;
            IsPlusVisible = true;
            SelectedExpence = new ExpenceModel();
        }

        // Property Changed Helpers
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
