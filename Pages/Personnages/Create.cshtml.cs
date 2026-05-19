using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeCollection.Pages.Personnages;

public class CreateModel : PageModel
{
    private readonly PersonnageService _personnageService;
    private readonly AnimeService _animeService;

    public CreateModel(
        PersonnageService personnageService,
        AnimeService animeService)
    {
        _personnageService = personnageService;
        _animeService = animeService;
    }

    [BindProperty]
    public Personnage Personnage { get; set; } = new();

    public List<SelectListItem> AnimeOptions { get; set; } = new();

    public List<SelectListItem> StatusOptions { get; set; } = new();

	[BindProperty]
	public IFormFile? Image { get; set; }

    public void OnGet()
    {
        LoadOptions();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadOptions();
            return Page();
        }
	if (Image is not null && Image.Length > 0)
	{
    	var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
    	var extension = Path.GetExtension(Image.FileName).ToLower();

    	if (!allowedExtensions.Contains(extension))
    	{
        	LoadOptions();
        	ModelState.AddModelError("Image", "Type d'image non autorisé");
        	return Page();
    	}

    	if (Image.Length > 5 * 1024 * 1024)
    	{
        	LoadOptions();
        	ModelState.AddModelError("Image", "L'image est trop volumineuse (max 5 MB)");
        	return Page();
    	}

    	var uploadsDir = Path.Combine("wwwroot", "uploads");
    	Directory.CreateDirectory(uploadsDir);

    	var fileName = $"{Guid.NewGuid()}{extension}";
    	var filePath = Path.Combine(uploadsDir, fileName);

    	using var stream = new FileStream(filePath, FileMode.Create);
    	Image.CopyTo(stream);

    	Personnage.ImagePath = $"/uploads/{fileName}";
	}

        _personnageService.Create(Personnage);

        TempData["Message"] = "Personnage ajouté avec succès";

        return RedirectToPage("Index");
    }

    private void LoadOptions()
    {
        AnimeOptions = _animeService
            .GetAll()
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title
            })
            .ToList();

        StatusOptions =
        [
            new SelectListItem { Value = "Vivant", Text = "Vivant" },
            new SelectListItem { Value = "Décédé", Text = "Décédé" },
            new SelectListItem { Value = "Inconnu", Text = "Inconnu" }
        ];
    }
}