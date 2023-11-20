using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;
using static ProyectoAlmacen.Pages.Almacenista.OpcionesMaterialesModel;

namespace ProyectoAlmacen.Pages.Almacenista
{
    public class AgregarMaterialModel : PageModel
    {
        private readonly TuDbContext _dbContext;
        [BindProperty]
        public Materiale materialAgregar { get; set; }
        [BindProperty]
        public DatosMateriale datoMaterialAgregar { get; set; }

        public AgregarMaterialModel(TuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public MaterialViewModel NuevoMaterial { get; set; }
        public List<DatosMateriale> NombresExistentes { get; set; }

        public IActionResult OnGet()
        {
            /*if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Response.Redirect("/Error");
            }*/
            
            NombresExistentes = _dbContext.DatosMateriales.ToList();
            return Page();
        }
        public IActionResult OnPost()
        {
            if (NuevoMaterial.IdDatosMateriales == -1)
            {
                // Crear un nuevo DatosMateriale
                var nuevoDatoMaterial = new DatosMateriale
                {
                    Nombre = NuevoMaterial.Nombre,
                    Descripcion = NuevoMaterial.Descripcion
                };

                // Agregar el nuevo DatosMateriale a la base de datos
                _dbContext.DatosMateriales.Add(nuevoDatoMaterial);
                _dbContext.SaveChanges();

                // Asignar el Id generado al NuevoMaterial
                NuevoMaterial.IdDatosMateriales = nuevoDatoMaterial.Id;
            }

            // Crear un nuevo Materiale y asignarle los valores
            var nuevoMaterial = new Materiale
            {
                NumeroInventario = NuevoMaterial.NumeroInventario,
                DatosMateriales = NuevoMaterial.IdDatosMateriales,
                AnioMaterial = NuevoMaterial.Anio,
                Estado = NuevoMaterial.Estado
            };

            // Agregar el nuevo Materiale a la base de datos
            _dbContext.Materiales.Add(nuevoMaterial);
            _dbContext.SaveChanges();

            // Redirige a la página de lista de materiales después de agregar
            return RedirectToPage("/Almacenista/OpcionesMateriales");
        }
    }
}
