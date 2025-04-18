using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Models
{
    public class Loan
    {
        public string PersonName { get; set; } = string.Empty;
        public bool IsRecive { get; set; }
        public double MoneyAmount { get; set; }
    }
}
