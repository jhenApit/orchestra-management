CREATE TABLE [dbo].[Section]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL,  
	[Principal] NVARCHAR(50) NULL,  
	[Assistant] NVARCHAR(50) NULL, 
)
