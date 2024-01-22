CREATE PROCEDURE [sp_CreateDb] AS
BEGIN
    CREATE DATABASE [Store]
END


CREATE PROCEDURE [sp_CreateTable] AS
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Items')
        BEGIN
            CREATE TABLE [Items]
            (
                [Id]          INT PRIMARY KEY IDENTITY,
                [ItemName]    NVARCHAR(255),
                [Quantity]    INT,
                [Category]    NVARCHAR(255),
                [Price]       INT,
                [Description] NVARCHAR(MAX),
                [AddedDate]   DATETIME
            )
        END
END