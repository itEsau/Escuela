CREATE DATABASE DBEscuela;

USE DBEscuela;

CREATE TABLE Estudiantes (
    idEstudiante INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    correo VARCHAR(255) NOT NULL
);

CREATE TABLE Cursos (
    idCurso INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(255) NOT NULL
);

CREATE TABLE CursoEstudiante (
    idCursoEstudiante INT PRIMARY KEY IDENTITY(1,1),
    idEstudiante INT NOT NULL,
    idCurso INT NOT NULL,
    CONSTRAINT FK_CursoEstudiante_Estudiantes FOREIGN KEY (idEstudiante)
        REFERENCES Estudiantes(idEstudiante),
    CONSTRAINT FK_CursoEstudiante_Cursos FOREIGN KEY (idCurso)
        REFERENCES Cursos(idCurso),
    CONSTRAINT UQ_CursoEstudiante UNIQUE (idEstudiante, idCurso)
);
