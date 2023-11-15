using Microsoft.AspNetCore.Http;
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
                              select new { UsuarioId = u.Id, TipoUsuario = "Estudiante" }).FirstOrDefault();

            if (estudiante != null)
            {
                HttpContext.Session.SetInt32("UsuarioId", (int)estudiante.UsuarioId);
                HttpContext.Session.SetString("TipoUsuario", estudiante.TipoUsuario);
                Console.WriteLine(HttpContext.Session.GetInt32("UsuarioId"));
                Console.WriteLine(HttpContext.Session.GetString("TipoUsuario"));
                return RedirectToPage("/PruebaSesiones");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
        if (TipoUsuario == 2)
        {
            var almacenista = (from u in _dbContext.Usuarios
                               where u.NombreCompleto == Usuario && u.Contraseña == Contrasena && u.TipoUsuario == 2
                               select u).FirstOrDefault();
            if (almacenista != null)
            {
                HttpContext.Session.SetInt32("UsuarioId", (int)almacenista.Id);
                HttpContext.Session.SetString("TipoUsuario", "Almacenista");
                return RedirectToPage("/PruebaSesiones");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
        if (TipoUsuario == 3)
        {
            var administrador = (from u in _dbContext.Usuarios
                                 join p in _dbContext.Profesores on u.Id equals p.Idusuario
                                 where p.Nomina == Usuario && u.Contraseña == Contrasena
                                 select u).FirstOrDefault();
            if (administrador != null)
            {
                HttpContext.Session.SetInt32("UsuarioId", (int)administrador.Id);
                HttpContext.Session.SetString("TipoUsuario", "Administrador");
                return RedirectToPage("/PruebaSesiones");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }
        if (TipoUsuario == 4)
        {
            var coordinador = (from u in _dbContext.Usuarios
                               join c in _dbContext.Coordinadores on u.Id equals c.Idusuario
                               where c.NumeroIdentificacion == Usuario && u.Contraseña == Contrasena
                               select u).FirstOrDefault();
            if (coordinador != null)
            {
                HttpContext.Session.SetInt32("UsuarioId", (int)coordinador.Id);
                HttpContext.Session.SetString("TipoUsuario", "Coordinador");
                return RedirectToPage("/PruebaSesiones");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

        return Page();
    }

}
