using System.Data;
using Microsoft.Data.SqlClient;
using MiniCommerce.Models;

namespace MiniCommerce.Data;

public class ProduitRepository
{
    private readonly string _connectionString;

    public ProduitRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private static Produit MapProduit(SqlDataReader reader) => new()
    {
        Id = reader.GetInt32("Id"),
        Nom = reader.GetString("Nom"),
        Prix = reader.GetDecimal("Prix"),
        Quantite = reader.GetInt32("Quantite"),
        Categorie = reader.GetString("Categorie"),
        ImageUrl = reader.IsDBNull("ImageUrl") ? null : reader.GetString("ImageUrl")
    };

    public IEnumerable<Produit> GetAll()
    {
        var produits = new List<Produit>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT Id, Nom, Prix, Quantite, Categorie, ImageUrl FROM Produits ORDER BY Id DESC", connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            produits.Add(MapProduit(reader));

        return produits;
    }

    public Produit? GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT Id, Nom, Prix, Quantite, Categorie, ImageUrl FROM Produits WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapProduit(reader) : null;
    }

    public void Add(Produit produit)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "INSERT INTO Produits (Nom, Prix, Quantite, Categorie, ImageUrl) VALUES (@Nom, @Prix, @Quantite, @Categorie, @ImageUrl)", connection);

        command.Parameters.AddWithValue("@Nom", produit.Nom);
        command.Parameters.AddWithValue("@Prix", produit.Prix);
        command.Parameters.AddWithValue("@Quantite", produit.Quantite);
        command.Parameters.AddWithValue("@Categorie", produit.Categorie);
        command.Parameters.AddWithValue("@ImageUrl", (object?)produit.ImageUrl ?? DBNull.Value);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Update(Produit produit)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "UPDATE Produits SET Nom = @Nom, Prix = @Prix, Quantite = @Quantite, Categorie = @Categorie, ImageUrl = @ImageUrl WHERE Id = @Id", connection);

        command.Parameters.AddWithValue("@Id", produit.Id);
        command.Parameters.AddWithValue("@Nom", produit.Nom);
        command.Parameters.AddWithValue("@Prix", produit.Prix);
        command.Parameters.AddWithValue("@Quantite", produit.Quantite);
        command.Parameters.AddWithValue("@Categorie", produit.Categorie);
        command.Parameters.AddWithValue("@ImageUrl", (object?)produit.ImageUrl ?? DBNull.Value);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("DELETE FROM Produits WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public IEnumerable<Produit> Search(string searchTerm)
    {
        var produits = new List<Produit>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT Id, Nom, Prix, Quantite, Categorie, ImageUrl FROM Produits WHERE Nom LIKE @SearchTerm OR Categorie LIKE @SearchTerm ORDER BY Id DESC", connection);
        command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            produits.Add(MapProduit(reader));

        return produits;
    }

    public IEnumerable<Produit> GetByCategory(string category)
    {
        var produits = new List<Produit>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT Id, Nom, Prix, Quantite, Categorie, ImageUrl FROM Produits WHERE Categorie = @Categorie ORDER BY Id DESC", connection);
        command.Parameters.AddWithValue("@Categorie", category);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            produits.Add(MapProduit(reader));

        return produits;
    }

    public IEnumerable<string> GetCategories()
    {
        var categories = new List<string>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT DISTINCT Categorie FROM Produits ORDER BY Categorie", connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            categories.Add(reader.GetString("Categorie"));

        return categories;
    }
}
