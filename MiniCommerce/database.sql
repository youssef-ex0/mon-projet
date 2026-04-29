-- ============================================
-- MiniCommerce - Database Setup Script
-- Run this script in SQL Server Management Studio
-- ============================================

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MiniCommerceDB')
BEGIN
    CREATE DATABASE MiniCommerceDB;
END
GO

USE MiniCommerceDB;
GO

-- Create Produits table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Produits')
BEGIN
    CREATE TABLE Produits (
        Id         INT IDENTITY(1,1) PRIMARY KEY,
        Nom        NVARCHAR(100)   NOT NULL,
        Prix       DECIMAL(10,2)   NOT NULL,
        Quantite   INT             NOT NULL,
        Categorie  NVARCHAR(50)    NOT NULL DEFAULT 'Non classee',
        ImageUrl   NVARCHAR(500)   NULL
    );
END
GO

-- Migration: add columns if table already exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Produits' AND COLUMN_NAME='Categorie')
    ALTER TABLE Produits ADD Categorie NVARCHAR(50) NOT NULL DEFAULT 'Non classee';
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Produits' AND COLUMN_NAME='ImageUrl')
    ALTER TABLE Produits ADD ImageUrl NVARCHAR(500) NULL;
GO

-- Insert sample data
IF NOT EXISTS (SELECT * FROM Produits)
BEGIN
    INSERT INTO Produits (Nom, Prix, Quantite, Categorie, ImageUrl) VALUES
        ('Ordinateur Portable', 1299.99, 15, 'Informatique',  'https://picsum.photos/seed/laptop/400/300'),
        ('Souris sans fil',      29.99,  50, 'Peripheriques', 'https://picsum.photos/seed/mouse/400/300'),
        ('Clavier mecanique',    89.99,  30, 'Peripheriques', 'https://picsum.photos/seed/keyboard/400/300'),
        ('Moniteur 27 pouces',  349.99,  10, 'Ecrans',        'https://picsum.photos/seed/monitor/400/300'),
        ('Casque audio',        59.99,  25, 'Audio',          'https://picsum.photos/seed/headset/400/300');
END
GO

PRINT 'Database MiniCommerceDB created successfully.';
GO
