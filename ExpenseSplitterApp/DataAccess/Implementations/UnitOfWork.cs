using ExpenseSplitterApp.DataAccess.Interfaces;
using ExpenseSplitterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.DataAccess.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<PersonModel> People { get; }
        public IRepository<ExpenceModel> Expenses { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            People = new Repository<PersonModel>(context);
            Expenses = new Repository<ExpenceModel>(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
