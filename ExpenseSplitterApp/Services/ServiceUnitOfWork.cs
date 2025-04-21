using ExpenseSplitterApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Services
{
    public class ServiceUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceUnitOfWork(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public ExpenseService Expense => new ExpenseService(_unitOfWork);
        public PersonService Person => new PersonService(_unitOfWork);
        public SplitCalculatorService SplitCalculatorService => new SplitCalculatorService(_unitOfWork);
    }
}
