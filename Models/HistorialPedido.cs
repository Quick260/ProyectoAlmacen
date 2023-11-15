using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class HistorialPedido
{
    public long Id { get; set; }

    public long Idsolicitud { get; set; }

    public string? EstadoPedido { get; set; }

    public virtual TipoUsuario IdsolicitudNavigation { get; set; } = null!;
}
