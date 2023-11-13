using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class ReporteDanio
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string? NumeroInventario { get; set; }

    public string? FechaReporte { get; set; }

    public string? HoraReporte { get; set; }

    public string? DescripcionDanio { get; set; }

    public string? TipoReporte { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual Materiale? NumeroInventarioNavigation { get; set; }
}
