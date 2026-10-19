namespace UaiQueSimples.Web.Models;

public class Favorite
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int RecipeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}