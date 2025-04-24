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
        private void OnThemeSwitchToggled(object sender, EventArgs e)
        {
            var newTheme = Application.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

            Application.Current.UserAppTheme = newTheme;
            Preferences.Set("AppTheme", newTheme.ToString());
        }
    }
}
