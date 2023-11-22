using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public List<MaterialViewModel> Materiales { get; set; }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Response.Redirect("/Error");
            }

            var consulta = from material in _dbContext.Materiales
                           join datosMaterial in _dbContext.DatosMateriales on material.DatosMateriales equals datosMaterial.Id
                           select new MaterialViewModel
                           {
                               NumeroInventario = material.NumeroInventario,
                               Anio = (int)material.AnioMaterial,
                               Estado = material.Estado,
                               IdDatosMateriales = datosMaterial.Id,
                               Nombre = datosMaterial.Nombre,
                               Descripcion = datosMaterial.Descripcion
                           };

            Materiales = consulta.ToList();
        }

        public class MaterialViewModel
        {
            public string NumeroInventario { get; set; }
            public int Anio { get; set; }
            public string Estado { get; set; }
            public long IdDatosMateriales { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
    }
}
