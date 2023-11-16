using System.Text.RegularExpressions;
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
        if (!validacionesUsuario(NuevoUsuario))
        {
            return RedirectToPage();
        }

        _dbContext.Usuarios.Add(NuevoUsuario);
        _dbContext.SaveChanges();
        long idGenerado = NuevoUsuario.Id;


        if (NuevoUsuario.TipoUsuario == 1)
        {
            if (!validacionesUsuario(NuevoEstudiante))
            {
                return RedirectToPage();
            }
            NuevoEstudiante.Idusuario = idGenerado;
            _dbContext.Estudiantes.Add(NuevoEstudiante);
            _dbContext.SaveChanges();
            TempData["UserCreated"] = "El estudiante se ha creado correctamente.";
        }
        if (NuevoUsuario.TipoUsuario == 2)
        {
            TempData["UserCreated"] = "El almacenista se ha creado correctamente.";
        }
        if (NuevoUsuario.TipoUsuario == 3)
        {
            NuevoProfesor.Idusuario = idGenerado;
            NuevoProfesor.Nomina = Request.Form["NominaProfesor"];
            NuevoProfesor.MateriasImpartidas = Request.Form["MateriaProfesor"];
            NuevoProfesor.SalonesAsignados = long.TryParse(Request.Form["SalonProfesor"], out long SalonesAsignados) ? SalonesAsignados : (long?)null;
            _dbContext.Profesores.Add(NuevoProfesor);
            _dbContext.SaveChanges();
            TempData["UserCreated"] = "El profesor se ha creado correctamente.";
        }
        if (NuevoUsuario.TipoUsuario == 4)
        {
            NuevoCoordinador.Idusuario = idGenerado;
            NuevoCoordinador.NumeroIdentificacion = Request.Form["NumeroIdentificacionCoordinador"];
            _dbContext.Coordinadores.Add(NuevoCoordinador);
            _dbContext.SaveChanges();
            TempData["UserCreated"] = "El coordinador se ha creado correctamente.";
        }

        return RedirectToPage();
    }

    bool validacionesUsuario(Usuario usuario)
    {
        if (usuario == null)
        {
            TempData["Error"] = "El usuario no puede ser nulo.";
            return false;
        }

        // Verificar si el campo Nombre contiene solo letras
        if (!string.IsNullOrWhiteSpace(usuario.NombreCompleto))
        {
            Regex regex = new Regex("^[a-zA-Z ]+$");

            if (!regex.IsMatch(usuario.NombreCompleto))
            {
                TempData["Error"] = "El campo Nombre Completo solo puede contener letras y espacios.";
                return false;
            }
        }
        else
        {
            TempData["Error"] = "El campo Nombre Completo no puede estar vacío.";
            return false;
        }

        if (!string.IsNullOrWhiteSpace(usuario.Contraseña) && usuario.Contraseña.Length >= 8)
        {
            bool contieneMayuscula = usuario.Contraseña.Any(char.IsUpper);
            bool contieneNumero = usuario.Contraseña.Any(char.IsDigit);

            if (!contieneMayuscula || !contieneNumero)
            {
                TempData["Error"] = "La contraseña debe contener al menos una letra mayúscula y un número.";
                return false;
            }
        }
        else
        {
            TempData["Error"] = "La contraseña debe tener al menos 8 caracteres.";
            return false;
        }
        return true;
    }

    bool validacionesUsuario(Estudiante estudiante)
    {
        if (estudiante == null)
        {
            TempData["Error"] = "El estudiante no puede ser nulo.";
            return false;
        }

        if (!string.IsNullOrEmpty(estudiante.Registro) && estudiante.Registro.Length == 8 && int.TryParse(estudiante.Registro, out int registroNumerico))
        {
            int primerosDosDigitos = registroNumerico / 1000000;

            int añoActual = DateTime.Now.Year % 100;

            if (primerosDosDigitos > añoActual)
            {
                TempData["Error"] = "El campo Registro no es válido.";
                return false;
            }
        }
        else
        {
            TempData["Error"] = "El campo Registro debe tener 8 números.";
            return false;
        }

        return true;
    }
}