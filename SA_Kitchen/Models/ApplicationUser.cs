using Microsoft.AspNetCore.Identity;

namespace SA_Kitchen.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Required properties (non-nullable with defaults)
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // ALL optional properties must be nullable with '?'
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? FavoriteCuisine { get; set; }

        // Boolean with default value
        public bool IsAdmin { get; set; } = false;

        // Navigation property for recipes
        public virtual ICollection<Recipe>? Recipes { get; set; }
    }
}