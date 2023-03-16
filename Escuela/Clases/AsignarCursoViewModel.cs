using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escuela.Models;

namespace Escuela.Clases
{
    public class AsignarCursoViewModel
    {

        public Estudiantes Estudiante { get; set; }
        public List<Cursos> Cursos { get; set; }
        public int IdCurso { get; set; }
    }
}