using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SA_Kitchen.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ingredients")]
        public string Ingredients { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; } = string.Empty;

        [Display(Name = "Prep Time (minutes)")]
        [Range(1, 1440, ErrorMessage = "Prep time must be between 1 and 1440 minutes")]
        public int PrepTime { get; set; }

        [Display(Name = "Cook Time (minutes)")]
        [Range(1, 1440, ErrorMessage = "Cook time must be between 1 and 1440 minutes")]
        public int CookTime { get; set; }

        [Display(Name = "Servings")]
        [Range(1, 100, ErrorMessage = "Servings must be between 1 and 100")]
        public int Servings { get; set; }

        [Display(Name = "Cuisine Type")]
        public string? CuisineType { get; set; }

        [Display(Name = "Difficulty Level")]
        public string? DifficultyLevel { get; set; } // Easy, Medium, Hard

        [Display(Name = "Recipe Image")]
        public string? RecipeImageUrl { get; set; }

        [Display(Name = "Is Public")]
        public bool IsPublic { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedAt { get; set; }

        // Foreign Key
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        // Navigation properties for social features (will add later)
        // public ICollection<Comment>? Comments { get; set; }
        // public ICollection<Like>? Likes { get; set; }
        // public ICollection<Save>? Saves { get; set; }
    }
}