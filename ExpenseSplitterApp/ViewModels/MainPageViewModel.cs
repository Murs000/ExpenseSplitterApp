using ExpenseSplitterApp.Commands;
using ExpenseSplitterApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseSplitterApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PersonModel> Persons { get; set; }
        public ObservableCollection<ExpenceModel> Expences { get; set; }

        private List<Loan> _result;
        public List<Loan> Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        private bool _isResultVisible;
        public bool IsResultVisible
        {
            get => _isResultVisible;
            set
            {
                _isResultVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand Calculate => new SplitCalculatorCommand(this,Persons.ToList(),Expences.ToList());

        public MainPageViewModel() 
        {
            Persons = new ObservableCollection<PersonModel>
            {
                new PersonModel { Id = 1, Name ="1"},
                new PersonModel { Id = 2, Name ="2"},
                new PersonModel { Id = 3, Name ="3"},
                new PersonModel { Id = 4, Name ="4"}
            };
            Expences = new ObservableCollection<ExpenceModel>
            {
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 14.99, PersonId = 1 },
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 17.98, PersonId = 1 },
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 13.30, PersonId = 1 },
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 14.99, PersonId = 2 }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
