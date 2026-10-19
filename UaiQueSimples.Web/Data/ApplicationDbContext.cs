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
    }
}