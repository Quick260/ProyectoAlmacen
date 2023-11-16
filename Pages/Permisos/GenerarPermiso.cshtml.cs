using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoAlmacen.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class GenerarPermisoModel : PageModel
{
    private readonly TuDbContext _dbContext;

    public GenerarPermisoModel(TuDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Estudiante Estudiante { get; set; }

    public Usuario? Usuario { get; set; }

    public List<Laboratorio> Laboratorios { get; set; }

    public List<Profesore> Profesores { get; set; }
    
    [BindProperty]
    public Solicitude NuevaSolicitud { get; set; }

    public void OnGet()
    {
        // Método ejecutado al cargar la página
        // Obtener los datos del usuario actual o de la sesión y asignarlos a las propiedades
        // Por ejemplo, puedes obtener el ID de usuario de la sesión y luego buscar el usuario en la base de datos
        int idUsuario = ObtenerIdUsuarioDesdeSesion(); // Reemplaza esto con tu lógica de obtención de ID de usuario
        Usuario = _dbContext.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

        if(Usuario != null){        
            Estudiante = _dbContext.Estudiantes.FirstOrDefault(e => e.Idusuario == Usuario.Id)!;
        }

        // Obtener los laboratorios y profesores de la base de datos y asignarlos a las propiedades
        Laboratorios = _dbContext.Laboratorios.ToList();
        Profesores = _dbContext.Profesores.ToList();
        foreach (var profesor in Profesores)
        {
            profesor.IdusuarioNavigation = _dbContext.Usuarios.FirstOrDefault(u => u.Id == profesor.Idusuario);
        }
    }

    // Método para manejar la solicitud POST al enviar el formulario
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelStateValue in ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            
            return RedirectToPage("/Error");
        }

        // Procesar los datos del formulario y guardar en la base de datos según sea necesario
        var idUsuario = ObtenerIdUsuarioDesdeSesion();

        NuevaSolicitud = new Solicitude
        {
            Idusuario = idUsuario,
            Laboratorio = long.TryParse(Request.Form["Laboratorio"], out long Laboratorio) ? Laboratorio : (long?)null,
            Profesor = long.TryParse(Request.Form["Profesor"], out long Profesor) ? Profesor : (long?)null,
            FechaSolicitud = Request.Form["Fecha"],
            HoraSolicitud = Request.Form["HoraSalida"],
            HoraRetorno = Request.Form["HoraRegreso"]
        };

        try
        {
            _dbContext.Solicitudes.Add(NuevaSolicitud);
            _dbContext.SaveChanges();
            
            // Obtén el ID de la solicitud recién creada
            var nuevaSolicitudId = NuevaSolicitud.Id;

            // Redirige a la página ElegirMaterial con el ID de la solicitud como parámetro
            return RedirectToPage("/Permisos/ElegirMaterial", new { solicitudId = nuevaSolicitudId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToPage("/Error");
            // Manejo de errores según tus necesidades
        }
 // Redirigir a la página principal u otra página después de procesar el formulario
    }
    // Método de ejemplo para obtener el ID de usuario desde la sesión
    private int ObtenerIdUsuarioDesdeSesion()
    {
        // Reemplaza esto con tu lógica real para obtener el ID de usuario desde la sesión
        // Por ejemplo, si estás utilizando ASP.NET Core Identity, puedes acceder al ID de usuario así: 
        // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // y luego convertirlo a int según sea necesario.
        return 54; // Solo un valor de ejemplo, reemplázalo con tu lógica real
    }
}
