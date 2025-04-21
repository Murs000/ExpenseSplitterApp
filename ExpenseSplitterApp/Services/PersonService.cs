using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Services
{
    public class PersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PersonModel>> GetAllAsync()
        {
            return await _unitOfWork.People.GetAllAsync();
        }
        public async Task<PersonModel> GetAsync(int id)
        {
            return await _unitOfWork.People.GetByIdAsync(id);
        }
        public async Task AddAsync(PersonModel person)
        {
            await _unitOfWork.People.AddAsync(person);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateAsync(PersonModel person)
        {
            var trackedPerson = await _unitOfWork.People.GetByIdAsync(person.Id);
            trackedPerson.Name = person.Name;

            _unitOfWork.People.Update(trackedPerson);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(PersonModel person) 
        {
            var trackedPerson = await _unitOfWork.People.GetByIdAsync(person.Id);
            _unitOfWork.People.Remove(trackedPerson);
            await _unitOfWork.SaveAsync();
        }
    }
}
