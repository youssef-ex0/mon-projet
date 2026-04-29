using System.ComponentModel.DataAnnotations;

namespace MiniCommerce.Models;

public class Produit
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prix est obligatoire")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit etre superieur a 0")]
    public decimal Prix { get; set; }

    [Required(ErrorMessage = "La quantite est obligatoire")]
    [Range(0, int.MaxValue, ErrorMessage = "La quantite doit etre positive")]
    public int Quantite { get; set; }

    [Required(ErrorMessage = "La categorie est obligatoire")]
    [StringLength(50)]
    public string Categorie { get; set; } = string.Empty;

    [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }
}
