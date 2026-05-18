using System.ComponentModel.DataAnnotations;

namespace AnimeCollection.Models;

public class Personnage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "La description est obligatoire")]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Le statut est obligatoire")]
    public string Status { get; set; } = "";

    public string? ImagePath { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vous devez choisir un animé")]
    public int AnimeId { get; set; }

    public string AnimeTitle { get; set; } = "";
}