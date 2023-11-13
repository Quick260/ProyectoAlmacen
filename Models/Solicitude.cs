using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Solicitude
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string? NumeroInventario { get; set; }

    public long? Cantidad { get; set; }

    public string? FechaSolicitud { get; set; }

    public string? HoraSolicitud { get; set; }

    public string? EstadoSolicitud { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual Materiale? NumeroInventarioNavigation { get; set; }
}
