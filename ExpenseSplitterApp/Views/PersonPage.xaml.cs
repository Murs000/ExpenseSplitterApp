using ExpenseSplitterApp.ViewModels;

namespace ExpenseSplitterApp.Views;

public partial class PersonPage : ContentPage
{
	public PersonPage(PersonViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Re-enable flyout when leaving this page
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
    }
}