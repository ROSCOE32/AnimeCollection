using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeCollection.Pages.Personnages;

public class DeleteModel : PageModel
{
    private readonly PersonnageService _personnageService;

    public DeleteModel(
        PersonnageService personnageService)
    {
        _personnageService = personnageService;
    }

    [BindProperty]
    public Personnage Personnage { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        var personnage =
            _personnageService.GetById(id);

        if (personnage is null)
        {
            return NotFound();
        }

        Personnage = personnage;

        return Page();
    }

    public IActionResult OnPost()
    {
        var deleted =
            _personnageService.Delete(Personnage.Id);

        if (!deleted)
        {
            return NotFound();
        }

        TempData["Message"] =
            "Personnage supprimé avec succès";

        return RedirectToPage("Index");
    }
}