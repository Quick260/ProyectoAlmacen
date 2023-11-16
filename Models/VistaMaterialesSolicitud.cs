using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class VistaMaterialesSolicitud
{
    public long? MaterialSolicitudId { get; set; }

    public long? SolicitudId { get; set; }

    public string? NombreMaterial { get; set; }

    public string? DescripcionMaterial { get; set; }

    public string? NumeroInventarioMaterial { get; set; }

    public long? Cantidad { get; set; }
}
