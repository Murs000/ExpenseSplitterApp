using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Models
{
    public class Loan
    {
        public int PersonId { get; set; }
        public bool IsRecive { get; set; }
        public double MoneyAmount { get; set; }
    }
}
