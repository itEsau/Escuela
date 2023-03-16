using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escuela.Models;
using Escuela.Clases;
using System.Net;
using System.Data.Entity;

namespace Escuela.Controllers
{
    public class EstudianteController : Controller
    {


        public DBEscuelaEntities db = new DBEscuelaEntities();
        public ActionResult Index(string searchString)
        {
            var estudiantes = db.Estudiantes.Include(e => e.CursoEstudiante);

            if (!String.IsNullOrEmpty(searchString))
            {
                estudiantes = estudiantes.Where(e => e.nombre.Contains(searchString));
            }

            ViewBag.Cursos = db.Cursos.ToList();
            return View(estudiantes.ToList());
        }
        public ActionResult AsignarCursos(int idEstudiante)
        {
            var cursos = db.Cursos.ToList();
            ViewBag.IdCurso = db.Cursos.Select(c => new SelectListItem { Value = c.idCurso.ToString(), Text = c.nombre }).ToList();

            ViewBag.IdEstudiante = idEstudiante;
            return View();
        }

        [HttpPost]
        public ActionResult AsignarCursos(int idEstudiante, int idCurso)
        {
            try
            {
                ViewBag.idEstudiante = new SelectList(db.Estudiantes, "idEstudiante", "nombre");
                ViewBag.idCurso = new SelectList(db.Cursos, "idCurso", "nombre");
                // Buscar el estudiante en la base de datos
                var estudiante = db.Estudiantes.Find(idEstudiante);
                if (estudiante == null)
                {
                    return HttpNotFound();
                }

                // Buscar el curso en la base de datos
                var curso = db.Cursos.Find(idCurso);
                if (curso == null)
                {
                    return HttpNotFound();
                }

                // Verificar que el estudiante no tenga ya asignado el curso
                var cursosAsignados = estudiante.CursoEstudiante.Select(c => c.idCurso);
                if (cursosAsignados.Contains(idCurso))
                {
                    return Content("Ya asignado"); // Devuelve una respuesta con un mensaje de error
                }

                // Asignar el curso al estudiante
                estudiante.CursoEstudiante.Add(new CursoEstudiante { Estudiantes = estudiante, Cursos = curso });
                db.SaveChanges();

                return Content("Asignado correctamente"); // Devuelve una respuesta con un mensaje de éxito
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message); // Devuelve una respuesta con un mensaje de error
            }
        }






        // Método para remover cursos de un estudiante

        public ActionResult RemoverCursos(int idEstudiante, List<int> idCursos)
        {
            // Verificar que el estudiante exista
            var estudiante = db.Estudiantes.FirstOrDefault(e => e.idEstudiante == idEstudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Verificar que los cursos existan
            var cursos = db.Cursos.Where(c => idCursos.Contains(c.idCurso)).ToList();
            if (cursos.Count != idCursos.Count)
            {
                return HttpNotFound();
            }

            // Verificar que el estudiante tenga asignado los cursos que se desean remover
            var cursosAsignados = db.CursoEstudiante.Where(ce => ce.idEstudiante == idEstudiante && idCursos.Contains(ce.idCurso)).ToList();
            if (cursosAsignados.Count != idCursos.Count)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "El estudiante no tiene asignado alguno de los cursos que se desean remover");
            }

            // Remover los cursos asignados al estudiante
            db.CursoEstudiante.RemoveRange(cursosAsignados);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = idEstudiante });
        }
    }
}
