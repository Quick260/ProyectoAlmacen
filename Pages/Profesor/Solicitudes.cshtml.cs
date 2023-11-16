using Microsoft.AspNetCore.Http;
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
                Response.Redirect("/Error");
            }

            var usuarioId = HttpContext.Session.GetString("UsuarioId");

            var solicitudes = _dbContext.VistaSolicitudes
            .Where(s => s.NominaProfesor == usuarioId.ToString() && s.EstadoSolicitud == "Pendiente")
            .ToList();
            Console.WriteLine($"Number of VistaSolicitudes: {solicitudes.Count}");
            Console.WriteLine(usuarioId);

            SolicitudesConMateriales = solicitudes
            .Select(solicitud => new SolicitudConMateriales
            {
                Solicitud = solicitud,
                Materiales = _dbContext.VistaMaterialesSolicituds
                    .Where(material => material.SolicitudId == solicitud.SolicitudId)
                    .ToList()
            })
            .ToList();
            Console.WriteLine($"Number of SolicitudConMateriales: {SolicitudesConMateriales.Count}");
        }
    }

    public class SolicitudConMateriales
    {
        public VistaSolicitude Solicitud { get; set; }
        public List<VistaMaterialesSolicitud> Materiales { get; set; }
    }
}