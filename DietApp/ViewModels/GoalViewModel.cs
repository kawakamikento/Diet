using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DietApp.Models;
using DietApp.Services;

namespace DietApp.ViewModels;

public partial class GoalViewModel : ObservableObject
{
    private readonly DataService _dataService;

    [ObservableProperty] private double startWeight;
    [ObservableProperty] private double targetWeight;
    [ObservableProperty] private DateTime startDate = DateTime.Today;
    [ObservableProperty] private DateTime targetDate = DateTime.Today.AddMonths(3);
    [ObservableProperty] private string statusMessage = string.Empty;

    public GoalViewModel(DataService dataService)
    {
        _dataService = dataService;
    }

    public async Task InitializeAsync()
    {
        var data = await _dataService.LoadAsync();
        if (data.Goal is { } goal)
        {
            StartWeight = goal.StartWeight;
            TargetWeight = goal.TargetWeight;
            StartDate = goal.StartDate;
            TargetDate = goal.TargetDate;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (StartWeight <= 0 || TargetWeight <= 0)
        {
            StatusMessage = "体重は正の値で入力してください。";
            return;
        }
        if (TargetDate <= StartDate)
        {
            StatusMessage = "目標日は開始日より後にしてください。";
            return;
        }

        var data = await _dataService.LoadAsync();
        data.Goal = new Goal
        {
            StartWeight = StartWeight,
            TargetWeight = TargetWeight,
            StartDate = StartDate,
            TargetDate = TargetDate
        };
        await _dataService.SaveAsync(data);
        StatusMessage = $"保存しました ({DateTime.Now:HH:mm:ss})";
    }
}
