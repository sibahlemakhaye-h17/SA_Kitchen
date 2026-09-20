using System.ComponentModel.DataAnnotations;

namespace SA_Kitchen.ViewModels
{
    public class EditRecipeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ingredients")]
        public string Ingredients { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; } = string.Empty;

        [Display(Name = "Prep Time (minutes)")]
        [Range(1, 1440)]
        public int PrepTime { get; set; }

        [Display(Name = "Cook Time (minutes)")]
        [Range(1, 1440)]
        public int CookTime { get; set; }

        [Display(Name = "Servings")]
        [Range(1, 100)]
        public int Servings { get; set; }

        [Display(Name = "Cuisine Type")]
        public string? CuisineType { get; set; }

        [Display(Name = "Difficulty Level")]
        public string? DifficultyLevel { get; set; }

        [Display(Name = "Is Public")]
        public bool IsPublic { get; set; } = true;

        [Display(Name = "Recipe Image")]
        public string? RecipeImageUrl { get; set; }

        [Display(Name = "Upload New Image")]
        public IFormFile? RecipeImage { get; set; }
    }
}
