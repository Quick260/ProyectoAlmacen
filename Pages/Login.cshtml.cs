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
                
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

        return Page();
    }

}
