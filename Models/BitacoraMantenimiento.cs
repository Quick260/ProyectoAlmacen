using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class BitacoraMantenimiento
{
    public long Id { get; set; }

    public string? NumeroInventario { get; set; }

    public string? TipoMantenimiento { get; set; }

    public string? DescripcionMantenimiento { get; set; }

    public string? RefaccionUtilizada { get; set; }

    public string? FechaMantenimiento { get; set; }

    public string? FechaProgramada { get; set; }

    public virtual Materiale? NumeroInventarioNavigation { get; set; }
}
