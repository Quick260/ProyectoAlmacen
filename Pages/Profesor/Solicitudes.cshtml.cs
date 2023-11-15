
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

namespace ProyectoAlmacen.Pages.Profesor
{
    public class SolicitudesModel : PageModel
    {
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

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var solicitudes = (from s in _dbContext.Solicitudes
                   join u in _dbContext.Usuarios on s.Idusuario equals u.Id
                   join l in _dbContext.Laboratorios on s.Laboratorio equals l.Id
                   join p in _dbContext.Profesores on s.Profesor equals p.Id
                   where p.Id == usuarioId
                   select new SolicitudViewModel
                   {
                       SolicitudID = s.Id,
                       NombreUsuario = u.NombreCompleto,
                       NombreLaboratorio = l.NombreLaboratorio,
                       NominaProfesor = p.Nomina,
                       FechaCreacion = s.FechaCreacion,
                       FechaSolicitud = s.FechaSolicitud,
                       HoraSolicitud = s.HoraSolicitud,
                       HoraRetorno = s.HoraRetorno,
                       EstadoSolicitud = s.EstadoSolicitud
                   }).ToList();
        }
    }

    
}
