using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoAlmacen.Models;
using ProyectoAlmacen.Pages.Profesor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoAlmacen.Pages.Almacenista
{
    public class SolicitudesAlmacenModel : PageModel
    {
        private readonly TuDbContext _dbContext;

        public SolicitudesAlmacenModel(TuDbContext context)
        {
            _dbContext = context;
        }

        public List<SolicitudConMateriales> SolicitudesConMateriales { get; set; }
        public Prestamo prestamo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Console.WriteLine("Error: /Almacenista/SolicitudesAlmacen error en tipo de usuario");
                Response.Redirect("/Error");
            }
            // Puedes personalizar esta lógica según tus necesidades
            var solicitudes = _dbContext.VistaSolicitudes
                .Where(s => s.EstadoSolicitud == "Aprobada")
                .ToList();

            SolicitudesConMateriales = solicitudes
            .Select(solicitud => new SolicitudConMateriales
            {
                Solicitud = solicitud,
                Materiales = _dbContext.VistaMaterialesSolicituds
                    .Where(material => material.SolicitudId == solicitud.SolicitudId)
                    .ToList()
            })
            .ToList();

            return Page();
        }

        public IActionResult OnPostPrestar(long solicitudId)
        {
            CambiarEstadoSolicitud(solicitudId, "Prestado");
            GenerarPrestamo(solicitudId);
            return RedirectToPage();
        }
        private void CambiarEstadoSolicitud(long solicitudId, string nuevoEstado)
        {
            var solicitud = _dbContext.Solicitudes.Find(solicitudId);

            if (solicitud != null)
            {
                solicitud.EstadoSolicitud = nuevoEstado;
                _dbContext.SaveChanges();
            }
        }

        private void GenerarPrestamo(long solicitudId)
        {
            // Obtener la solicitud correspondiente
            var solicitud = _dbContext.Solicitudes.Find(solicitudId);

            if (solicitud != null)
            {
                // Crear un nuevo objeto Prestamo
                var prestamo = new Prestamo
                {
                    Idsolicitud = solicitudId,
                    FechaDevolucion = null,
                    EstadoPrestamo = "Pendiente"
                };

                // Agregar el prestamo a la tabla Prestamos
                _dbContext.Prestamos.Add(prestamo);

                // Guardar los cambios en la base de datos
                _dbContext.SaveChanges();
            }
        }

    }
}
