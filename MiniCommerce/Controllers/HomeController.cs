using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Data;
using MiniCommerce.Models;

namespace MiniCommerce.Controllers;

public class HomeController : Controller
{
    private readonly ProduitRepository _repository;

    public HomeController(ProduitRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var produits = _repository.GetAll().ToList();
        var categories = _repository.GetCategories().ToList();

        ViewBag.Categories = categories;
        ViewBag.ProduitsRecents = produits.Take(6);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
