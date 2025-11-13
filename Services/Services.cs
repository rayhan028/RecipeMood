using RecipeMood.Models;

namespace RecipeMood.Services;

public class MoodService
{
    private readonly List<MoodEntry> _entries = new();
    private int _nextId = 1;

    private static readonly Dictionary<MoodCategory, List<Recipe>> MoodRecipeMap = new()
    {
        { MoodCategory.Happy, new() {
            new Recipe { Name="Chocolate Cake", Type="Dessert", Difficulty="Medium", Ingredients=new(){"Flour","Cocoa","Sugar","Eggs"} },
            new Recipe { Name="Fruit Salad", Type="Snack", Difficulty="Easy", Ingredients=new(){"Apple","Banana","Orange"} },
            new Recipe { Name="Pasta Primavera", Type="Main", Difficulty="Medium", Ingredients=new(){"Pasta","Vegetables","Olive Oil"} }
        }},
        { MoodCategory.Sad, new() {
            new Recipe { Name="Tomato Soup", Type="Main", Difficulty="Easy", Ingredients=new(){"Tomatoes","Onion","Garlic"} },
            new Recipe { Name="Warm Brownies", Type="Dessert", Difficulty="Medium", Ingredients=new(){"Flour","Cocoa","Butter"} },
            new Recipe { Name="Mashed Potatoes", Type="Side", Difficulty="Easy", Ingredients=new(){"Potatoes","Butter","Milk"} }
        }},
        { MoodCategory.Energetic, new() {
            new Recipe { Name="Protein Smoothie", Type="Snack", Difficulty="Easy", Ingredients=new(){"Milk","Protein Powder","Banana"} },
            new Recipe { Name="Grilled Chicken Salad", Type="Main", Difficulty="Medium", Ingredients=new(){"Chicken","Lettuce","Tomatoes"} },
            new Recipe { Name="Energy Bars", Type="Snack", Difficulty="Medium", Ingredients=new(){"Oats","Honey","Nuts"} }
        }},
        { MoodCategory.Calm, new() {
            new Recipe { Name="Herbal Tea", Type="Drink", Difficulty="Easy", Ingredients=new(){"Chamomile","Water"} },
            new Recipe { Name="Oatmeal Porridge", Type="Breakfast", Difficulty="Easy", Ingredients=new(){"Oats","Milk","Honey"} },
            new Recipe { Name="Baked Salmon", Type="Main", Difficulty="Medium", Ingredients=new(){"Salmon","Lemon","Herbs"} }
        }},
        { MoodCategory.Angry, new() {
            new Recipe { Name="Spicy Tacos", Type="Main", Difficulty="Medium", Ingredients=new(){"Tortilla","Beef","Chili"} },
            new Recipe { Name="Hot Wings", Type="Snack", Difficulty="Medium", Ingredients=new(){"Chicken Wings","Hot Sauce"} },
            new Recipe { Name="Chili Con Carne", Type="Main", Difficulty="Medium", Ingredients=new(){"Beef","Beans","Chili"} }
        }},
        { MoodCategory.Romantic, new() {
            new Recipe { Name="Strawberry Cheesecake", Type="Dessert", Difficulty="Hard", Ingredients=new(){"Cheese","Strawberry","Sugar"} },
            new Recipe { Name="Wine Risotto", Type="Main", Difficulty="Medium", Ingredients=new(){"Rice","Wine","Parmesan"} },
            new Recipe { Name="Chocolate Fondue", Type="Dessert", Difficulty="Medium", Ingredients=new(){"Chocolate","Cream","Fruits"} }
        }},
        { MoodCategory.Anxious, new() {
            new Recipe { Name="Green Smoothie", Type="Drink", Difficulty="Easy", Ingredients=new(){"Spinach","Apple","Water"} },
            new Recipe { Name="Yogurt Bowl", Type="Breakfast", Difficulty="Easy", Ingredients=new(){"Yogurt","Berries","Honey"} },
            new Recipe { Name="Steamed Veggies", Type="Side", Difficulty="Easy", Ingredients=new(){"Broccoli","Carrots","Peas"} }
        }},
        { MoodCategory.Creative, new() {
            new Recipe { Name="Sushi Rolls", Type="Main", Difficulty="Hard", Ingredients=new(){"Rice","Seaweed","Fish"} },
            new Recipe { Name="Gourmet Sandwich", Type="Snack", Difficulty="Medium", Ingredients=new(){"Bread","Cheese","Veggies"} },
            new Recipe { Name="Exotic Salad", Type="Main", Difficulty="Medium", Ingredients=new(){"Kale","Quinoa","Fruits"} }
        }}
    };

    public List<MoodEntry> GetAllEntries() => _entries.OrderByDescending(e => e.Date).ToList();

    public void AddEntry(MoodEntry entry)
    {
        entry.Id = _nextId++;
        entry.Date = DateTime.Now;

        if (MoodRecipeMap.ContainsKey(entry.Mood))
            entry.SuggestedRecipes = MoodRecipeMap[entry.Mood].Take(3).ToList();

        _entries.Add(entry);
    }

    public void DeleteEntry(int id) => _entries.RemoveAll(e => e.Id == id);

    public MoodStats GetStats()
    {
        if (!_entries.Any())
            return new MoodStats();

        var moodFreq = _entries.GroupBy(e => e.Mood)
                               .ToDictionary(g => g.Key.ToString(), g => g.Count());

        var recipeTypeFreq = _entries.SelectMany(e => e.SuggestedRecipes)
                                     .GroupBy(r => r.Type)
                                     .ToDictionary(g => g.Key, g => g.Count());

        var intensityTrend = _entries.GroupBy(e => e.Date.Date)
                                     .ToDictionary(g => g.Key, g => (int)Math.Round(g.Average(e => e.Intensity)));

        return new MoodStats
        {
            MostCommonMood = moodFreq.OrderByDescending(kvp => kvp.Value).First().Key,
            AverageIntensity = _entries.Average(e => e.Intensity),
            MoodFrequency = moodFreq,
            RecipeTypeFrequency = recipeTypeFreq,
            IntensityTrend = intensityTrend
        };
    }
}
