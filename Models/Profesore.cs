using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Profesore
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string Nomina { get; set; } = null!;

    public string? MateriasImpartidas { get; set; }

    public string? SalonesAsignados { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }
}
