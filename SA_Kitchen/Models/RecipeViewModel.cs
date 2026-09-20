namespace SA_Kitchen.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string? CuisineType { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? RecipeImageUrl { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserProfilePicture { get; set; } = string.Empty;
        public int TotalTime => PrepTime + CookTime;
    }
}
