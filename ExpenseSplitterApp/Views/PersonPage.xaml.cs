using ExpenseSplitterApp.ViewModels;
using Microsoft.Maui.Controls.Shapes;
using System.Reflection.Metadata.Ecma335;

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

    private SwipeView _currentlyOpenSwipeView;
    private const int AutoCloseDelayMilliseconds = 5000; // 5 seconds for demonstration (can be adjusted)

    private void OnSwipeEnded(object sender, SwipeEndedEventArgs e)
    {
        var endedSwipeView = sender as SwipeView;

        // If swipe is closed, reset border and set currentlyOpenSwipeView to null
        if (endedSwipeView != null && !e.IsOpen)
        {
            ResetBorder(endedSwipeView);

            // If this is the currently open SwipeView, reset the reference
            if (_currentlyOpenSwipeView == endedSwipeView)
                _currentlyOpenSwipeView = null;
        }

        // If swipe is open, update the currently open SwipeView and close the previous one
        if (endedSwipeView != null && e.IsOpen)
        {
            // Close previous open SwipeView, if any
            if (_currentlyOpenSwipeView != null && _currentlyOpenSwipeView != endedSwipeView)
            {
                _currentlyOpenSwipeView.Close();
                ResetBorder(_currentlyOpenSwipeView);
            }

            // Update the currently open SwipeView
            _currentlyOpenSwipeView = endedSwipeView;
            HighlightBorder(endedSwipeView);

            // Start the timer to auto-close after the specified delay
            Device.StartTimer(TimeSpan.FromMilliseconds(AutoCloseDelayMilliseconds), () =>
            {
                // Close the SwipeView after the delay
                endedSwipeView.Close();
                ResetBorder(endedSwipeView);

                // Return false to stop the timer after the action is triggered
                return false;
            });
        }
    }
    private void HighlightBorder(SwipeView swipeView)
    {
        if (swipeView.Content is Border border)
        {
            border.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12, 0, 12, 0) };
        }
    }

    private void ResetBorder(SwipeView swipeView)
    {
        if (swipeView.Content is Border border)
        {
            border.StrokeShape = new RoundRectangle { CornerRadius = 12 };
        }
    }
}