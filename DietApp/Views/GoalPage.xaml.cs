using DietApp.ViewModels;

namespace DietApp.Views;

public partial class GoalPage : ContentPage
{
    private readonly GoalViewModel _viewModel;

    public GoalPage(GoalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
