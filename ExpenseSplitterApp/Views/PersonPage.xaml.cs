using ExpenseSplitterApp.ViewModels;

namespace ExpenseSplitterApp.Views;

public partial class PersonPage : ContentPage
{
	public PersonPage(PersonViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}