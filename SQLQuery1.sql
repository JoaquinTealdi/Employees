create database DBEmpleado

use DBEmpleado

create table Departamento(
IdDepartamento int primary key identity,
Nombre varchar(50),
FechaCreacion datetime default getdate()
)

create table Empleado(
IdEmpleado int primary key identity,
NombreCompleto varchar(50),
IdDepartamento int references Departamento(IdDepartamento),
Sueldo int,
FechaContrato datetime,
FechaCreacion datetime default getdate()
)

insert into Departamento (nombre)
values('Administración'),
('Marketing'),
('Comercio'),
('Ventas');

insert into Empleado(NombreCompleto, IdDepartamento, Sueldo, FechaContrato)
values('Jose Lopez', 1, 1300, getdate())


select * from Departamento
select * from Empleado
