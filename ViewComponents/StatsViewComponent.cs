using AnimeCollection.Models;
using AnimeCollection.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnimeCollection.ViewComponents;

public class StatsViewComponent : ViewComponent
{
    private readonly AnimeService _animeService;
    private readonly PersonnageService _personnageService;

    public StatsViewComponent(
        AnimeService animeService,
        PersonnageService personnageService)
    {
        _animeService = animeService;
        _personnageService = personnageService;
    }

    public IViewComponentResult Invoke()
    {
        var stats = new StatsViewModel
        {
            AnimeCount = _animeService.CountAll(),
            PersonnageCount = _personnageService.CountAll()
        };

        return View(stats);
    }
}