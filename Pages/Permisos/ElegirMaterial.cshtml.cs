using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoAlmacen.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class ElegirMaterialModel : PageModel
{
    private readonly TuDbContext _dbContext;

    public ElegirMaterialModel(TuDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public long SolicitudId { get; set; }

    public List<Inventario> MaterialesDisponibles { get; set; }
    public List<Inventario> MaterialesSeleccionados { get; set; } = new List<Inventario>();

    public void OnGet(long solicitudId)
    {
        SolicitudId = solicitudId;
        Console.WriteLine("SolicitudId: " + SolicitudId);

        MaterialesDisponibles = _dbContext.Inventarios.ToList();
    }
    public IActionResult OnPost(string agregarMaterial, List<int> cantidades)
    {
        if (MaterialesDisponibles == null)
        {
            MaterialesDisponibles = _dbContext.Inventarios.ToList();
        }

        if (!string.IsNullOrEmpty(agregarMaterial) && int.TryParse(agregarMaterial, out int materialId) && cantidades != null)
        {
            var materialSeleccionado = MaterialesSeleccionados.FirstOrDefault(m => m.IdMaterial == materialId);

            // Si el material ya está en la lista de seleccionados, solo aumentar la cantidad
            if (materialSeleccionado != null)
            {
                var materialDisponible = MaterialesDisponibles.FirstOrDefault(m => m.IdMaterial == materialId);
                
                if (materialDisponible != null)
                {
                    var cantidadExistente = materialSeleccionado.CantidadDisponible;
                    var nuevaCantidad = cantidadExistente + cantidades.Sum();

                    // Asegurarse de que hay suficiente cantidad disponible
                    if (nuevaCantidad <= materialDisponible.CantidadDisponible)
                    {
                        materialSeleccionado.CantidadDisponible = nuevaCantidad;

                        // Restar la cantidad seleccionada de los materiales disponibles
                        materialDisponible.CantidadDisponible -= cantidades.Sum();
                    }
                    else
                    {
                        // Manejar el caso en el que no hay suficientes materiales disponibles
                        // Puedes agregar aquí la lógica para mostrar un mensaje de error o realizar alguna acción específica.
                    }
                }
            }
            else
            {
                // Si no está en la lista, agregar el nuevo material
                var materialNuevo = MaterialesDisponibles.FirstOrDefault(m => m.IdMaterial == materialId);

                if (materialNuevo != null)
                {
                    // Asegurarse de que hay suficiente cantidad disponible
                    if (cantidades.Sum() <= materialNuevo.CantidadDisponible)
                    {
                        // Restar la cantidad seleccionada de los materiales disponibles
                        materialNuevo.CantidadDisponible -= cantidades.Sum();

                        // Agregar el material seleccionado a la lista de materiales seleccionados
                        var nuevoMaterialSeleccionado = new Inventario
                        {
                            IdMaterial = materialNuevo.IdMaterial,
                            MaterialNombre = materialNuevo.MaterialNombre,
                            MaterialDescripcion = materialNuevo.MaterialDescripcion,
                            Estado = materialNuevo.Estado,
                            CantidadDisponible = cantidades.Sum()
                        };

                        MaterialesSeleccionados.Add(nuevoMaterialSeleccionado);
                    }
                    else
                    {
                        // Manejar el caso en el que no hay suficientes materiales disponibles
                        // Puedes agregar aquí la lógica para mostrar un mensaje de error o realizar alguna acción específica.
                    }
                }
            }
        }

        // Volver a cargar la página con los datos actualizados
        OnGet(SolicitudId);

        return Page();
    }
}