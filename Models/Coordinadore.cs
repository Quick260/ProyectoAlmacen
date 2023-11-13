using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Coordinadore
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string NumeroIdentificacion { get; set; } = null!;

    public virtual Usuario? IdusuarioNavigation { get; set; }
}
