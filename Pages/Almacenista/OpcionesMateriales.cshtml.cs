using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoAlmacen.Models;
using System.Collections.Generic;

namespace ProyectoAlmacen.Pages.Almacenista
{
    public class OpcionesMaterialesModel : PageModel
    {
        private readonly TuDbContext _dbContext;
        public OpcionesMaterialesModel(TuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Materiale> Materiales { get; set; } = new List<Materiale>();
        public Materiale Material { get; set; } = new Materiale();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Response.Redirect("/Error");
            }

            Materiales = _dbContext.Materiales.Include(m => m.DatosMaterialesNavigation).ToList();
        }

        public IActionResult OnPostEditarMaterial(long materialId){
            return RedirectToPage("/Almacenista/AgregarMaterial", new { id = materialId});
        }

        public IActionResult OnPostEliminarMaterial(string materialId){
            Console.WriteLine("MaterialId: " + materialId);
            var material = _dbContext.Materiales.FirstOrDefault(m => m.NumeroInventario == materialId);
            _dbContext.Materiales.Remove(material);
            _dbContext.SaveChanges();
            return RedirectToPage("/Almacenista/OpcionesMateriales");
        }
    }
}
