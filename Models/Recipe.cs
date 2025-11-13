namespace RecipeMood.Models;

public class Recipe
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Dessert, Main, Snack
    public string Difficulty { get; set; } = "Easy"; // Easy, Medium, Hard
    public List<string> Ingredients { get; set; } = new();
}
