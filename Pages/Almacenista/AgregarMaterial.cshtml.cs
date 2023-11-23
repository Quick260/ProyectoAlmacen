using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public bool EsNuevo { get; set; } = true;

        public AgregarMaterialModel(TuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<DatosMateriale> NombresExistentes { get; set; }

        public IActionResult OnGet(long id)
        {
            /*if (HttpContext.Session.GetString("TipoUsuario") != "Almacenista")
            {
                Response.Redirect("/Error");
            }*/
            datoMaterialAgregar = new DatosMateriale();
            materialAgregar = new Materiale();

            var materialRecuperado = _dbContext.Materiales.Include(m => m.DatosMaterialesNavigation).FirstOrDefault(m => m.NumeroInventario == id.ToString());
            if (materialRecuperado != null)
            {
                datoMaterialAgregar.Id = long.Parse(materialRecuperado.NumeroInventario); // Asigna el Id existente si lo tienes
                datoMaterialAgregar.Nombre = materialRecuperado.DatosMaterialesNavigation?.Nombre;
                datoMaterialAgregar.Descripcion = materialRecuperado.DatosMaterialesNavigation?.Descripcion;
                materialAgregar.NumeroInventario = materialRecuperado.NumeroInventario;
                materialAgregar.AnioMaterial = materialRecuperado.AnioMaterial; // Asumiendo que hay una propiedad Anio en tu modelo
                materialAgregar.Estado = materialRecuperado.Estado;
                EsNuevo = false;
            }

            NombresExistentes = _dbContext.DatosMateriales.ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            EsNuevo = _dbContext.Materiales.Any(m => m.NumeroInventario == materialAgregar.NumeroInventario);
            if (!EsNuevo)
            {
                var nuevoMaterial = new Materiale();
                if (datoMaterialAgregar.Id == -1)
                {
                    var nuevoDatoMaterial = new DatosMateriale
                    {
                        Nombre = datoMaterialAgregar.Nombre,
                        Descripcion = datoMaterialAgregar.Descripcion,
                    };

                    _dbContext.DatosMateriales.Add(nuevoDatoMaterial);
                    _dbContext.SaveChanges();

                    Console.WriteLine("Nuevo dato material: " + nuevoDatoMaterial.Id);
                    materialAgregar.DatosMateriales = nuevoDatoMaterial.Id;

                    // Crear un nuevo Materiale y asignarle los valores
                    nuevoMaterial = new Materiale
                    {
                        NumeroInventario = materialAgregar.NumeroInventario,
                        DatosMateriales = materialAgregar.DatosMateriales,
                        AnioMaterial = materialAgregar.AnioMaterial,
                        Estado = materialAgregar.Estado,
                    };
                }
                else
                {
                    nuevoMaterial = new Materiale
                    {
                        NumeroInventario = materialAgregar.NumeroInventario,
                        DatosMateriales = datoMaterialAgregar.Id,
                        AnioMaterial = materialAgregar.AnioMaterial,
                        Estado = materialAgregar.Estado,
                    };
                }



                // Agregar el nuevo Materiale a la base de datos
                _dbContext.Materiales.Add(nuevoMaterial);
                _dbContext.SaveChanges();
            }
            else
            {
                var materialRecuperado = _dbContext.Materiales.FirstOrDefault(m => m.NumeroInventario == materialAgregar.NumeroInventario);
                if (materialRecuperado != null)
                {
                    Console.WriteLine("Material recuperado: " + materialRecuperado.NumeroInventario);
                    materialRecuperado.AnioMaterial = materialAgregar.AnioMaterial; // Asumiendo que hay una propiedad Anio en tu modelo
                    materialRecuperado.Estado = materialAgregar.Estado;

                    _dbContext.SaveChanges();
                }


            }


            // Redirige a la página de lista de materiales después de agregar
            return RedirectToPage("/Almacenista/OpcionesMateriales");
        }
    }
}
