using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

public class GenerarPermisoModel : PageModel{
    private readonly TuDbContext _dbContext;

    public GenerarPermisoModel(TuDbContext dbContext){
        _dbContext = dbContext;
    }

    
    public void OnGet(){
        // Método ejecutado al cargar la página
    }
}