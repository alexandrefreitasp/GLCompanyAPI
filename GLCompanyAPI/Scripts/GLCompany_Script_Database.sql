--13/03/2025
-- Alex Pereira

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GLCompany')
BEGIN
    CREATE DATABASE GLCompany;
END;
GO

USE GLCompany;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Companies' AND xtype='U')
BEGIN
    CREATE TABLE Companies (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Exchange NVARCHAR(50) NOT NULL,
        Ticker NVARCHAR(10) NOT NULL,
        Isin NVARCHAR(12) NOT NULL UNIQUE,
        Website NVARCHAR(255) NULL
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL
    );
END;
GO

