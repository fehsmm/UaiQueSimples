namespace UaiQueSimples.Web.Models;

public class Comment
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}