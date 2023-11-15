using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class MaterialSolicitud
{
    public long Id { get; set; }

    public long? Idsolicitud { get; set; }

    public string NumeroInventario { get; set; } = null!;

    public long Cantidad { get; set; }

    public virtual Solicitude? IdsolicitudNavigation { get; set; }

    public virtual Materiale NumeroInventarioNavigation { get; set; } = null!;
}
