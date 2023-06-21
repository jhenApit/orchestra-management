CREATE TABLE [dbo].[Concert]    
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Description] NVARCHAR(255) NOT NULL,
	[Image] NVARCHAR(MAX) NULL,
	[PerformanceDate] DATETIME NOT NULL ,
	[OrchestraId] INT NULL,
	CONSTRAINT FK_ConcertOrchestra FOREIGN KEY ([OrchestraId]) REFERENCES [dbo].[Orchestra](Id)
)