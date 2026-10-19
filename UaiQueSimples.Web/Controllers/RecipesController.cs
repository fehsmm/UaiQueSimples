using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UaiQueSimples.Web.Data;
using UaiQueSimples.Web.Models;

namespace UaiQueSimples.Web.Controllers;

public class RecipesController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<RecipesController> _logger;

    public RecipesController(ApplicationDbContext db, ILogger<RecipesController> logger)
    {
        _db = db;
        _logger = logger;
    }

    // GET: /Recipes
    public async Task<IActionResult> Index(string? q, int? categoryId, int? difficultyId)
    {
        var query = _db.Recipes
            .Include(r => r.Category)
            .Include(r => r.Difficulty)
            .Include(r => r.Comments)
            .Where(r => r.IsApproved);

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(r =>
                r.Title.Contains(q) ||
                r.PreparationMethod.Contains(q));
        }

        if (categoryId.HasValue) query = query.Where(r => r.CategoryId == categoryId);
        if (difficultyId.HasValue) query = query.Where(r => r.DifficultyId == difficultyId);

        var list = await query
            .OrderBy(r => r.Title)
            .Select(r => new Recipe {
                Id = r.Id,
                Title = r.Title,
                Category = r.Category,
                Difficulty = r.Difficulty,
                PreparationTimeMinutes = r.PreparationTimeMinutes,
                AverageRating = r.Comments.Any() ? r.Comments.Average(c => c.Rating) : null
            }).ToListAsync();

        ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Difficulties = await _db.Difficulties.OrderBy(d => d.Name).ToListAsync();
        ViewBag.Query = q;
        return View(list);
    }

    // GET: /Recipes/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var recipe = await _db.Recipes
            .Include(r => r.Category)
            .Include(r => r.Difficulty)
            .Include(r => r.Ingredients).ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Ingredients).ThenInclude(ri => ri.UnitOfMeasure)
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == id && r.IsApproved);

        if (recipe == null) return NotFound();
        recipe.AverageRating = recipe.Comments.Any() ? recipe.Comments.Average(c => c.Rating) : null;
        return View(recipe);
    }

    // GET: /Recipes/Create
    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Categories = _db.Categories.OrderBy(c => c.Name).ToList();
        ViewBag.Difficulties = _db.Difficulties.OrderBy(d => d.Name).ToList();
        ViewBag.Ingredients = _db.Ingredients.OrderBy(i => i.Name).ToList();
        ViewBag.Units = _db.Units.OrderBy(u => u.Name).ToList();
        return View(new Recipe());
    }

    // POST: /Recipes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(Recipe recipe, int[] ingredientIds, decimal[] quantities, int[] unitIds)
    {
        if (ModelState.IsValid)
        {
            recipe.IsApproved = false; // pending
            recipe.CreatedByUserId = User?.Identity?.Name;
            _db.Recipes.Add(recipe);
            await _db.SaveChangesAsync();

            for (int i = 0; i < ingredientIds.Length; i++)
            {
                _db.RecipeIngredients.Add(new RecipeIngredient {
                    RecipeId = recipe.Id,
                    IngredientId = ingredientIds[i],
                    Quantity = i < quantities.Length ? quantities[i] : 0,
                    UnitOfMeasureId = i < unitIds.Length ? unitIds[i] : _db.Units.First().Id
                });
            }
            await _db.SaveChangesAsync();

            TempData["Message"] = "Receita enviada para aprovação. Obrigado!";
            return RedirectToAction(nameof(Index));
        }
        return View(recipe);
    }

    // POST: /Recipes/AddComment
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddComment(int recipeId, string text, int rating)
    {
        var recipe = await _db.Recipes.FindAsync(recipeId);
        if (recipe == null || !recipe.IsApproved) return NotFound();

        _db.Comments.Add(new Comment {
            RecipeId = recipeId,
            Username = User?.Identity?.Name ?? "user",
            Text = text,
            Rating = Math.Clamp(rating, 1, 5),
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = recipeId });
    }
}