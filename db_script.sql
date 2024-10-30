---- Crear la base de datos
CREATE DATABASE EVA_DB;

USE EVA_DB;

---- Crear la tabla DE USERS
CREATE TABLE Users (
    Id uniqueidentifier NOT NULL,
	Name nvarchar(250) NOT NULL,
	Email nvarchar(150) NOT NULL,
	Password nvarchar(max) NOT NULL,
	IsActive bit NOT NULL,
	Created datetime2(7) NOT NULL,
	LastLogin datetime2(7) NOT NULL,
	Modified  datetime2(7) NOT NULL,
	Token nvarchar (max) NOT NULL,

    CONSTRAINT PK_User PRIMARY KEY (Id ASC)
);


--- Crear la tabla DE PHONES
CREATE TABLE Phones(
	Id int IDENTITY(1,1) NOT NULL,
	UserId uniqueidentifier NOT NULL,
	Number nvarchar(max) NOT NULL,
	CityCode nvarchar(max) NOT NULL,
	CountryCode nvarchar(max) NOT NULL,
 CONSTRAINT PK_Phones PRIMARY KEY CLUSTERED (Id ASC)
)

--- Crear FK
ALTER TABLE Phones  WITH CHECK ADD  CONSTRAINT FK_Phones_Users_UserId FOREIGN KEY(UserId)
REFERENCES Users (Id)
ON DELETE CASCADE

