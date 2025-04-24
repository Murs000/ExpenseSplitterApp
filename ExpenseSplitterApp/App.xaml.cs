namespace ExpenseSplitterApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var savedTheme = Preferences.Get("AppTheme", "Unspecified");
            Application.Current.UserAppTheme = Enum.TryParse(savedTheme, out AppTheme theme)
                ? theme
                : AppTheme.Light;

            MainPage = new AppShell();
        }
    }
}
