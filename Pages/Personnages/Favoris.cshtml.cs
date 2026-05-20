using System.Security.Claims;
using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeCollection.Pages.Personnages;

[Authorize]
public class FavorisModel : PageModel
{
    private readonly FavoriService _favoriService;

    public FavorisModel(FavoriService favoriService)
    {
        _favoriService = favoriService;
    }

    public List<Personnage> Personnages { get; set; } = new();

    public IActionResult OnGet()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
        {
            return RedirectToPage("/Account/Login");
        }

        Personnages = _favoriService.GetFavorites(userId);

        return Page();
    }
}