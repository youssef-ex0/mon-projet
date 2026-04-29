===============================================================================
            PRESENTATION DU PROJET MINICOMMERCE
            Gestion des Produits - Mini e-commerce avec ADO.NET
===============================================================================

-------------------------------------------------------------------------------
SOMMAIRE
-------------------------------------------------------------------------------

  1. Introduction et contexte
  2. Objectifs du projet
  3. Architecture technique
  4. Structure du projet
  5. Base de donnees
  6. Modele de donnees
  7. Couche d'acces aux donnees (ADO.NET)
  8. Les Controleurs
  9. Les Vues
 10. Page d'accueil avec categories et photos
 11. Demonstration en direct
 12. Problemes rencontres et solutions
 13. Ameliorations futures
 14. Conclusion


===============================================================================
1. INTRODUCTION ET CONTEXTE
===============================================================================

Bonjour a tous,

Aujourd'hui, je vous presente mon projet "MiniCommerce".

C'est une application web de gestion de produits, developpee avec ASP.NET MVC
et ADO.NET. Le principe est simple : c'est une boutique en ligne miniature
qui permet de gerer un catalogue de produits.

Ce projet a ete realise dans le cadre de l'apprentissage de :
  - ASP.NET MVC (architecture Model-Vue-Controleur)
  - ADO.NET (acces direct a la base de donnees SQL Server)
  - Le developpement web avec C# et .NET 10


===============================================================================
2. OBJECTIFS DU PROJET
===============================================================================

L'application permet de realiser les operations CRUD completes :

  [C] CREATE  -> Ajouter un nouveau produit
  [R] READ    -> Afficher la liste des produits
  [U] UPDATE  -> Modifier un produit existant
  [D] DELETE  -> Supprimer un produit

Plus des fonctionnalites supplementaires :
  - Recherche de produits par nom ou par categorie
  - Filtrage par categorie (Informatique, Peripheriques, Audio, Ecrans)
  - Page d'accueil avec photos et categories
  - Interface responsive avec Bootstrap 5


===============================================================================
3. ARCHITECTURE TECHNIQUE
===============================================================================

  +-------------------+     +-------------------+     +-------------------+
  |     NAVIGATEUR    | --> |   ASP.NET MVC     | --> |   SQL Server      |
  |   (HTML/CSS/JS)   | <-- |   (.NET 10)       | <-- |   (LocalDB)       |
  +-------------------+     +-------------------+     +-------------------+

  Backend      : ASP.NET MVC avec .NET 10
  Base de      : SQL Server Express LocalDB
  donnees
  Acces data   : ADO.NET (SqlConnection, SqlCommand, SqlDataReader)
  Frontend     : HTML, CSS, Bootstrap 5.3, Bootstrap Icons
  Langage      : C# 13
  IDE          : Visual Studio Code + Claude Code (IA)


===============================================================================
4. STRUCTURE DU PROJET
===============================================================================

  MiniCommerce/
  |
  |-- Controllers/
  |   |-- HomeController.cs         Page d'accueil
  |   +-- ProduitController.cs      CRUD produits
  |
  |-- Models/
  |   +-- Produit.cs                Modele Produit (Id, Nom, Prix, Quantite,
  |                                  Categorie, ImageUrl)
  |
  |-- Data/
  |   +-- ProduitRepository.cs      Acces base de donnees (ADO.NET)
  |
  |-- Views/
  |   |-- Home/
  |   |   +-- Index.cshtml          Page d'accueil (hero + categories + produits)
  |   |-- Produit/
  |   |   |-- Index.cshtml          Liste des produits avec filtres
  |   |   |-- Create.cshtml         Formulaire d'ajout
  |   |   |-- Edit.cshtml           Formulaire de modification
  |   |   +-- Delete.cshtml         Confirmation de suppression
  |   +-- Shared/
  |       +-- _Layout.cshtml        Layout commun (navbar, footer)
  |
  |-- wwwroot/
  |   +-- css/site.css              Styles personnalises
  |
  |-- database.sql                  Script de creation de la base
  |-- Program.cs                    Configuration de l'application
  +-- appsettings.json              Chaine de connexion


===============================================================================
5. BASE DE DONNEES
===============================================================================

La base s'appelle "MiniCommerceDB" et contient une seule table : PRODUITS.

  +------------+------------------+----------------------------+
  | Champ      | Type             | Description                |
  +------------+------------------+----------------------------+
  | Id         | INT (PK)         | Identifiant auto-increment |
  | Nom        | NVARCHAR(100)    | Nom du produit             |
  | Prix       | DECIMAL(10,2)    | Prix unitaire              |
  | Quantite   | INT              | Stock disponible           |
  | Categorie  | NVARCHAR(50)     | Categorie du produit       |
  | ImageUrl   | NVARCHAR(500)    | URL de la photo (optionnel)|
  +------------+------------------+----------------------------+

Donnees de test : 5 produits repartis en 4 categories

  - Ordinateur Portable  | Informatique   | 1 299,99 EUR | Stock : 15
  - Souris sans fil      | Peripheriques  |    29,99 EUR | Stock : 50
  - Clavier mecanique    | Peripheriques  |    89,99 EUR | Stock : 30
  - Moniteur 27 pouces   | Ecrans         |   349,99 EUR | Stock : 10
  - Casque audio         | Audio          |    59,99 EUR | Stock : 25


