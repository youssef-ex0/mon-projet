using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Data;
using MiniCommerce.Models;

namespace MiniCommerce.Controllers;

public class ProduitController : Controller
{
    private readonly ProduitRepository _repository;

    public ProduitController(ProduitRepository repository)
    {
        _repository = repository;
    }

    // GET: /Produit or /Produit?searchTerm=xxx&categorie=yyy
    public IActionResult Index(string? searchTerm, string? categorie)
    {
        IEnumerable<Produit> produits;

        if (!string.IsNullOrWhiteSpace(searchTerm))
            produits = _repository.Search(searchTerm);
        else if (!string.IsNullOrWhiteSpace(categorie))
            produits = _repository.GetByCategory(categorie);
        else
            produits = _repository.GetAll();

        ViewBag.Categories = _repository.GetCategories().ToList();
        ViewBag.SearchTerm = searchTerm;
        ViewBag.CategorieSelectionnee = categorie;

        return View(produits);
    }

    // GET: /Produit/Create
    public IActionResult Create()
    {
        ViewBag.Categories = _repository.GetCategories().ToList();
        return View();
    }

    // POST: /Produit/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Produit produit)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _repository.GetCategories().ToList();
            return View(produit);
        }

        _repository.Add(produit);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Produit/Edit/5
    public IActionResult Edit(int id)
    {
        var produit = _repository.GetById(id);
        if (produit == null)
            return NotFound();

        ViewBag.Categories = _repository.GetCategories().ToList();
        return View(produit);
    }

    // POST: /Produit/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Produit produit)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _repository.GetCategories().ToList();
            return View(produit);
        }

        _repository.Update(produit);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Produit/Delete/5
    public IActionResult Delete(int id)
    {
        var produit = _repository.GetById(id);
        if (produit == null)
            return NotFound();

        return View(produit);
    }

    // POST: /Produit/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _repository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
