using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeCollection.Pages.Personnages;

public class EditModel : PageModel
{
    private readonly PersonnageService _personnageService;
    private readonly AnimeService _animeService;

    public EditModel(
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

    public IActionResult OnGet(int id)
    {
        LoadOptions();

        var personnage = _personnageService.GetById(id);

        if (personnage is null)
        {
            return NotFound();
        }

        Personnage = personnage;

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadOptions();
            return Page();
        }

        var updated = _personnageService.Update(Personnage);

        if (!updated)
        {
            return NotFound();
        }

        TempData["Message"] =
            "Personnage modifié avec succès";

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
            new SelectListItem
            {
                Value = "Vivant",
                Text = "Vivant"
            },
            new SelectListItem
            {
                Value = "Décédé",
                Text = "Décédé"
            },
            new SelectListItem
            {
                Value = "Inconnu",
                Text = "Inconnu"
            }
        ];
    }
}