CREATE OR ALTER PROCEDURE get_ingredients
AS
    BEGIN
        SET NOCOUNT ON
        
        SELECT id, nom FROM Ingredient;
    END
GO

CREATE OR ALTER PROCEDURE get_ingredient_by_id(
    @id INT
)
AS
    BEGIN
        SET NOCOUNT ON
    
        SELECT nom, description FROM Ingredient
        WHERE id = @id;
    END
GO

CREATE OR ALTER PROCEDURE get_ingredients_by_name(
    @nom VARCHAR(64)
)
AS
    BEGIN
        SET NOCOUNT ON
    
        SELECT id FROM Ingredient
        WHERE nom LIKE '%'+@nom+'%';
    END
GO

CREATE OR ALTER PROCEDURE add_ingredient(
    @nom VARCHAR(64),
    @description VARCHAR(1024)
)
AS
    BEGIN 
        SET NOCOUNT ON
        
        INSERT INTO Ingredient (nom, description) VALUES (@nom, @description);
    END
GO

CREATE OR ALTER PROCEDURE delete_ingredient(
    @id INT
)
AS
    BEGIN
        SET NOCOUNT ON
    
        DELETE FROM Ingredient WHERE id = @id;
    END
GO

CREATE OR ALTER PROCEDURE update_ingredient(
    @id INT,
    @nom VARCHAR(64),
    @description VARCHAR(1024)
)
AS
    BEGIN
        SET NOCOUNT ON

        UPDATE Ingredient
        SET nom = @nom, description = @description
        WHERE id = @id;
    END
GO