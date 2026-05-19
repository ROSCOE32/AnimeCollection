using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeCollection.Pages.Personnages;

public class DetailsModel : PageModel
{
    private readonly PersonnageService _personnageService;

    public DetailsModel(PersonnageService personnageService)
    {
        _personnageService = personnageService;
    }

    public Personnage Personnage { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        var personnage = _personnageService.GetById(id);

        if (personnage is null)
        {
            return NotFound();
        }

        Personnage = personnage;

        return Page();
    }
}