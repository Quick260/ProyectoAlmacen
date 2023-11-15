using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Prestamo
{
    public long Id { get; set; }

    public long? Idsolicitud { get; set; }

    public string? FechaDevolucion { get; set; }

    public string? EstadoPrestamo { get; set; }

    public virtual Solicitude? IdsolicitudNavigation { get; set; }
}
