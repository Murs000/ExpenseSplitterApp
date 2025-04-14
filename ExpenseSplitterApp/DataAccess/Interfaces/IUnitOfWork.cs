using ExpenseSplitterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<PersonModel> People { get; }
        IRepository<ExpenceModel> Expenses { get; }
        Task<int> SaveAsync();
    }
}
