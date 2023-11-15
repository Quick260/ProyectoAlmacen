using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class DatosMateriale
{
    public long Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Materiale> Materiales { get; set; } = new List<Materiale>();
}
