USE InventoryDB
INSERT INTO ims.Cities(City,Country) 
			VALUES ('Dhaka','Bangladesh'),
				   ('Chattogram','Bangladesh'),
				   ('Rajshahi','Bangladesh'),
				   ('Khulna','Bangladesh'),
				   ('Barishal','Bangladesh'),
				   ('Sylhet','Bangladesh'),
				   ('Rangpur','Bangladesh'),
				   ('Mymensingh','Bangladesh')
GO

USE InventoryDB
INSERT INTO ims.ContactTypes(ContactTypeName,ContactTypeDescription)
			VALUES 
				   ('Supplier', 'The wizards behind the curtain, supplying us with the magic we need!'),
				   ('Manufacturer', 'The master craftsmen, turning dreams into reality, one widget at a time!');
GO

USE InventoryDB
INSERT INTO ims.Customers
VALUES('Mousumi Khan',    'Female','Block B, Lalmatia, Behind Lake Circus','1', '01887766778','Khan@gmail.com'),
('Rajib Hossain',   'Male','2nd Floor, Building 9, Uttara','1', '01678901234','Hossain'),
('Nusrat Chowdhury','Female','Flat 301, Orchid Plaza, Shyamoli','1', '01522113344','Nusrat@mail.com'),
('Mehedi Hossain',  'Male','67, Elephant Road, Hathi Bagan','1', '01666699888','hossain@mail.com')
GO
																				
USE InventoryDB
INSERT INTO ims.Contacts(ContactFirstName,ContactLastName,ContactTypeID,Gender,Addresses,City,Phone,Email)
			VALUES ('Md. Rahman', 'Ali',     '1','Male','Flat 101, House 15, Road 5, Gulshan','1', '01711223344','Ali@gmail.com'),
					('Sara', 'Ahmed',        '2','Female','23, Green Road, Farmgate','1', '01666557788','Ahmed@gmail.com'),
					('Tariqul', 'Islam',     '1','Male','Holding 7, Ward 4, Mirpur','1', '01554488999','Islam'),
					('Ayesha', 'Akter',      '2','Female','34, Nandan Kanan, Bashundhara','1', '01999001122','Akter'),
					('Sabrina', 'Nasrin',    '1','Female','14, Kazi Nazrul Islam Avenue, Karwan Bazar','1', '01755566777','Nasrin'),
					('Faisal', 'Haque',      '2','Male','88, DIT Avenue, Malibagh','1', '01899005555','Haque@gmail.com'),
					('Imran', 'Kabir',       '1','Male','House 45, Road 8, Baridhara','1', '01777665544','Kabir@mail.com'),
					('Nadia', 'Ahmed',       '2','Female','5th Floor, Star Tower, Tejgaon','1', '01999778866','Nadia@gmail.com'),
					('Tasnim', 'Rahman',     '1','Female','3C, Niketon, Gulshan 1','1', NULL,'TasnimR@Yahoo.com'),
					('Aminul', 'Islam',      '2','Male','6, Dhanmondi, Lake Road','1', '01555669911','Aminul@Yahoo.com'),
					('Farhana', 'Khan',      '1','Female','Sector 11, Uttara, Near Rajlakshmi Complex','1', NULL,'Farhana@Mail.com'),
					('Belal', 'Ahmed',       '1','Male','House 29, Road 16, Banani','1', '01999774566','Belal@gmail.com'),
					('Kamal', 'Hossain',     '2','Male','Flat 201, Rupayan Golden Age, Puran Dhaka','1', '01666109888','Kamal@mail.com'),
					('Taushi', 'Rahman',     '2','Female','13, Green View, Mirpur DOHS','1', NULL,'RadmanT@Yahoo.com'),
					('Zahidul', 'Islam',     '1','Male','18, North Road, Vatara','1', '01555669311','Zahid094@Yahoo.com'),
					('Arman', 'Khan',        '2','Male','Halisahar, Block-B','2', NULL,'Arman@Mail.com'),
					('Amit', 'Das',			 '1','Male','Flat 101, House 15, Road 5, Jamal Khan','2', '01666778899', 'Amit@Gmail.com'),
			 	    ('Nazia', 'Islam',		 '1','Female','23, Green Road, Agrabad','2', '01788990015', 'Nazia@gmail.com'),
			 	    ('Kamal', 'Hossain',	 '2','Male','Block B, Lalmatia','2', '01556667777', 'Kamal@gmail.com'),
			 	    ('Rakib', 'Khan',		 '2','Male','Holding 7, Ward 4, Mirpur','2', '01998887766', 'Rakib@gmail.com'),
			 	    ('Sonia', 'Ahmed',		 '1','Female','34, Nandan Kanan, Firingee Bazar','2', '01665554444', 'Sonia@gmail.com'),
			 	    ('Imtiaz', 'Chowdhury',  '2','Male','2nd Floor, Building 9, Panchlaish','2', '01770001111', 'Imtiaz@gmail.com'),
			 	    ('Nabila', 'Rahman',     '1','Female','14, Kazi Nazrul Islam Avenue, Chawkbazar','2', '01886663333', 'Rahman@gmail.com')
