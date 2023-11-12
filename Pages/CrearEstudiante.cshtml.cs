using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

public class CrearEstudianteModel : PageModel
{
    public long? NuevoUsuarioId { get; set; }
    private readonly TuDbContext tuDbContext;

    [BindProperty]
    public Estudiante NuevoEstudiante { get; set; }

    public CrearEstudianteModel(TuDbContext dbContext)
    {
        tuDbContext = dbContext;
    }

    public void OnGet()
    {
        // Método ejecutado al cargar la página
        if(TempData["NuevoUsuarioId"] is int nuevoUsuarioId){
            NuevoUsuarioId = nuevoUsuarioId;
        }
        Console.WriteLine($"Valor de NuevoUsuarioId: {NuevoUsuarioId}");
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using (var transaction = tuDbContext.Database.BeginTransaction())
        {
            long ultimoId = tuDbContext.Estudiantes.Max(u => (long?)u.Id) ?? 0;
            NuevoEstudiante.Id = ultimoId + 1;

            NuevoEstudiante.Idusuario = NuevoUsuarioId;

            tuDbContext.Estudiantes.Add(NuevoEstudiante);
            tuDbContext.SaveChanges();

            transaction.Commit();

            return RedirectToPage("/Index"); // Puedes redirigir a donde desees
        }
    }
}
