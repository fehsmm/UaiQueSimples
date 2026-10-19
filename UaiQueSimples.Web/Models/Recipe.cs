namespace UaiQueSimples.Web.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string PreparationMethod { get; set; } = string.Empty;
    public int PreparationTimeMinutes { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int DifficultyId { get; set; }
    public Difficulty? Difficulty { get; set; }
    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public double? AverageRating { get; set; } // computed in queries
    public bool IsApproved { get; set; } = false; // pending approval by Admin 
    public string? CreatedByUserId { get; set; }
    public string? ImagePath { get; set; }
}