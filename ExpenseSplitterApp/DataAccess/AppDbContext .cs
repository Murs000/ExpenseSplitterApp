using ExpenseSplitterApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<PersonModel> People => Set<PersonModel>();
        public DbSet<ExpenceModel> Expenses => Set<ExpenceModel>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
