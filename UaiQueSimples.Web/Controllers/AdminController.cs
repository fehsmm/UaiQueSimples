using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UaiQueSimples.Web.Data;

namespace UaiQueSimples.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var pending = await _db.Recipes
            .Include(r => r.Category)
            .Include(r => r.Difficulty)
            .Where(r => !r.IsApproved)
            .ToListAsync();
        return View(pending);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var recipe = await _db.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();
        recipe.IsApproved = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var recipe = await _db.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();
        _db.Recipes.Remove(recipe);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}