using System.ComponentModel.DataAnnotations;

namespace SA_Kitchen.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Bio")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ProfilePictureUrl { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "Province")]
        public string? Province { get; set; }

        [Display(Name = "Favorite Cuisine")]
        public string? FavoriteCuisine { get; set; }

        [Display(Name = "Member Since")]
        public DateTime CreatedAt { get; set; }

        // For profile picture upload
        public IFormFile? ProfileImage { get; set; }
    }
}