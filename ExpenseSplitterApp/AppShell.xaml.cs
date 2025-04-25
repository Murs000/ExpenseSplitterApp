using ExpenseSplitterApp.Views;

namespace ExpenseSplitterApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PersonPage), typeof(PersonPage));
            SetInitialTheme();
        }
        private void OnThemeRadioCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
                return;

            var selectedRadio = sender as RadioButton;
            var selectedValue = selectedRadio?.Value?.ToString();

            switch (selectedValue)
            {
                case "Dark":
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    Preferences.Set("AppTheme", "Dark");
                    break;

                case "Light":
                    Application.Current.UserAppTheme = AppTheme.Light;
                    Preferences.Set("AppTheme", "Light");
                    break;

                case "Default":
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    Preferences.Set("AppTheme", "Default");
                    break;
            }
        }
        private void SetInitialTheme()
        {
            var darkRadioButton = this.FindByName<RadioButton>("DarkRadio");
            var lightRadioButton = this.FindByName<RadioButton>("LightRadio");
            var defaultRadioButton = this.FindByName<RadioButton>("DefaultRadio");

            // Ensure the controls are loaded and accessible
            switch (Application.Current.UserAppTheme)
            {
                case AppTheme.Dark:
                    darkRadioButton.IsChecked = true;
                    break;
                case AppTheme.Light:
                    lightRadioButton.IsChecked = true;
                    break;
                default:
                    defaultRadioButton.IsChecked = true;
                    break;
            }
        }
    }
}
