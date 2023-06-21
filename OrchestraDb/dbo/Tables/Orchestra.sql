CREATE TABLE [dbo].[Orchestra]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL,
	[Image] NVARCHAR(MAX) NULL,
	[ConductorId] INT NOT NULL,
	[Date] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Description] NVARCHAR(MAX) NULL, 
    CONSTRAINT FK_OrchestraConductor FOREIGN KEY ([ConductorId]) REFERENCES [dbo].[Conductor](Id)
)
