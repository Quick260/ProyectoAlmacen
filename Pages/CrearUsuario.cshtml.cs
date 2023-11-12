using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

public class CrearUsuarioModel : PageModel
{
    private readonly TuDbContext _dbContext;

    [BindProperty]
    public Usuario NuevoUsuario { get; set; }
    public Estudiante NuevoEstudiante { get; set; }
    public Profesore NuevoProfesor { get; set; }
    public Coordinadore NuevoCoordinador { get; set; }

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
        if (!ModelState.IsValid)
        {
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    Console.WriteLine($"Model Error: {error.ErrorMessage}");
                }
            }
            return RedirectToPage("/Error");
        }

        NuevoUsuario.Id = _dbContext.Usuarios.Max(u => (long?)u.Id) ?? 0;
        _dbContext.Usuarios.Add(NuevoUsuario);
        _dbContext.SaveChanges();

        if (NuevoUsuario.TipoUsuario == "1")
        {
            NuevoEstudiante.Id = _dbContext.Estudiantes.Max(u => (long?)u.Id) ?? 0;
            NuevoEstudiante.Idusuario = NuevoUsuario.Id;
            _dbContext.Estudiantes.Add(NuevoEstudiante);
            _dbContext.SaveChanges();
        }
        else if (NuevoUsuario.TipoUsuario == "3")
        {
            NuevoProfesor.Id = _dbContext.Profesores.Max(u => (long?)u.Id) ?? 0;
            NuevoProfesor.Idusuario = NuevoUsuario.Id;
            _dbContext.Profesores.Add(NuevoProfesor);
            _dbContext.SaveChanges();
        }
        else if (NuevoUsuario.TipoUsuario == "4")
        {
            NuevoCoordinador.Id = _dbContext.Coordinadores.Max(u => (long?)u.Id) ?? 0;
            NuevoCoordinador.Idusuario = NuevoUsuario.Id;
            _dbContext.Coordinadores.Add(NuevoCoordinador);
            _dbContext.SaveChanges();
        }

        return RedirectToPage("/Index");
    }

}
