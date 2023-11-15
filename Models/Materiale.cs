using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Materiale
{
    public string NumeroInventario { get; set; } = null!;

    public long? DatosMateriales { get; set; }

    public long? AnioMaterial { get; set; }

    public string? Estado { get; set; }
    public string? NumeroSerie { get; set; }

    public virtual ICollection<BitacoraMantenimiento> BitacoraMantenimientos { get; set; } = new List<BitacoraMantenimiento>();

    public virtual DatosMateriale? DatosMaterialesNavigation { get; set; }

    public virtual ICollection<MaterialSolicitud> MaterialSolicituds { get; set; } = new List<MaterialSolicitud>();

    public virtual ICollection<ReporteDanio> ReporteDanios { get; set; } = new List<ReporteDanio>();
}
