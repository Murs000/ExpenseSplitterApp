using ExpenseSplitterApp.Views;

namespace ExpenseSplitterApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PersonPage), typeof(PersonPage));
        }
    }
}
