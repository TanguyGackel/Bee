CREATE OR ALTER PROCEDURE get_tests
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT id, nom, type, valide FROM Test
   
END
GO

CREATE OR ALTER PROCEDURE get_tests_by_id(
    @id_test INT
)
AS
BEGIN 
    
    SELECT nom, description, type, valide FROM Test 
    WHERE id = @id_test;
    
END
GO

CREATE OR ALTER PROCEDURE get_tests_by_name(
    @nom VARCHAR(64)
)
AS
BEGIN 
    
    SELECT id, nom, type, valide FROM Test
    WHERE nom LIKE '%'+@nom+'%';
    
END
GO

CREATE OR ALTER PROCEDURE update_test(
    @id_test INT,
    @valide_field bit
)
AS
BEGIN

    UPDATE Test SET valide = @valide_field
    WHERE id = @id_test;

END
GO

CREATE OR ALTER PROCEDURE get_procede_test(
    @id_test INT
)
AS
BEGIN

    SELECT ProcedeFabrication.id, ProcedeFabrication.nom, ProcedeFabrication.description FROM ProcedeFabrication
    JOIN ProcedeFabrication_Test ON ProcedeFabrication_Test.id_procedeFabrication = ProcedeFabrication.id
    WHERE ProcedeFabrication_Test.id_test = @id_test;

END
GO

