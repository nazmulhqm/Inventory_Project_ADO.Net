USE master
DROP DATABASE IF Exists InventoryDB
CREATE DATABASE InventoryDB

USE InventoryDB
GO

------==================================================================================
---                             Create Schema
------==================================================================================
CREATE SCHEMA ims
GO

USE InventoryDB

DROP TABLE IF EXISTS ims.Cities
CREATE TABLE ims.Cities
(
	CityID INT PRIMARY KEY IDENTITY,
	City VARCHAR(30) Not Null,
	Country VARCHAR(30) NOT NULL
)
GO

USE InventoryDB
DROP TABLE IF EXISTS ims.ContactTypes
CREATE TABLE ims.ContactTypes
(
	ContactTypeID INT PRIMARY KEY IDENTITY,
	ContactTypeName VARCHAR(30) NOT NULL,
	ContactTypeDescription VARCHAR(100) NOT NULL
)

USE InventoryDB
DROP TABLE IF EXISTS ims.Contacts
CREATE TABLE ims.Contacts
(
	ContactID INT PRIMARY KEY IDENTITY,
	ContactFirstName VARCHAR(20) not null,
	ContactLastName VARCHAR(20) not null,
	ContactTypeID INT FOREIGN KEY REFERENCES ims.ContactTypes(ContactTypeID) NOT NULL,
	Gender VARCHAR(20) not null,
	Addresses nvarchar (255),
	City int foreign key references ims.Cities(CityID) not null,
	Phone CHAR(15) CHECK (LEN(Phone)=11 and PHONE Like '[0][1][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') NULL,
	Email NVARCHAR(255) NULL 
)
GO

USE InventoryDB
DROP TABLE IF EXISTS ims.Customers
CREATE TABLE ims.Customers
(
	CustomerID INT PRIMARY KEY IDENTITY,
	CustomerName VARCHAR(20) not null,
	Gender VARCHAR(20) not null,
	Addresses nvarchar (255),
	City int foreign key references ims.Cities(CityID) not null,
	Phone CHAR(15) CHECK (LEN(Phone)=11 and PHONE Like '[0][1][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') NULL,
	Email NVARCHAR(255) NULL 
)

USE InventoryDB
DROP TABLE IF EXISTS ims.Roles
CREATE TABLE ims.Roles
(
	RolesID INT PRIMARY KEY IDENTITY,
	RoleName Varchar(30) NOT NULL,
	RoleDescription VARCHAR(255) NOT NULL
)

USE InventoryDB
DROP TABLE IF EXISTS ims.Authenticate
CREATE TABLE ims.Authenticate
(
	AuthenticateID INT PRIMARY KEY IDENTITY,
	UserName varchar(50) NOT NULL,
	Password varchar(50) NOT NULL,
	IsActive BIT NOT NULL,
	Role int foreign key references ims.Roles(RolesID) NULL
)

USE InventoryDB
DROP TABLE IF EXISTS ims.ProductCategories
CREATE TABLE ims.ProductCategories
(
	ProductCategoryID int PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(50) not null,
	Description VARCHAR(150) not null,
	ParentCategoriesID int FOREIGN KEY REFERENCES ims.ProductCategories(ProductCategoryID)
)
GO
USE InventoryDB



USE InventoryDB
DROP TABLE IF EXISTS ims.Products
CREATE TABLE ims.Products 
(
	ProductID INT PRIMARY KEY IDENTITY,
	item_code AS ('IN - '+Cast(ProductID AS VARCHAR(50))),
	ProductName VARCHAR(50) not null,
	ProductCategoryID INT REFERENCES ims.ProductCategories(ProductCategoryID),
	Description VARCHAR(200) not null,
	Image NVARCHAR(300) null,
	Supplier_id INT REFERENCES ims.Contacts(ContactID),
	UnitPrice decimal(10,2) not null,
	QuantityStock int not null default 0,
	QuantityAvailable int not null,
	PurchasePrice decimal(10,2) not null
);
GO


