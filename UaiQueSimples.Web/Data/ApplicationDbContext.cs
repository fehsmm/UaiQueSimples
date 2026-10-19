using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UaiQueSimples.Web.Models;

namespace UaiQueSimples.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Difficulty> Difficulties => Set<Difficulty>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<UnitOfMeasure> Units => Set<UnitOfMeasure>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Comment>()
            .Property(c => c.Rating).HasDefaultValue(0);
        builder.Entity<Recipe>()
            .Ignore(r => r.AverageRating);
        builder.Entity<RecipeIngredient>()
            .Property(ri => ri.Quantity).HasPrecision(10, 2);
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Quitandas" },
            new Category { Id = 2, Name = "Pratos Principais" },
            new Category { Id = 3, Name = "Acompanhamentos" },
            new Category { Id = 4, Name = "Sopas e Caldos" },
            new Category { Id = 5, Name = "Doces e Sobremesas" },
            new Category { Id = 6, Name = "Petiscos" });
        builder.Entity<Difficulty>().HasData(
            new Difficulty { Id = 1, Name = "Fácil" },
            new Difficulty { Id = 2, Name = "Médio" },
            new Difficulty { Id = 3, Name = "Difícil" });
        builder.Entity<UnitOfMeasure>().HasData(
            new UnitOfMeasure { Id = 1, Name = "Colher de sopa" },
            new UnitOfMeasure { Id = 2, Name = "Colher de chá" },
            new UnitOfMeasure { Id = 3, Name = "Xícara" },
            new UnitOfMeasure { Id = 4, Name = "Copo" },
            new UnitOfMeasure { Id = 5, Name = "Grama" },
            new UnitOfMeasure { Id = 6, Name = "Quilo" },
            new UnitOfMeasure { Id = 7, Name = "Litro" },
            new UnitOfMeasure { Id = 8, Name = "Mililitro" },
            new UnitOfMeasure { Id = 9, Name = "Pitada" });
        builder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 1, Name = "Sal" },
            new Ingredient { Id = 2, Name = "Açúcar" },
            new Ingredient { Id = 3, Name = "Farinha" },
            new Ingredient { Id = 4, Name = "Ovo" },
            new Ingredient { Id = 5, Name = "Leite" },
            new Ingredient { Id = 6, Name = "Manteiga" },
            new Ingredient { Id = 7, Name = "Creme de leite" },
            new Ingredient { Id = 8, Name = "Água" },
            new Ingredient { Id = 9, Name = "Azeite de oliva" },
            new Ingredient { Id = 10, Name = "Queijo Minas" },
            new Ingredient { Id = 11, Name = "Queijo Mussarela" },
            new Ingredient { Id = 12, Name = "Queijo Cottage" },
            new Ingredient { Id = 13, Name = "Polvilho Azedo" },
            new Ingredient { Id = 14, Name = "Polvilho Doce" },
            new Ingredient {Id = 15, Name = "Farinha de Trigo" },
            new Ingredient { Id = 16, Name = "Farinha de Mandioca" },
            new Ingredient { Id = 17, Name = "Farinha de Milho" },
            new Ingredient { Id = 18, Name = "Pó Royal" },
            new Ingredient { Id=19, Name = "Goiabada" },
            new Ingredient { Id = 20, Name = "Fubá" });
    }
}
