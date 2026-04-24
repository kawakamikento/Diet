using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DietApp.Models;
using DietApp.Services;

namespace DietApp.ViewModels;

public partial class ProgressViewModel : ObservableObject
{
    private readonly DataService _dataService;

    [ObservableProperty] private double inputWeight;
    [ObservableProperty] private string summary = "目標が未設定です。目標設定タブから入力してください。";
    [ObservableProperty] private string statusMessage = string.Empty;

    public ObservableCollection<WeightRecord> RecentRecords { get; } = new();

    public ProgressViewModel(DataService dataService)
    {
        _dataService = dataService;
    }

    public async Task InitializeAsync()
    {
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task RecordAsync()
    {
        if (InputWeight <= 0)
        {
            StatusMessage = "体重は正の値で入力してください。";
            return;
        }

        var data = await _dataService.LoadAsync();
        var today = DateTime.Today;
        var existing = data.Records.FirstOrDefault(r => r.Date == today);
        if (existing is not null)
        {
            existing.Weight = InputWeight;
        }
        else
        {
            data.Records.Add(new WeightRecord { Date = today, Weight = InputWeight });
        }
        await _dataService.SaveAsync(data);
        StatusMessage = $"{today:yyyy-MM-dd} の体重を {InputWeight:0.0} kg で記録しました。";
        await RefreshAsync();
    }

    [RelayCommand]
    private Task RefreshCommandAsync() => RefreshAsync();

    private async Task RefreshAsync()
    {
        var data = await _dataService.LoadAsync();

        RecentRecords.Clear();
        foreach (var r in data.Records.OrderByDescending(r => r.Date).Take(10))
            RecentRecords.Add(r);

        if (data.Goal is null)
        {
            Summary = "目標が未設定です。目標設定タブから入力してください。";
            return;
        }

        var goal = data.Goal;
        var latest = data.Records.OrderByDescending(r => r.Date).FirstOrDefault();
        var currentWeight = latest?.Weight ?? goal.StartWeight;

        var remainingKg = currentWeight - goal.TargetWeight;
        var totalDays = Math.Max(1, (goal.TargetDate - goal.StartDate).Days);
        var elapsedDays = Math.Max(0, (DateTime.Today - goal.StartDate).Days);
        var daysLeft = Math.Max(0, (goal.TargetDate - DateTime.Today).Days);

        var totalToLose = goal.StartWeight - goal.TargetWeight;
        var actualLoss = goal.StartWeight - currentWeight;
        var progressPct = totalToLose == 0
            ? 0
            : Math.Clamp(actualLoss / totalToLose * 100.0, 0.0, 100.0);

        Summary =
            $"現在の体重: {currentWeight:0.0} kg / 目標: {goal.TargetWeight:0.0} kg\n" +
            $"目標まで残り: {remainingKg:+0.0;-0.0;0.0} kg\n" +
            $"経過: {elapsedDays} 日 / 残り: {daysLeft} 日 (全 {totalDays} 日)\n" +
            $"進捗率: {progressPct:0.0} %";
    }
}
