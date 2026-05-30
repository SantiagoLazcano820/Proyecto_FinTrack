DROP DATABASE IF EXISTS DbFinTrack;
CREATE DATABASE DbFinTrack;
USE DbFinTrack;

CREATE TABLE Security (
    Id INT NOT NULL AUTO_INCREMENT,
    Login VARCHAR(50) NOT NULL,
    Password VARCHAR(200) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Role VARCHAR(15) NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE Role (
    Id INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(20) NOT NULL, 
    PRIMARY KEY (Id)
);

CREATE TABLE User (
    Id INT NOT NULL AUTO_INCREMENT,
    RoleId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    UNIQUE (Email)
);

CREATE TABLE Category (
    Id INT NOT NULL AUTO_INCREMENT,
    UserId INT NOT NULL, 
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(100) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (Id)
);

CREATE TABLE Transaction (
    Id INT NOT NULL AUTO_INCREMENT,
    UserId INT NOT NULL,
    CategoryId INT NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    Type VARCHAR(10) NOT NULL, 
    Date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Description VARCHAR(255) NULL,
    PRIMARY KEY (Id)
);

INSERT INTO Role (Name) VALUES ('Admin'), ('StandardUser');

INSERT INTO User (RoleId, Name, LastName, Email, Password, IsActive) VALUES 
(1, 'Santiago', 'Lazcano', 'santiago@ucb.edu.bo', 'Pass123!', 1),
(2, 'Ana', 'García', 'ana.garcia@gmail.com', 'Pass123!', 1),
(2, 'Roberto', 'Sánchez', 'roberto.s@outlook.com', 'Pass123!', 1),
(2, 'Lucía', 'Mendoza', 'lucia.m@ucb.edu.bo', 'Pass123!', 1),
(2, 'Marcos', 'Torres', 'marcos.t@gmail.com', 'Pass123!', 1),
(2, 'Elena', 'Ríos', 'elena.r@outlook.com', 'Pass123!', 1),
(2, 'David', 'Vargas', 'd.vargas@ucb.edu.bo', 'Pass123!', 1),
(2, 'Sofía', 'Castro', 'sofi.c@gmail.com', 'Pass123!', 1),
(2, 'Javier', 'Ortiz', 'j.ortiz@outlook.com', 'Pass123!', 1),
(2, 'Valeria', 'Luna', 'valeria.l@ucb.edu.bo', 'Pass123!', 0),
(2, 'Andrés', 'Silva', 'asilva@gmail.com', 'Pass123!', 1),
(2, 'Paola', 'Rojas', 'p.rojas@outlook.com', 'Pass123!', 1),
(2, 'Diego', 'Mejía', 'diego.m@ucb.edu.bo', 'Pass123!', 1),
(2, 'Camila', 'Peña', 'camila.p@gmail.com', 'Pass123!', 0),
(2, 'Gabriel', 'Núñez', 'g.nunez@outlook.com', 'Pass123!', 1),
(2, 'Isabella', 'León', 'isa.leon@ucb.edu.bo', 'Pass123!', 1),
(2, 'Mateo', 'Bravo', 'mbravo@gmail.com', 'Pass123!', 1),
(2, 'Sara', 'Mora', 'sara.m@outlook.com', 'Pass123!', 1),
(2, 'Nicolás', 'Paredes', 'n.paredes@ucb.edu.bo', 'Pass123!', 1),
(2, 'Daniela', 'Flores', 'daniela.f@gmail.com', 'Pass123!', 1),
(2, 'Samuel', 'Guerra', 'samu.g@outlook.com', 'Pass123!', 1),
(2, 'Victoria', 'Cano', 'v.cano@ucb.edu.bo', 'Pass123!', 1),
(2, 'Joaquín', 'Díaz', 'joaco.d@gmail.com', 'Pass123!', 0),
(2, 'Martina', 'Cruz', 'martu.c@outlook.com', 'Pass123!', 1),
(2, 'Luis', 'Soto', 'luis.s@ucb.edu.bo', 'Pass123!', 1),
(2, 'Fernanda', 'Lara', 'fer.lara@gmail.com', 'Pass123!', 1),
(2, 'Hugo', 'Bernal', 'h.bernal@outlook.com', 'Pass123!', 1),
(2, 'Renata', 'Reyes', 'reny.r@ucb.edu.bo', 'Pass123!', 0),
(2, 'Sebastián', 'Osorio', 'seba.o@gmail.com', 'Pass123!', 1),
(1, 'Admin', 'Root', 'admin@ucb.edu.bo', 'Ucb.2025', 1);

-- Sembrado de la tabla Security con hashes PBKDF2 correspondientes a sus claves
INSERT INTO Security (Login, Password, Name, Role) VALUES 
('admin@ucb.edu.bo', '1000.wHzn4g1ZG/Cjy6FziJNZeQ==.jIO0ml54qGCgMBqRRWsGnT7anbKjLNzuGEWpzmDo+R8=', 'Admin Root', 'Admin'),
('santiago@ucb.edu.bo', '1000.98l8wKiSh5ro7Khwl/06Cg==.CFjCfX1WjKmK8ohvjVbphr8YbO7jjyPw6X4SakVsox4=', 'Santiago Lazcano', 'Admin'),
('ana.garcia@gmail.com', '1000.98l8wKiSh5ro7Khwl/06Cg==.CFjCfX1WjKmK8ohvjVbphr8YbO7jjyPw6X4SakVsox4=', 'Ana García', 'StandardUser');

INSERT INTO Category (UserId, Name, Description) SELECT Id, 'Sueldo', 'Ingreso mensual' FROM User;
INSERT INTO Category (UserId, Name, Description) SELECT Id, 'Transporte', 'Gasolina y micro' FROM User;
INSERT INTO Category (UserId, Name, Description) SELECT Id, 'Alimentación', 'Supermercado' FROM User;
INSERT INTO Category (UserId, Name, Description) SELECT Id, 'Entretenimiento', 'Cine y salidas' FROM User;
INSERT INTO Category (UserId, Name, Description) SELECT Id, 'Salud', 'Farmacia' FROM User;

INSERT INTO Transaction (UserId, CategoryId, Amount, Type, Date, Description)
SELECT U.Id, C.Id, ROUND(4000 + (RAND() * 2000), 2), 'Income', '2026-04-01 08:00:00', 'Sueldo mensual'
FROM User U JOIN Category C ON U.Id = C.UserId WHERE C.Name = 'Sueldo';

INSERT INTO Transaction (UserId, CategoryId, Amount, Type, Date, Description)
SELECT U.Id, C.Id, ROUND(50 + (RAND() * 300), 2), 'Expense', '2026-04-10 12:00:00', 'Gasto diario'
FROM User U JOIN Category C ON U.Id = C.UserId WHERE C.Name IN ('Alimentación', 'Transporte') LIMIT 60;

ALTER TABLE User 
ADD CONSTRAINT FK_User_Role 
FOREIGN KEY (RoleId) REFERENCES Role(Id);

ALTER TABLE Category 
ADD CONSTRAINT FK_Category_User 
FOREIGN KEY (UserId) REFERENCES User(Id);

ALTER TABLE Transaction 
ADD CONSTRAINT FK_Transaction_User 
FOREIGN KEY (UserId) REFERENCES User(Id);

ALTER TABLE Transaction 
ADD CONSTRAINT FK_Transaction_Category 
FOREIGN KEY (CategoryId) REFERENCES Category(Id);