GO


Insert into ims.Discounts
VALUES('0% Discount','0 percentage discount on each and all products',0),
	  ('10% Discount','10 percentage discount on each and all products',10),
	  ('20% Discount','20 percentage discount on each and all products',20),
	  ('25% Discount','25 percentage discount on each and all products',25)
GO



INSERT INTO ims.Roles (RoleName, RoleDescription)
VALUES 
    ('Admin', 'Admin role with full access and control'),
    ('Manager', 'Manager role with access to inventory management features'),
    ('Employee', 'Employee role with limited access to inventory functions'),
    ('Auditor', 'Auditor role with read-only access for auditing purposes');
GO

	INSERT INTO ims.Authenticate (UserName, Password, IsActive, Role)
VALUES ('Admin', 'admin', 1, 1),
       ('Nazmul', 'nazmul', 1, 1);
GO

INSERT INTO ims.ProductCategories (CategoryName, Description, ParentCategoriesID)
			 VALUES('Electronics', 'Electronic devices and accessories', NULL),
				   ('Groceries', 'Various food and grocery items', NULL),
				   ('Soft Drinks', 'Non-alcoholic beverages', NULL),
				   ('Mobile Phones', 'Cell phones and accessories', 1),
				   ('Laptops', 'Portable computers and accessories', 1),
				   ('Smart TVs', 'Television sets with smart features', 1),
				   ('Fresh Produce', 'Fresh fruits and vegetables', 2),
				   ('Canned Goods', 'Canned and preserved food items', 2),
				   ('Snacks', 'Snack items and packaged foods', 2),
				   ('Sodas', 'Carbonated soft drinks', 3),
				   ('Juices', 'Fruit and vegetable juices', 3),
				   ('Water', 'Bottled water and related products', 3)
GO

-- Inserting sample data into ims.Products table with realistic product names (15 products)
INSERT INTO ims.Products (ProductName, ProductCategoryID, Description, Image, Supplier_id, UnitPrice, QuantityStock, QuantityAvailable, PurchasePrice)
VALUES 
    ('Apple MacBook Pro 13-inch', 1, 'Thin and light laptop with Retina display', '1.jpg', 1, 1499, 120, 120, 1200),
    ('Samsung Galaxy Watch 4', 7, 'Smartwatch with health and fitness tracking features', '2.jpg', 2, 299, 150, 150, 250),
    ('Sony PlayStation 5', 8, 'Next-generation gaming console with ultra-fast SSD', '3.jpg', 3, 499, 200, 200, 450),
    ('Bose QuietComfort 45 Headphones', 3, 'Noise-canceling headphones with Bluetooth connectivity', '4.jpg', 4, 329, 125, 125, 280),
    ('Canon EOS Rebel T8i DSLR Camera', 6, '24.1MP digital SLR camera with 4K video recording', '5.jpg', 5, 899, 150, 150, 750),
    ('iPhone 13 Pro Max', 2, 'Apple smartphone with Pro camera system', '6.jpg', 2, 1099, 150, 150, 950),
    ('LG CX 55-inch OLED TV', 9, '4K Ultra HD Smart OLED TV with AI ThinQ', '7.jpg', 4, 1499, 120, 120, 1300),
    ('Microsoft Surface Pro 8', 1, 'Versatile 2-in-1 laptop and tablet with Windows 11', '8.jpg', 3, 1299, 200, 200, 1100),
    ('GoPro HERO10 Black', 10, 'Action camera with HyperSmooth 4.0 stabilization', '9.jpg', 5, 499, 150, 150, 400),
    ('Logitech MX Master 3 Mouse', 11, 'Advanced wireless mouse with ergonomic design', '10.jpg', 1, 99, 130, 130, 80)
GO
    
