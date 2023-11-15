using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class TipoUsuario
{
    public long Id { get; set; }

    public string? TipoUsuario1 { get; set; }

    public virtual ICollection<HistorialPedido> HistorialPedidos { get; set; } = new List<HistorialPedido>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
