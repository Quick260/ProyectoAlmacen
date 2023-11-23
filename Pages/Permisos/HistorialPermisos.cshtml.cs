

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoAlmacen.Models;

public class HistorialPermisosModel : PageModel{

    TuDbContext _dbContext;

    public HistorialPermisosModel(TuDbContext dbContext){
        _dbContext = dbContext;
    }

    
    public List<VistaSolicitude> Solicitudes { get; set; }
    public List<VistaMaterialesSolicitud> DetallesSolicitud { get; set; }

public void OnGet()
{
    Solicitudes = _dbContext.VistaSolicitudes
        .Where(s => s.UsuarioId == HttpContext.Session.GetInt32("UsuarioId"))
        .ToList();

    Solicitudes.ForEach(s => 
    {
        s.FechaCreacion = DateTime.Parse(s.FechaCreacion).ToString("dd/MM/yyyy");

        // Obtén el nombre completo del profesor
        var profesor = _dbContext.Profesores
            .Include(p => p.IdusuarioNavigation) // Asegúrate de que IdusuarioNavigation esté incluido
            .FirstOrDefault(p => p.Nomina == s.NominaProfesor);

            if (profesor != null && profesor.IdusuarioNavigation != null)
            {
                // Asigna el nombre completo al campo correspondiente en VistaSolicitude
                s.NominaProfesor = profesor.IdusuarioNavigation.NombreCompleto;
            }
        });

    DetallesSolicitud = _dbContext.VistaMaterialesSolicituds.ToList();

}

    
}