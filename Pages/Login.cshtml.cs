using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

public class LoginModel : PageModel
{
    private readonly TuDbContext _dbContext;


    public LoginModel(TuDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        // Método ejecutado al cargar la página
    }

    public IActionResult OnPost(string Usuario, string Contrasena, int TipoUsuario)
    {
        if (TipoUsuario == 1)
        {
            var estudiante = (from u in _dbContext.Usuarios
                              join e in _dbContext.Estudiantes on u.Id equals e.Idusuario
                              where e.Registro == Usuario && u.Contraseña == Contrasena
                              select e).FirstOrDefault();

            if (estudiante != null)
            {
                // // Almacenar el tipo de usuario en la sesión
                // HttpContext.Session.SetString("TipoUsuario", TipoUsuario.ToString());

                // // Puedes almacenar más información del estudiante en la sesión si es necesario
                // HttpContext.Session.SetString("EstudianteId", estudiante.Id.ToString());

                // // Redireccionar a la página deseada (por ejemplo, la página del estudiante)
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

        // Almacenar el tipo de usuario en la sesión
        //HttpContext.Session.SetString("TipoUsuario", TipoUsuario.ToString());

        // Redireccionar a la página deseada
        return Page();
    }

}