USE InventoryDB
DROP TABLE IF EXISTS ims.Sales
CREATE TABLE ims.Sales
(
	SaleID INT PRIMARY KEY IDENTITY,
	CustomerID INT FOREIGN KEY REFERENCES ims.Customers(CustomerID),
	SalesDate DATETIME,
	TransactionID UNIQUEIDENTIFIER DEFAULT newid(),
	Status BIT Default 0
)
GO

USE InventoryDB
DROP TABLE IF EXISTS ims.Discounts
CREATE TABLE ims.Discounts
(
	DiscountID INT PRIMARY KEY IDENTITY,
	DiscountName VARCHAR(20) NOT NULL,
	DiscountDetails VARCHAR(50) NULL,
	DiscountPercentage INT NOT NULL
)

USE InventoryDB
DROP TABLE IF EXISTS ims.SalesDetails
CREATE TABLE ims.SalesDetails
(
	SaleDetailID INT PRIMARY KEY IDENTITY,
	SaleID INT FOREIGN KEY REFERENCES ims.Sales(SaleID),
	ProductID INT FOREIGN KEY REFERENCES ims.Products(ProductID),
	DiscountID INT FOREIGN KEY REFERENCES ims.Discounts(DiscountID),
	Quantity INT,
	UnitPrice DECIMAL(10,2),
	TotalAmount DECIMAL(10,2)
)
GO

--Order Store pro
DROP FUNCTION IF EXISTS ims.fnAvailableQuantity
GO
CREATE FUNCTION ims.fnAvailableQuantity
(
	@productID varchar(20)
)
RETURNS int
BEGIN
RETURN(SELECT QuantityAvailable FROM ims.Products 
	   WHERE Products.item_code=@productID)
END
GO

USE InventoryDB
DROP VIEW IF EXISTS ims.vw_Manufacturers
GO
CREATE VIEW ims.vw_Manufacturers
AS 
SELECT c.ContactFirstName+' '+c.ContactLastName AS Name,Gender,c.Addresses,a.City,a.Country,ct.ContactTypeName,Phone
FROM ims.Contacts c Join ims.Cities a 
		ON c.City=a.CityID Join ims.ContactTypes ct
			ON c.ContactTypeID=ct.ContactTypeID
WHERE c.ContactTypeID=3
GO

USE InventoryDB
DROP VIEW IF EXISTS ims.vw_Suppliers
GO
CREATE VIEW ims.vw_Suppliers
AS 
SELECT c.ContactFirstName+' '+c.ContactLastName AS Name,Gender,c.Addresses,a.City,a.Country,ct.ContactTypeName,Phone
FROM ims.Contacts c Join ims.Cities a 
		ON c.City=a.CityID Join ims.ContactTypes ct
			ON c.ContactTypeID=ct.ContactTypeID
WHERE c.ContactTypeID=2
GO

USE InventoryDB
DROP VIEW IF EXISTS ims.vw_Suppliers
GO
CREATE VIEW ims.vw_Suppliers
AS 
SELECT c.ContactFirstName+' '+c.ContactLastName AS Name,Gender,c.Addresses,a.City,a.Country,ct.ContactTypeName,Phone
FROM ims.Contacts c Join ims.Cities a 
		ON c.City=a.CityID Join ims.ContactTypes ct
			ON c.ContactTypeID=ct.ContactTypeID
WHERE c.ContactTypeID=2
GO

USE InventoryDB
--DROP VIEW IF EXISTS ims.vw_Sales
GO
CREATE VIEW ims.vw_Sales
AS 
SELECT LEFT(TransactionID, 8) AS TransactionID,CustomerName AS Name,SalesDate,p.ProductName,sd.UnitPrice,Quantity,TotalAmount
FROM ims.Sales s Join ims.SalesDetails sd 
		ON s.SaleID=sd.SaleID Join ims.Products p
			ON p.ProductID=sd.ProductID Join ims.Customers c
				ON c.CustomerID = s.CustomerID Join ims.Discounts d
					ON d.DiscountID = sd.DiscountID
GO