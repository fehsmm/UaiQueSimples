namespace UaiQueSimples.Web.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Nome/Descrição
    public bool IsApproved { get; set; } = true;
    public string? CreatedByUserId { get; set; }
}