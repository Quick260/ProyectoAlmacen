using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

public class CrearUsuarioModel : PageModel
{
    private readonly TuDbContext _dbContext;

    [BindProperty]
    public Usuario NuevoUsuario { get; set; }
    [BindProperty]
    public Estudiante NuevoEstudiante { get; set; }
    [BindProperty]
    public Coordinadore NuevoCoordinador { get; set; }
    [BindProperty]
    public Profesore NuevoProfesor { get; set; }
    

    public CrearUsuarioModel(TuDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        // Método ejecutado al cargar la página
    }

    public IActionResult OnPost()
    {
        _dbContext.Usuarios.Add(NuevoUsuario);
        _dbContext.SaveChanges();
        long idGenerado = NuevoUsuario.Id;
        

        if (NuevoUsuario.TipoUsuario == 1)
        {
            NuevoEstudiante.Idusuario = idGenerado;
            _dbContext.Estudiantes.Add(NuevoEstudiante);
            _dbContext.SaveChanges();
        }
        if (NuevoUsuario.TipoUsuario == 3)
        {
            NuevoProfesor.Idusuario = idGenerado;
            NuevoProfesor.Nomina = Request.Form["NominaProfesor"];
            NuevoProfesor.MateriasImpartidas = Request.Form["MateriaProfesor"];
            NuevoProfesor.SalonesAsignados = long.TryParse(Request.Form["SalonProfesor"], out long SalonesAsignados) ? SalonesAsignados : (long?)null;
            _dbContext.Profesores.Add(NuevoProfesor);
            _dbContext.SaveChanges();
        }
        if (NuevoUsuario.TipoUsuario == 4)
        {
            NuevoCoordinador.Idusuario = idGenerado;
            NuevoCoordinador.NumeroIdentificacion = Request.Form["NumeroIdentificacionCoordinador"];
            _dbContext.Coordinadores.Add(NuevoCoordinador);
            _dbContext.SaveChanges();
        }

        return RedirectToPage("/Index");
    }

    /*bool validacionesUsuario(Usuario)
    {
        return true;
    }*/
}
