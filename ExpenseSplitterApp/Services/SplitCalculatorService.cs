using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using ExpenseSplitterApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseSplitterApp.Services
{
    public class SplitCalculatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SplitCalculatorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Loan>> CalculateAsync()
        {
            var expences = await _unitOfWork.Expenses.GetAllAsync("Person");
            var people = await _unitOfWork.People.GetAllAsync();

            var totalSpent = expences.Sum(e => e.ExpenceAmount);
            var perPersonShare = totalSpent / people.Count();

            var personSpending = people.ToDictionary(
                p => p.Id,
                p => expences.Where(e => e.PersonId == p.Id).Sum(e => e.ExpenceAmount)
            );

            List<Loan> loans = [];

            foreach (var person in people)
            {
                var paid = personSpending[person.Id];
                var balance = paid - perPersonShare;

                Loan loan = new Loan();

                if (balance > 0)
                {
                    loan.PersonName = person.Name;
                    loan.IsRecive = true;
                    loan.MoneyAmount = balance;
                }
                else if (balance < 0)
                {
                    loan.PersonName = person.Name;
                    loan.IsRecive = false;
                    loan.MoneyAmount = balance * -1;
                }
                else
                {
                    loan.PersonName = person.Name;
                    loan.IsRecive = false;
                    loan.MoneyAmount = balance;
                }
                loans.Add(loan);
            }
            return loans;
        }
    }
}
