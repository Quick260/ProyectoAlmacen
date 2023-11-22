using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Inventario
{
    public string? MaterialNombre { get; set; }

    public string? MaterialDescripcion { get; set; }

    public string? Estado { get; set; }

    public byte[]? CantidadDisponible { get; set; }
}
