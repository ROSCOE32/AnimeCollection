using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using AnimeCollection.Services;


namespace AnimeCollection.Pages.Personnages;

public class DetailsModel : PageModel
{
    private readonly PersonnageService _personnageService;
	private readonly FavoriService _favoriService;
	public DetailsModel(PersonnageService personnageService, FavoriService favoriService)
	{
    	_personnageService = personnageService;
    	_favoriService = favoriService;
	}
	

	public bool IsFavorite { get; set; }

	public int? UserId { get; set; }

    public Personnage Personnage { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        var personnage = _personnageService.GetById(id);

        if (personnage is null)
        {
            return NotFound();
        }

        Personnage = personnage;
		var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (int.TryParse(userIdValue, out var userId))
		{
    		UserId = userId;
    		IsFavorite = _favoriService.IsFavorite(userId, id);
		}

        return Page();
    }

	public IActionResult OnPostToggleFavorite(int id)
	{
   		 var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

   		if (!int.TryParse(userIdValue, out var userId))
    	{
     	   return RedirectToPage("/Account/Login");
    	}

    	if (_favoriService.IsFavorite(userId, id))
   		{
      	  _favoriService.Remove(userId, id);
    	}
    	else
    	{
       		 _favoriService.Add(userId, id);
    	}

   		 return RedirectToPage("Details", new { id });
	}
}