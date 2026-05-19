using System.ComponentModel.DataAnnotations;
using AnimeCollection.Helpers;
using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeCollection.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserService _userService;

    public RegisterModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string Name { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
    public string Password { get; set; } = "";

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (_userService.GetByEmail(Email) is not null)
        {
            ModelState.AddModelError(
                "Email",
                "Cet email est déjà utilisé");

            return Page();
        }

        var user = new User
        {
            Name = Name,
            Email = Email,
            PasswordHash = PasswordHelper.HashPassword(Password),
            Role = "User"
        };

        _userService.Create(user);

        TempData["Message"] = "Compte créé avec succès";

        return RedirectToPage("Index");
    }
}