namespace ExpenseSplitterApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string savedTheme = Preferences.Get("AppTheme", "Default");

            Application.Current.UserAppTheme = savedTheme switch
            {
                "Dark" => AppTheme.Dark,
                "Light" => AppTheme.Light,
                _ => AppTheme.Unspecified
            };

            MainPage = new AppShell();
        }
    }
}
