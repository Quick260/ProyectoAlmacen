using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoAlmacen.Pages.Profesor
{
    public class SolicitudesModel : PageModel
    {
        public List<SolicitudConMateriales> SolicitudesConMateriales { get; set; }

        private readonly TuDbContext _dbContext;
        public SolicitudesModel(TuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Profesor")
            {
                Console.WriteLine("Error: /Profesor/Solicitudes error en tipo de usuario");
                Response.Redirect("/Error");
            }

            var usuarioId = HttpContext.Session.GetString("UsuarioId");

            var solicitudes = _dbContext.VistaSolicitudes
            .Where(s => s.NominaProfesor == usuarioId.ToString() && s.EstadoSolicitud == "Pendiente")
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
        }

        public IActionResult OnPostAceptar(long solicitudId)
        {
            CambiarEstadoSolicitud(solicitudId, "Aprobada");
            return RedirectToPage();
        }

        public IActionResult OnPostRechazar(long solicitudId)
        {
            CambiarEstadoSolicitud(solicitudId, "Denegada");
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
    }

    public class SolicitudConMateriales
    {
        public VistaSolicitude Solicitud { get; set; }
        public List<VistaMaterialesSolicitud> Materiales { get; set; }
    }
}