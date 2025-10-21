-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ParkingSimulator')
BEGIN
    CREATE DATABASE ParkingSimulator;
END
GO

-- Switch to the database
USE ParkingSimulator;
GO

-- Create table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tickets' AND xtype='U')
BEGIN
CREATE TABLE Tickets (
                         Id NVARCHAR(50) PRIMARY KEY,
                         Spot NCHAR(3) NOT NULL,
                         PurchaseTime DATETIME2 NOT NULL,
                         ExitTime DATETIME2 NULL,
                         IsPaid BIT NOT NULL DEFAULT 0
);
END
GO
SELECT * FROM Tickets