
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;

namespace ProyectoAlmacen.Pages.Almacenista
{
    public class OpcionesMaterialesModel : PageModel
    {
        private readonly TuDbContext _dbContext;

        public void OnGet()
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Response.Redirect("/Error");
            }

            var consulta = from material in _dbContext.Materiales
                           join datosMaterial in _dbContext.DatosMateriales on material.DatosMateriales equals datosMaterial.Id
                           select new
                           {
                               material.NumeroInventario,
                               material.AnioMaterial,
                               material.Estado,
                               datosMaterial.Nombre // Agrega el campo Nombre de la tabla DatosMateriales
                           };
            var resultado = consulta.ToList();

            
        }

        public Materiale NuevoMaterial { get; set; }
    }
}