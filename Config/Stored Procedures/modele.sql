CREATE OR ALTER PROCEDURE get_modeles
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, nom FROM Modele;
END
GO

CREATE OR ALTER PROCEDURE get_modele_by_id(
    @id INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT nom, description, pUHT, gamme FROM Modele                                     
    WHERE id = @id;
END
GO

CREATE OR ALTER PROCEDURE get_modeles_by_name(
    @nom VARCHAR(64)
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, nom FROM Modele
    WHERE nom LIKE '%'+@nom+'%';
END
GO

CREATE OR ALTER PROCEDURE get_modeles_by_gamme(
    @gamme VARCHAR(64)
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, nom, gamme FROM Modele
    WHERE gamme LIKE '%'+@gamme+'%';
END
GO

CREATE OR ALTER PROCEDURE get_modele_ingredients(
    @id_modele int
)
AS
    BEGIN 
        SET NOCOUNT ON
        
        SELECT Ingredient.id, Ingredient.nom, grammage FROM Modele_Ingredient
        JOIN Ingredient ON Ingredient.id = id_ingredient
        WHERE id_modele = @id_modele;
    END
GO

CREATE OR ALTER PROCEDURE get_modele_caracteristiques(
    @id_modele int
)
AS
    BEGIN 
        SET NOCOUNT ON
        
        SELECT Caracteristique.id, Caracteristique.nom FROM Modele_Caracteristique
        JOIN Caracteristique ON Caracteristique.id = id_caracteristique
        WHERE id_modele = @id_modele;
    END
GO

CREATE OR ALTER PROCEDURE get_modele_procedeFabrications(
    @id_modele int
)
AS
    BEGIN 
        SET NOCOUNT ON
        
        SELECT ProcedeFabrication.id, ProcedeFabrication.nom FROM ProcedeFabrication
        JOIN Modele ON Modele.id = id_modele
        WHERE id_modele = @id_modele;
    END
GO

CREATE OR ALTER PROCEDURE add_modele(
    @nom VARCHAR(64),
    @description VARCHAR(1024),
    @pUHT INT,
    @gamme VARCHAR(64)
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO Modele (nom, description, pUHT, gamme) VALUES (@nom, @description, @pUHT, @gamme);
END
GO

CREATE OR ALTER PROCEDURE add_ingredient_to_modele(
    @id_modele INT,
    @id_ingredient INT,
    @grammage INT
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO Modele_Ingredient (id_modele, id_ingredient, grammage) VALUES (@id_modele, @id_ingredient, @grammage);
END
GO

CREATE OR ALTER PROCEDURE add_Caracteristique_to_modele(
    @id_modele INT,
    @id_caracteristique INT
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO Modele_Caracteristique (id_modele, id_caracteristique) VALUES (@id_modele, @id_caracteristique);
END
GO

CREATE OR ALTER PROCEDURE delete_modele(
    @id INT
)
AS
BEGIN
    SET NOCOUNT ON
    DELETE FROM Modele_Ingredient WHERE id_modele = @id;
    DELETE FROM Modele_Caracteristique WHERE id_modele = @id;
    DELETE FROM ProcedeFabrication WHERE id_modele = @id;
    DELETE FROM Modele WHERE id = @id;
END
GO

CREATE OR ALTER PROCEDURE delete_ingredient_from_modele(
    @id_modele INT,
    @id_ingredient INT
)
AS
    BEGIN
        SET NOCOUNT ON

        DELETE FROM Modele_Ingredient WHERE id_modele = @id_modele AND id_ingredient = @id_ingredient;
    END
GO

CREATE OR ALTER PROCEDURE delete_Caracteristique_from_modele(
    @id_modele INT,
    @id_caracteristique INT
)
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM Modele_Caracteristique WHERE id_modele = @id_modele AND id_caracteristique = @id_caracteristique;
END
GO

CREATE OR ALTER PROCEDURE update_modele(
    @id INT,
    @nom VARCHAR(64),
    @description VARCHAR(1024),
    @pUHT INT,
    @gamme VARCHAR(64)
)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE Modele
    SET nom = @nom, description = @description, pUHT = @pUHT, gamme = @gamme
    WHERE id = @id;
END
GO

CREATE OR ALTER PROCEDURE get_modele_test(
    @id_modele int
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT Test.id, Test.description, type FROM Test
    JOIN ProcedeFabrication_Test ON ProcedeFabrication_Test.id_test = Test.id
    JOIN ProcedeFabrication ON ProcedeFabrication.id_modele = ProcedeFabrication_Test.id_procedeFabrication
    WHERE ProcedeFabrication.id_modele = @id_modele;
END
GO