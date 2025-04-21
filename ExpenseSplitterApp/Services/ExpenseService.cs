using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Services
{
    public class ExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ExpenceModel>> GetAllAsync()
        {
            return await _unitOfWork.Expenses.GetAllAsync("Person");
        }
        public async Task<ExpenceModel> GetAsync(int id)
        {
            return await _unitOfWork.Expenses.GetByIdAsync(id);
        }
        public async Task AddAsync(ExpenceModel expence)
        {
            var newExpence = new ExpenceModel
            {
                Description = expence.Description,
                ExpenceAmount = expence.ExpenceAmount,
                PersonId = expence.Person.Id
            };
            await _unitOfWork.Expenses.AddAsync(newExpence);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateAsync(ExpenceModel expence)
        {
            var trackedExpence = await _unitOfWork.Expenses.GetByIdAsync(expence.Id);
            var newPerson = await _unitOfWork.People.GetByIdAsync(expence.Person.Id);

            trackedExpence.Description = expence.Description;
            trackedExpence.ExpenceAmount = expence.ExpenceAmount;
            trackedExpence.PersonId = expence.Person.Id;
            trackedExpence.Person = newPerson;

            _unitOfWork.Expenses.Update(trackedExpence);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(ExpenceModel expence)
        {
            var trackedExpence = await _unitOfWork.Expenses.GetByIdAsync(expence.Id);
            _unitOfWork.Expenses.Remove(trackedExpence);
            await _unitOfWork.SaveAsync();
        }
    }
}
