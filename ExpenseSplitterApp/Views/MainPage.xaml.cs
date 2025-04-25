using ExpenseSplitterApp.ViewModels;
using ExpenseSplitterApp.Views;
using Microsoft.Maui.Controls.Shapes;

namespace ExpenseSplitterApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }
        private async void OnGoToSecondPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.PersonPage));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadData();
        }
    }
}
