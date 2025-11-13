using Microsoft.AspNetCore.Mvc;
using RecipeMood.Models;
using RecipeMood.Services;

namespace RecipeMood.Controllers;

public class MoodController : Controller
{
    private readonly MoodService _service;

    public MoodController(MoodService service) => _service = service;

    public IActionResult Index(MoodCategory? filterMood = null, string? searchNotes = null)
    {
        var entries = _service.GetAllEntries();

        if (filterMood.HasValue)
            entries = entries.Where(e => e.Mood == filterMood.Value).ToList();

        if (!string.IsNullOrEmpty(searchNotes))
            entries = entries.Where(e => e.Notes != null && e.Notes.Contains(searchNotes, StringComparison.OrdinalIgnoreCase)).ToList();

        return View(entries);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(MoodEntry entry)
    {
        _service.AddEntry(entry);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Stats()
    {
        var stats = _service.GetStats();
        return View(stats);
    }

    public IActionResult RecipeDetails(int id)
    {
        var entry = _service.GetAllEntries().FirstOrDefault(e => e.Id == id);
        if (entry == null)
            return NotFound();

        return View(entry);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _service.DeleteEntry(id);
        return RedirectToAction(nameof(Index));
    }
}
