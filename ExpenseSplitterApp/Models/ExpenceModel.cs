using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Models
{
    public class ExpenceModel
    {
        public int Id { get; set; }
        public double ExpenceAmount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int PersonId { get; set; }
    }
}
