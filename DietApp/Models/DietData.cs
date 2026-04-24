namespace DietApp.Models;

public class DietData
{
    public Goal? Goal { get; set; }
    public List<WeightRecord> Records { get; set; } = new();
}
