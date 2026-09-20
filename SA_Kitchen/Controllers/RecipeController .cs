using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA_Kitchen.Data;
using SA_Kitchen.Models;
using SA_Kitchen.ViewModels;

namespace SA_Kitchen.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecipeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Recipe/Index
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var recipes = await _context.Recipes
                .Include(r => r.User)
                .Where(r => r.IsPublic || r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RecipeViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Ingredients = r.Ingredients,
                    Instructions = r.Instructions,
                    PrepTime = r.PrepTime,
                    CookTime = r.CookTime,
                    Servings = r.Servings,
                    CuisineType = r.CuisineType,
                    DifficultyLevel = r.DifficultyLevel,
                    RecipeImageUrl = r.RecipeImageUrl,
                    IsPublic = r.IsPublic,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    UserId = r.UserId,
                    UserName = r.User != null ? $"{r.User.FirstName} {r.User.LastName}" : "Unknown User",
                    UserProfilePicture = r.User != null ? r.User.ProfilePictureUrl ?? "/images/default-profile.png" : "/images/default-profile.png"
                })
                .ToListAsync();

            return View(recipes);
        }

        // GET: Recipe/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var recipe = new Recipe
                {
                    Title = model.Title,
                    Description = model.Description,
                    Ingredients = model.Ingredients,
                    Instructions = model.Instructions,
                    PrepTime = model.PrepTime,
                    CookTime = model.CookTime,
                    Servings = model.Servings,
                    CuisineType = model.CuisineType,
                    DifficultyLevel = model.DifficultyLevel,
                    IsPublic = model.IsPublic,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now
                };

                // Handle image upload
                if (model.RecipeImage != null && model.RecipeImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "recipes");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now.Ticks}{Path.GetExtension(model.RecipeImage.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.RecipeImage.CopyToAsync(stream);
                    }

                    recipe.RecipeImageUrl = $"/images/recipes/{uniqueFileName}";
                }

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your recipe has been created successfully!";
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }

            return View(model);
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!recipe.IsPublic && recipe.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var model = new RecipeViewModel
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients,
                Instructions = recipe.Instructions,
                PrepTime = recipe.PrepTime,
                CookTime = recipe.CookTime,
                Servings = recipe.Servings,
                CuisineType = recipe.CuisineType,
                DifficultyLevel = recipe.DifficultyLevel,
                RecipeImageUrl = recipe.RecipeImageUrl,
                IsPublic = recipe.IsPublic,
                CreatedAt = recipe.CreatedAt,
                UpdatedAt = recipe.UpdatedAt,
                UserId = recipe.UserId,
                UserName = recipe.User != null ? $"{recipe.User.FirstName} {recipe.User.LastName}" : "Unknown User",
                UserProfilePicture = recipe.User != null ? recipe.User.ProfilePictureUrl ?? "/images/default-profile.png" : "/images/default-profile.png"
            };

            return View(model);
        }

        // GET: Recipe/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (recipe.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var model = new EditRecipeViewModel
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients,
                Instructions = recipe.Instructions,
                PrepTime = recipe.PrepTime,
                CookTime = recipe.CookTime,
                Servings = recipe.Servings,
                CuisineType = recipe.CuisineType,
                DifficultyLevel = recipe.DifficultyLevel,
                IsPublic = recipe.IsPublic,
                RecipeImageUrl = recipe.RecipeImageUrl
            };

            return View(model);
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRecipeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var recipe = await _context.Recipes.FindAsync(id);

                if (recipe == null)
                {
                    return NotFound();
                }

                var userId = _userManager.GetUserId(User);
                if (recipe.UserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                recipe.Title = model.Title;
                recipe.Description = model.Description;
                recipe.Ingredients = model.Ingredients;
                recipe.Instructions = model.Instructions;
                recipe.PrepTime = model.PrepTime;
                recipe.CookTime = model.CookTime;
                recipe.Servings = model.Servings;
                recipe.CuisineType = model.CuisineType;
                recipe.DifficultyLevel = model.DifficultyLevel;
                recipe.IsPublic = model.IsPublic;
                recipe.UpdatedAt = DateTime.Now;

                if (model.RecipeImage != null && model.RecipeImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "recipes");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    if (!string.IsNullOrEmpty(recipe.RecipeImageUrl))
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, recipe.RecipeImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now.Ticks}{Path.GetExtension(model.RecipeImage.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.RecipeImage.CopyToAsync(stream);
                    }

                    recipe.RecipeImageUrl = $"/images/recipes/{uniqueFileName}";
                }

                _context.Update(recipe);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your recipe has been updated successfully!";
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }

            return View(model);
        }

        // GET: Recipe/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (recipe.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (recipe.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (!string.IsNullOrEmpty(recipe.RecipeImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, recipe.RecipeImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Recipe deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Recipe/MyRecipes
        [Authorize]
        public async Task<IActionResult> MyRecipes()
        {
            var userId = _userManager.GetUserId(User);

            var recipes = await _context.Recipes
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RecipeViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Ingredients = r.Ingredients,
                    Instructions = r.Instructions,
                    PrepTime = r.PrepTime,
                    CookTime = r.CookTime,
                    Servings = r.Servings,
                    CuisineType = r.CuisineType,
                    DifficultyLevel = r.DifficultyLevel,
                    RecipeImageUrl = r.RecipeImageUrl,
                    IsPublic = r.IsPublic,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    UserId = r.UserId,
                    UserName = r.User != null ? $"{r.User.FirstName} {r.User.LastName}" : "Unknown User",
                    UserProfilePicture = r.User != null ? r.User.ProfilePictureUrl ?? "/images/default-profile.png" : "/images/default-profile.png"
                })
                .ToListAsync();

            return View(recipes);
        }
    }
}