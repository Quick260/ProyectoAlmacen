using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoAlmacen.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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


    TempData["Error"] = null;
    // Obtener la fecha actual
    var fechaActual = DateTime.Now;

    // Validar que la fecha sea mínimo un día después de la solicitud de materiales
    var fechaSolicitud = DateTime.ParseExact(Request.Form["Fecha"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
    if ((fechaSolicitud - fechaActual).Days <= 0)
    {     
        TempData["Error"] = "La fecha debe ser mínimo un día después de la solicitud";
        ModelState.AddModelError("Fecha", "La fecha debe ser mínimo un día después de la solicitud");
        Console.WriteLine("La fecha debe ser mínimo un día después de la solicitud");
        Console.WriteLine("FechaSolicitud: " + fechaSolicitud);
    }
    
    // Validar que las horas registradas estén dentro de la jornada estudiantil (entre 7:00 y 14:30)

    TimeSpan.TryParse(Request.Form["HoraSalida"], out var horaSalida);
    TimeSpan.TryParse(Request.Form["HoraRegreso"], out var horaRegreso);

    Console.WriteLine("HoraSalida: " + horaSalida);
    Console.WriteLine("HoraRegreso: " + horaRegreso);

    if (horaSalida < TimeSpan.Parse("07:00") ||
        horaRegreso > TimeSpan.Parse("14:30"))
    {
        TempData["Error"] = "Las horas registradas deben estar dentro de la jornada estudiantil (entre 7:00 y 14:30).";
        ModelState.AddModelError("NuevaSolicitud.HoraSalida", "Las horas registradas deben estar dentro de la jornada estudiantil (entre 7:00 y 14:30).");
        ModelState.AddModelError("NuevaSolicitud.HoraRegreso", "Las horas registradas deben estar dentro de la jornada estudiantil (entre 7:00 y 14:30).");
        Console.WriteLine("Las horas registradas deben estar dentro de la jornada estudiantil (entre 7:00 y 14:30).");
        Console.WriteLine("HoraSalida: " + horaSalida);
        Console.WriteLine("HoraRegreso: " + horaRegreso);
    }

    // Validar que la hora de salida sea antes que la hora de entrada
    if (horaSalida >= horaRegreso)
    {
        TempData["Error"] = "La hora de salida debe ser antes que la hora de regreso.";
        ModelState.AddModelError("NuevaSolicitud.HoraSalida", "La hora de salida debe ser antes que la hora de regreso.");
        ModelState.AddModelError("NuevaSolicitud.HoraRegreso", "La hora de salida debe ser antes que la hora de regreso.");
        Console.WriteLine("La hora de salida debe ser antes que la hora de regreso.");
    }

    // Validar que el tiempo de préstamo mínimo sea de 50 minutos
    if ((horaRegreso - horaSalida).TotalMinutes < 50)
    {
        TempData["Error"] = "El tiempo de préstamo mínimo debe ser de 50 minutos.";
        ModelState.AddModelError("NuevaSolicitud.HoraRegreso", "El tiempo de préstamo mínimo debe ser de 50 minutos.");
        Console.WriteLine("El tiempo de préstamo mínimo debe ser de 50 minutos.");
    }
    
        if (!ModelState.IsValid)
        {
            return RedirectToPage("/Permisos/GenerarPermiso");
            
        }else{
            TempData["Error"] = null;
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
        
            int? id = HttpContext.Session.GetInt32("UsuarioId");
            Console.WriteLine("UsuarioId: " + id);
            return id ?? 0;
    }
}
