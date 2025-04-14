using ExpenseSplitterApp.Models;
using ExpenseSplitterApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseSplitterApp.Commands
{
    public class SplitCalculatorCommand : ICommand
    {
        public readonly List<PersonModel> _persons = [];
        public readonly List<ExpenceModel> _expences = [];
        private readonly MainPageViewModel _mainPageViewModel;
        public SplitCalculatorCommand(MainPageViewModel mainPageViewModel, List<PersonModel> personModels, List<ExpenceModel> expenceModels )
        {
            _persons = personModels;
            _expences = expenceModels;
            _mainPageViewModel = mainPageViewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var totalSpent = _expences.Sum(e => e.ExpenceAmount);
            var perPersonShare = totalSpent / _persons.Count;

            var personSpending = _persons.ToDictionary(
                p => p.Id,
                p => _expences.Where(e => e.PersonId == p.Id).Sum(e => e.ExpenceAmount)
            );

            List<Loan> loans = [];

            foreach (var person in _persons)
            {
                var paid = personSpending[person.Id];
                var balance = paid - perPersonShare;

                Loan loan = new Loan(); 

                if (balance > 0)
                {
                    loan.PersonId = person.Id;
                    loan.IsRecive = true;
                    loan.MoneyAmount = balance;
                }
                else if (balance < 0)
                {
                    loan.PersonId = person.Id;
                    loan.IsRecive = false;
                    loan.MoneyAmount = balance * -1;
                }
                else
                {
                    loan.PersonId = person.Id;
                    loan.IsRecive = false;
                    loan.MoneyAmount = balance;
                }
                loans.Add(loan);
            }
            _mainPageViewModel.Result = loans;
            _mainPageViewModel.IsResultVisible = true;
        }
    }
}
