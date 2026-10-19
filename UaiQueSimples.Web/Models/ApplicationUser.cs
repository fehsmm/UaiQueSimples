using Microsoft.AspNetCore.Identity;

namespace UaiQueSimples.Web.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd > DateTimeOffset.UtcNow;
}