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