===============================================================================
6. MODELE DE DONNEES (Models/Produit.cs)
===============================================================================

Le modele Produit comporte 6 proprietes avec validation :

  public class Produit
  {
      public int Id { get; set; }

      [Required]                          // Le nom est obligatoire
      [StringLength(100)]
      public string Nom { get; set; }

      [Required]                          // Le prix est obligatoire
      [Range(0.01, double.MaxValue)]      // Le prix doit etre > 0
      public decimal Prix { get; set; }

      [Required]                          // La quantite est obligatoire
      [Range(0, int.MaxValue)]            // La quantite doit etre positive
      public int Quantite { get; set; }

      [Required]                          // La categorie est obligatoire
      [StringLength(50)]
      public string Categorie { get; set; }

      [Url]                               // L'URL doit etre valide
      [StringLength(500)]
      public string? ImageUrl { get; set; }
  }

=> Les annotations [Required], [Range], [Url] assurent la validation
   automatique cote serveur.


===============================================================================
7. COUCHE D'ACCES AUX DONNEES (Data/ProduitRepository.cs)
===============================================================================

C'est le coeur du projet. On utilise ADO.NET pur, sans Entity Framework.

  Principe :
  1. Ouvrir une SqlConnection avec la chaine de connexion
  2. Creer un SqlCommand avec la requete SQL
  3. Ajouter les parametres (protection contre les injections SQL)
  4. Executer la commande (ExecuteReader ou ExecuteNonQuery)
  5. Lire les resultats avec SqlDataReader

  Methodes implementees :

  +---------------------+----------------------------------------------+
  | Methode             | Fonction                                     |
  +---------------------+----------------------------------------------+
  | GetAll()            | Recuperer tous les produits                  |
  | GetById(id)         | Recuperer un produit par son ID              |
  | Add(produit)        | Inserer un nouveau produit                   |
  | Update(produit)     | Modifier un produit existant                 |
  | Delete(id)          | Supprimer un produit                         |
  | Search(term)        | Rechercher par nom OU categorie              |
  | GetByCategory(cat)  | Filtrer par categorie                        |
  | GetCategories()     | Lister les categories distinctes             |
  +---------------------+----------------------------------------------+

  Exemple de code (methode Add) :

      public void Add(Produit produit)
      {
          using var connection = new SqlConnection(_connectionString);
          using var command = new SqlCommand(
              "INSERT INTO Produits (Nom, Prix, Quantite, Categorie, ImageUrl)
               VALUES (@Nom, @Prix, @Quantite, @Categorie, @ImageUrl)", connection);

          command.Parameters.AddWithValue("@Nom", produit.Nom);
          command.Parameters.AddWithValue("@Prix", produit.Prix);
          command.Parameters.AddWithValue("@Quantite", produit.Quantite);
          command.Parameters.AddWithValue("@Categorie", produit.Categorie);
          command.Parameters.AddWithValue("@ImageUrl", produit.ImageUrl ?? DBNull.Value);

          connection.Open();
          command.ExecuteNonQuery();
      }

  Points importants :
  - "using" assure la fermeture automatique de la connexion
  - Les parametres @Nom, @Prix... protegent contre les INJECTIONS SQL
  - DBNull.Value gere les champs null (ImageUrl optionnel)


===============================================================================
8. LES CONTROLEURS
===============================================================================

A. HomeController (page d'accueil)

  - Index() : Charge les categories et les 6 derniers produits,
              les passe a la vue via ViewBag

B. ProduitController (CRUD)

  +------------------+------+------------------------------------------+
  | Action           | HTTP | Comportement                             |
  +------------------+------+------------------------------------------+
  | Index(search,    | GET  | Liste avec recherche + filtre categorie  |
  |   categorie)     |      |                                          |
  | Create()         | GET  | Affiche le formulaire vide               |
  | Create(produit)  | POST | Valide, enregistre, redirige vers Index  |
  | Edit(id)         | GET  | Charge le produit dans le formulaire     |
  | Edit(produit)    | POST | Valide, met a jour, redirige             |
  | Delete(id)       | GET  | Affiche la confirmation                  |
  | DeleteConfirmed() | POST | Supprime et redirige                    |
  +------------------+------+------------------------------------------+

  Securite :
  - [ValidateAntiForgeryToken] sur tous les POST
  - Anti-forgery token dans chaque formulaire
  - Protection contre les attaques CSRF


===============================================================================
9. LES VUES
===============================================================================

Toutes les vues utilisent Bootstrap 5 pour le design responsive.

  A. _Layout.cshtml (template commun)
     - Barre de navigation : Accueil | Produits | Ajouter
     - Bootstrap Icons pour les icones
     - Footer

  B. Home/Index.cshtml (page d'accueil)
     - Section HERO avec degrade (titre + bouton)
     - Grille de cartes CATEGORIES avec icones et couleurs
     - Grille de cartes PRODUITS avec photos, prix, badge stock
     - Bouton "Voir le catalogue complet"

  C. Produit/Index.cshtml (liste des produits)
     - Barre de recherche + filtre par categorie (dropdown)
     - Tableau avec : photo miniature, nom, categorie, prix, stock
     - Badge rouge si stock <= 5
     - Boutons Modifier / Supprimer

  D. Produit/Create.cshtml et Edit.cshtml
     - Formulaire avec : Nom, Categorie (avec autocomplete),
       Prix, Quantite, URL de l'image
     - Validation en temps reel (messages d'erreur en francais)

  E. Produit/Delete.cshtml
     - Photo du produit + details
     - Confirmation avant suppression


===============================================================================
10. PAGE D'ACCUEIL - DETAIL
===============================================================================

La page d'accueil est la vitrine de l'application. Elle contient :

  1. BANNIERE HERO
     - Fond degrade bleu fonce
     - Titre : "Bienvenue sur MiniCommerce"
     - Bouton "Voir tous les produits"

  2. CARTES CATEGORIES (4 categories)
     - Informatique   (icone ordinateur, bleu)
     - Peripheriques  (icone souris, vert)
     - Audio          (icone casque, rouge)
     - Ecrans         (icone ecran, orange)
     - Chaque carte est cliquable et filtre les produits

  3. GRILLE PRODUITS RECENTS (6 max)
     - Carte avec photo, nom, prix, badge stock
     - Images depuis picsum.photos (placeholders)
     - Hover animation (zoom + ombre)

  4. CSS PERSONNALISE
     - Animations au survol (translateY + box-shadow)
     - Degrade hero-gradient
     - Transitions fluides


===============================================================================
11. DEMONSTRATION EN DIRECT
===============================================================================

  [Lancer l'application : dotnet run --no-build]
  [Ouvrir le navigateur : http://localhost:5249]

  ETAPE 1 : Page d'accueil
  - Montrer le hero banner
  - Montrer les cartes de categories
  - Montrer les produits avec photos

  ETAPE 2 : Navigation par categorie
  - Cliquer sur "Audio" -> affiche uniquement le casque audio
  - Cliquer sur "Peripheriques" -> affiche souris + clavier

  ETAPE 3 : Ajouter un produit
  - Cliquer sur "Ajouter"
  - Remplir le formulaire : Nom, Categorie, Prix, Quantite, URL image
  - Valider -> le produit apparait dans la liste

  ETAPE 4 : Modifier un produit
  - Cliquer sur "Modifier" sur un produit
  - Changer le prix ou la quantite
  - Sauvegarder -> les changements sont pris en compte

  ETAPE 5 : Rechercher
  - Taper "souris" dans la barre de recherche
  - Resultat : uniquement les produits correspondants

  ETAPE 6 : Supprimer un produit
  - Cliquer sur "Supprimer"
  - Confirmation affichee avec photo et details
  - Confirmer -> produit supprime


===============================================================================
12. PROBLEMES RENCONTRES ET SOLUTIONS
===============================================================================

  PROBLEME 1 : SQL Server non installe
  - Solution : Utilisation de SQL Server LocalDB (integre a Visual Studio)
  - Avantage : pas besoin d'installer SQL Server complet

  PROBLEME 2 : Erreur de type FLOAT vs DECIMAL
  - SQL Server FLOAT retourne un "double" en C#
  - Mais le modele utilise "decimal" pour la precision monetaire
  - Solution : Change la colonne en DECIMAL(10,2) dans la base

  PROBLEME 3 : Culture francaise vs separateur decimal
  - Les inputs HTML envoient "45.99" (avec point)
  - Mais le systeme francais attend "45,99" (avec virgule)
  - Solution : Configurer InvariantCulture dans Program.cs

  PROBLEME 4 : Injection de dependance
  - Le HomeController n'avait pas acces au repository
  - Solution : Injection via le constructeur (AddScoped dans Program.cs)


===============================================================================
13. AMELIORATIONS FUTURES
===============================================================================

  Fonctionnalites qu'on pourrait ajouter :

  [ ] Upload d'images locales (au lieu des URLs)
  [ ] Systeme de panier d'achat
  [ ] Authentification (login / mot de passe)
  [ ] Table Categories separee (au lieu d'une simple colonne)
  [ ] Pagination de la liste des produits
  [ ] Dashboard admin avec statistiques
  [ ] API REST pour une application mobile
  [ ] Deploiement sur Azure ou un hebergeur web


===============================================================================
14. CONCLUSION
===============================================================================

Ce projet m'a permis de mettre en pratique :

  - L'architecture MVC : separation claire entre Modele, Vue, Controleur
  - ADO.NET : connexion directe a SQL Server avec SqlCommand et SqlDataReader
  - La validation des donnees : annotations [Required], [Range], [Url]
  - Le design web : Bootstrap 5, responsive, animations CSS
  - La securite : protection CSRF, parametres SQL pour eviter les injections
  - L'injection de dependances : pattern moderne dans .NET

L'application est fonctionnelle et couvre toutes les operations CRUD
avec une interface utilisateur moderne et agréable.

Merci pour votre attention !

Questions ?
