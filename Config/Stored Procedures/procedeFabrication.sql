CREATE OR ALTER PROCEDURE get_procedeFabrications
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, nom FROM ProcedeFabrication;
END

GO
CREATE OR ALTER PROCEDURE get_procedeFabrication_by_id(
    @id_procedeFabrication INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT ProcedeFabrication.nom, ProcedeFabrication.description, ProcedeFabrication.id_modele, Modele.nom FROM ProcedeFabrication
    JOIN Modele ON Modele.id = ProcedeFabrication.id_modele
    WHERE ProcedeFabrication.id = @id_procedeFabrication;
END
GO

CREATE OR ALTER PROCEDURE get_procedeFabrication_by_name(
    @nom VARCHAR(64)
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, nom FROM ProcedeFabrication
    WHERE nom LIKE '%'+@nom+'%';
END
GO

CREATE OR ALTER PROCEDURE get_procedeFabrication_modele(
    @id_modele INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT id, nom FROM ProcedeFabrication
    WHERE id_modele = @id_modele;
END
GO

CREATE OR ALTER PROCEDURE get_procedeFabrication_tests(
    @id_procedeFabrication INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT Test.id, Test.nom, Test.type, Test.valide FROM ProcedeFabrication_Test
    JOIN Test ON Test.id = ProcedeFabrication_Test.id_test
    WHERE ProcedeFabrication_Test.id_procedeFabrication = @id_procedeFabrication;
END
GO

CREATE OR ALTER PROCEDURE get_procedeFabrication_etapes(
    @id_procedeFabrication INT
)
AS
BEGIN
    SET NOCOUNT ON

    SELECT Etape.id, Etape.nom FROM ProcedeFabrication_Etape
    JOIN Etape ON Etape.id = ProcedeFabrication_Etape.id_etape
    WHERE ProcedeFabrication_Etape.id_procedeFabrication = @id_procedeFabrication;
END
GO

CREATE OR ALTER PROCEDURE add_procedeFabrication(
    @nom VARCHAR(64),
    @description VARCHAR(1024),
    @id_modele INT
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO ProcedeFabrication (nom, description, id_modele) VALUES (@nom, @description, @id_modele);
END
GO

CREATE OR ALTER PROCEDURE add_test_to_procedeFabrication(
    @id_procedeFabrication INT,
    @id_test INT
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO ProcedeFabrication_Test (id_procedeFabrication, id_test) VALUES (@id_procedeFabrication, @id_test);
END
GO

CREATE OR ALTER PROCEDURE add_etape_to_procedeFabrication(
    @id_procedeFabrication INT,
    @id_etape INT
)
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO ProcedeFabrication_Etape (id_procedeFabrication, id_etape) VALUES (@id_procedeFabrication, @id_etape);
END
GO

CREATE OR ALTER PROCEDURE delete_procedeFabrication(
    @id INT
)
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE FROM ProcedeFabrication WHERE id = @id;
END
GO

CREATE OR ALTER PROCEDURE delete_test_from_procedeFabrication(
    @id_procedeFabrication INT,
    @id_test INT
)
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE FROM ProcedeFabrication_Test WHERE id_test = @id_test AND id_procedeFabrication = @id_procedeFabrication;
END
GO

CREATE OR ALTER PROCEDURE delete_etape_from_procedeFabrication(
    @id_procedeFabrication INT,
    @id_etape INT
)
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM ProcedeFabrication_Etape WHERE id_etape = @id_etape AND id_procedeFabrication = @id_procedeFabrication;
END
GO

CREATE OR ALTER PROCEDURE update_procedeFabrication(
    @id INT,
    @nom VARCHAR(64),
    @description VARCHAR(1024),
    @id_modele INT
)
AS
BEGIN
    SET NOCOUNT ON

    UPDATE ProcedeFabrication
    SET nom = @nom, description = @description, id_modele = @id_modele
    WHERE id = @id;
END
GO