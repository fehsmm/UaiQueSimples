namespace UaiQueSimples.Web.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    public int IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
    public decimal Quantity { get; set; }
    public int UnitOfMeasureId { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}