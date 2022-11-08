IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'AddressBookDb')
BEGIN
  CREATE DATABASE AddressBookDb;
END