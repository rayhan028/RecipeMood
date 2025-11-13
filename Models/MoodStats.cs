namespace RecipeMood.Models;

public class MoodStats
{
    public string MostCommonMood { get; set; } = string.Empty;
    public double AverageIntensity { get; set; }
    public Dictionary<string, int> MoodFrequency { get; set; } = new();
    public Dictionary<string, int> RecipeTypeFrequency { get; set; } = new();
    public Dictionary<DateTime, int> IntensityTrend { get; set; } = new();
}
