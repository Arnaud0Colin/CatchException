CREATE TABLE [dbo].[CatchMe_Status]
(
	[CodeStatus] TINYINT NOT NULL PRIMARY KEY, 
    [Libelle] NVARCHAR(100) NOT NULL,
	[Suivant] NVARCHAR(100) NULL, 
    [Order] INT NOT NULL DEFAULT 0,
)
