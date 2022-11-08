BEGIN TRANSACTION
  Use AddressBookDb;

	IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Users' and xtype = 'U')
	BEGIN
	CREATE TABLE Users (
		UserId INT PRIMARY KEY IDENTITY (1, 1),
		FirstName NVARCHAR(100),
		LastName NVARCHAR(100)
	)
	END

	IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Addresses' and xtype = 'U')
	BEGIN
	CREATE TABLE Addresses (
		AddressId INT PRIMARY KEY IDENTITY (1, 1),
		UserId INT FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
		Street NVARCHAR(100),
		PostalCode NVARCHAR(100),
		Country NVARCHAR(100),
	)
	END

COMMIT;