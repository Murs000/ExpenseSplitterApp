using ExpenseSplitterApp.ViewModels;

namespace ExpenseSplitterApp.Views;

public partial class CreatePersonPage : ContentPage
{
	public CreatePersonPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}