using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public bool DebeCambiarPassword { get; set; } = false;
}
