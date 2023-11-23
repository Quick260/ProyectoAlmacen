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

        public List<Solicitude> SolicitudesConMateriales { get; set; } = new List<Solicitude>();
        public List<Solicitude> SolicitudesAtrasadas { get; set; } = new List<Solicitude>();
        public List<Prestamo> Prestamos { get; set; } = new List<Prestamo>();   
        public Prestamo prestamo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Console.WriteLine("Error: /Almacenista/SolicitudesAlmacen error en tipo de usuario");
                Response.Redirect("/Error");
            }

            SolicitudesConMateriales =_dbContext.Solicitudes
                                        .Include(m => m.MaterialSolicituds)
                                            .ThenInclude(m => m.NumeroInventarioNavigation)
                                                .ThenInclude(m => m.DatosMaterialesNavigation)
                                        .Include(m => m.LaboratorioNavigation)
                                        .Include(m => m.IdusuarioNavigation)
                                        .Include(m => m.ProfesorNavigation)
                                            .ThenInclude(p => p.IdusuarioNavigation)
                                        .Where(s => s.EstadoSolicitud == "Aprobada")
                                        .AsEnumerable()
                                        .Where(s => DateTime.Parse(s.FechaSolicitud) >= DateTime.Today)
                                        .OrderBy(s => s.FechaSolicitud)
                                        .ToList();

            SolicitudesAtrasadas = _dbContext.Solicitudes
                                        .Include(m => m.MaterialSolicituds)
                                            .ThenInclude(m => m.NumeroInventarioNavigation)
                                                .ThenInclude(m => m.DatosMaterialesNavigation)
                                        .Include(m => m.LaboratorioNavigation)
                                        .Include(m => m.IdusuarioNavigation)
                                        .Include(m => m.ProfesorNavigation)
                                            .ThenInclude(p => p.IdusuarioNavigation)
                                        .Where(s => s.EstadoSolicitud == "Prestado")
                                        .AsEnumerable()
                                        .Where(s => DateTime.Parse(s.FechaSolicitud) < DateTime.Today ||
                                        (DateTime.Parse(s.FechaSolicitud) == DateTime.Today &&
                                        TimeSpan.Parse(s.HoraRetorno) < (DateTime.Now.TimeOfDay - TimeSpan.FromMinutes(15))))
                                        .OrderBy(s => s.FechaSolicitud)
                                        .ToList();

            Prestamos = _dbContext.Prestamos.Include(p => p.IdsolicitudNavigation.MaterialSolicituds)
                                                    .ThenInclude(m => m.NumeroInventarioNavigation)
                                                        .ThenInclude(m => m.DatosMaterialesNavigation)
                                            .Include(m => m.IdsolicitudNavigation.LaboratorioNavigation)
                                            .Include(m => m.IdsolicitudNavigation.IdusuarioNavigation)
                                            .Include(m => m.IdsolicitudNavigation.ProfesorNavigation)
                                                .ThenInclude(p => p.IdusuarioNavigation)
                                            .Where(s => s.FechaDevolucion == null)                                                 
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
