using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Inventario
{
    public long IdMaterial { get; set; }
    public string? MaterialNombre { get; set; }

    public string? MaterialDescripcion { get; set; }

    public string? Estado { get; set; }

    public long? CantidadDisponible { get; set; }
}
