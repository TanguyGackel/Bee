IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = 'BeeDB')
BEGIN
    CREATE DATABASE BeeDB
END;

USE BeeDB;

DROP TABLE IF EXISTS ProcedeFabrication_Test;
DROP TABLE IF EXISTS ProcedeFabrication_Etape;
DROP TABLE IF EXISTS Test;
DROP TABLE IF EXISTS Etape;
DROP TABLE IF EXISTS ProcedeFabrication;
DROP TABLE IF EXISTS Modele_Ingredient;
DROP TABLE IF EXISTS Modele_Caracteristique;
DROP TABLE IF EXISTS Caracteristique;
DROP TABLE IF EXISTS Ingredient;
DROP TABLE IF EXISTS Modele;

CREATE TABLE Modele (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    nom VARCHAR(64) NOT NULL UNIQUE,
    description VARCHAR(1024) NOT NULL DEFAULT 'No description',
    pUHT INT NOT NULL CHECK(pUHT > 0),
    gamme VARCHAR(64) NOT NULL
);

CREATE TABLE Ingredient (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    nom VARCHAR(64) NOT NULL UNIQUE,
    description VARCHAR(1024) NOT NULL DEFAULT 'No description'
);

CREATE TABLE Caracteristique(
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    nom VARCHAR(64) NOT NULL UNIQUE,
    description VARCHAR(1024) NOT NULL DEFAULT 'No description'
)

CREATE TABLE Modele_Ingredient (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    id_modele INT NOT NULL,
    id_ingredient INT NOT NULL,
    grammage INT NOT NULL CHECK(grammage > 0),
    FOREIGN KEY (id_modele) REFERENCES Modele(id),
    FOREIGN KEY (id_ingredient) REFERENCES Ingredient(id)
);

CREATE TABLE Modele_Caracteristique (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    id_modele INT NOT NULL,
    id_caracteristique INT NOT NULL,
    FOREIGN KEY (id_modele) REFERENCES Modele(id),
    FOREIGN KEY (id_caracteristique) REFERENCES Caracteristique(id)
);

CREATE TABLE ProcedeFabrication (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    nom VARCHAR(64) NOT NULL UNIQUE,
    description VARCHAR(1024) NOT NULL DEFAULT 'No description',
    id_modele INT NOT NULL,
    FOREIGN KEY (id_modele) REFERENCES Modele(id)
);

CREATE TABLE Etape (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    nom VARCHAR(64) NOT NULL,
    description VARCHAR(1024) NOT NULL DEFAULT 'No description'
);

CREATE TABLE Test(
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    nom VARCHAR(64) NOT NULL,
    description VARCHAR(1024) NOT NULL DEFAULT 'No description',
    type VARCHAR(16) NOT NULL CHECK (type IN('None', 'Fonctionnel', 'Industriel')) DEFAULT 'None',
    valide BIT NOT NULL DEFAULT 0
);

CREATE TABLE ProcedeFabrication_Etape (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    id_procedeFabrication INT NOT NULL,
    id_etape INT NOT NULL,
    FOREIGN KEY (id_procedeFabrication) REFERENCES ProcedeFabrication(id),
    FOREIGN KEY (id_etape) REFERENCES Etape(id)
);

CREATE TABLE ProcedeFabrication_Test(
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    id_procedeFabrication INT NOT NULL,
    id_test INT NOT NULL,
    FOREIGN KEY (id_procedeFabrication) REFERENCES ProcedeFabrication(id),
    FOREIGN KEY (id_test) REFERENCES Test(id)
);