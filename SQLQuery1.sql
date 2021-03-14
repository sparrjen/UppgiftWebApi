CREATE TABLE Customers (
    Id int not null identity(1,1) primary key,
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
    Email varchar(100) not null
)
GO 

CREATE TABLE Administrators (
    Id int not null identity(1,1) primary key,
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
    Email varchar(100) not null,
    AdminHash varbinary(max) not null,
    AdminSalt varbinary(max) not null
)
GO 

CREATE TABLE Cases (
    Id int not null identity(1,1) primary key,
    CustomerId int not null references Customers(Id),
    AdministratorId int not null references Administrators(Id),
    CaseDate datetime not null,
    UpdateCaseDate datetime null,
    CaseDescription nvarchar(max) not null,
    CaseStatus nvarchar(20) not null
) 

CREATE TABLE SessionTokens (
     AdminId int  not null primary key,
     AccessToken varbinary(max) not null
)