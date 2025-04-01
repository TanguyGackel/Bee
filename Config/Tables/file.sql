-- MODELES
INSERT INTO Modele (nom, description, pUHT, gamme) VALUES
                                                       ('Model Alpha', N'Premier modèle haut de gamme', 1500, 'Premium'),
                                                       ('Model Beta', N'Modèle milieu de gamme', 800, 'Standard'),
                                                       ('Model Gamma', N'Modèle d’entrée de gamme', 400, N'Économique');

-- INGREDIENTS
INSERT INTO Ingredient (nom, description) VALUES
                                              ('Acier Inoxydable', N'Acier résistant à la corrosion'),
                                              ('Plastique ABS', N'Plastique robuste utilisé pour les coques'),
                                              ('Aluminium', N'Matériau léger et résistant');

-- CARACTERISTIQUES
INSERT INTO Caracteristique (nom, description) VALUES
                                                   (N'Résistance à l’eau', N'Peut être utilisé sous l’eau jusqu’à 10m'),
                                                   ('Bluetooth', N'Connexion sans fil avec une portée de 15m'),
                                                   (N'Économie d’énergie', N'Consomme 30% d’énergie en moins');

-- MODELE_INGREDIENT
INSERT INTO Modele_Ingredient (id_modele, id_ingredient, grammage) VALUES
                                                                       (1, 1, 500),  -- Model Alpha utilise 500g d'acier
                                                                       (1, 2, 200),  -- Model Alpha utilise 200g de plastique
                                                                       (2, 2, 300),  -- Model Beta utilise 300g de plastique
                                                                       (3, 3, 250);  -- Model Gamma utilise 250g d'aluminium

-- MODELE_CARACTERISTIQUE
INSERT INTO Modele_Caracteristique (id_modele, id_caracteristique) VALUES
                                                                       (1, 1),  -- Model Alpha est résistant à l'eau
                                                                       (1, 2),  -- et a Bluetooth
                                                                       (2, 2),  -- Model Beta a Bluetooth
                                                                       (3, 3);  -- Model Gamma est économique en énergie

-- PROCEDE FABRICATION
INSERT INTO ProcedeFabrication (nom, description, id_modele) VALUES
                                                                 (N'Procédé Alpha', N'Assemblage complexe en environnement stérile', 1),
                                                                 (N'Procédé Beta', N'Montage standard automatisé', 2),
                                                                 (N'Procédé Gamma', N'Assemblage simplifié manuel', 3);

-- ETAPES
INSERT INTO Etape (nom, description) VALUES
                                    ('Etape 1',N'Découpe des matériaux'),
                                    ('Etape 2','Assemblage des composants'),
                                    ('Etape 3',N'Contrôle qualité final');

-- PROCEDEFABRICATION_ETAPE
INSERT INTO ProcedeFabrication_Etape (id_procedeFabrication, id_etape) VALUES
                                                                           (1, 1),  -- Procédé Alpha commence par la découpe
                                                                           (1, 2),  -- puis l'assemblage
                                                                           (1, 3),  -- et le contrôle
                                                                           (2, 1),
                                                                           (2, 2),
                                                                           (3, 2),
                                                                           (3, 3);

-- TESTS
INSERT INTO Test (nom, description, type, valide) VALUES
                                                 ('Test1', N'Test d’étanchéité', 'Fonctionnel', 1),
                                                 ('Test2',N'Test de résistance mécanique', 'Industriel', 1),
                                                 ('Test3',N'Test de connectivité Bluetooth', 'Fonctionnel', 1),
                                                 ('Test4',N'Test d’économie d’énergie', 'Industriel', 0);

-- PROCEDEFABRICATION_TEST
INSERT INTO ProcedeFabrication_Test (id_procedeFabrication, id_test) VALUES
                                                                         (1, 1),  -- Procédé Alpha est testé pour l’étanchéité
                                                                         (1, 2),  -- et la résistance mécanique
                                                                         (1, 3),  -- et la connectivité
                                                                         (2, 2),  -- Procédé Beta teste la résistance
                                                                         (3, 4);  -- Procédé Gamma teste l'économie d'énergie