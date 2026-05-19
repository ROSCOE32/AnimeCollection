using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AnimeCollection.Helpers;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeCollection.Pages.Account;

public class LoginModel : PageModel
{
    private readonly UserService _userService;

    public LoginModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required(ErrorMessage = "L'email est obligatoire")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = "";

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = _userService.GetByEmail(Email);

        if (user is null)
        {
            ModelState.AddModelError(
                "",
                "Email ou mot de passe invalide");

            return Page();
        }

        var validPassword =
            PasswordHelper.VerifyPassword(
                Password,
                user.PasswordHash);

        if (!validPassword)
        {
            ModelState.AddModelError(
                "",
                "Email ou mot de passe invalide");

            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            "CookieAuth",
            principal);

        return RedirectToPage("/Index");
    }
}