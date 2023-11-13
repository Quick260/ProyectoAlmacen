using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Prestamo
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string? NumeroInventario { get; set; }

    public string? FechaPrestamo { get; set; }

    public string? FechaDevolucion { get; set; }

    public string? EstadoPrestamo { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual Materiale? NumeroInventarioNavigation { get; set; }
}
