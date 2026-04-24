namespace DietApp.Models;

public class Goal
{
    public double StartWeight { get; set; }
    public double TargetWeight { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime TargetDate { get; set; } = DateTime.Today.AddMonths(3);
}
