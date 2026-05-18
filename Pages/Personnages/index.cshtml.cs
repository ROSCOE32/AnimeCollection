using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeCollection.Pages.Personnages;

public class IndexModel : PageModel
{
    private readonly PersonnageService _personnageService;

    public IndexModel(PersonnageService personnageService)
    {
        _personnageService = personnageService;
    }

    public List<Personnage> Personnages { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public void OnGet()
    {
        Personnages = _personnageService.GetAll(Search);
    }
}