using ExpenseSplitterApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.ViewModels
{
    public class MainPageViewModel
    {
        public ObservableCollection<PersonModel> Persons { get; set; }
        public ObservableCollection<ExpenceModel> Expences { get; set; }

        public MainPageViewModel() 
        {
            Persons = new ObservableCollection<PersonModel>
            {
                new PersonModel { Id = 1, Name ="1"},
                new PersonModel { Id = 2, Name ="2"},
                new PersonModel { Id = 3, Name ="3"},
                new PersonModel { Id = 4, Name ="4"}
            };
            Expences = new ObservableCollection<ExpenceModel>
            {
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 14.99, PersonId = 1 },
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 17.98, PersonId = 1 },
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 13.30, PersonId = 1 },
                new ExpenceModel { Id = 1, Description = "Food", ExpenceAmount = 14.99, PersonId = 2 }
            };
        }
    }
}
