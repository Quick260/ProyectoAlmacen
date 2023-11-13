using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Estudiante
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string Registro { get; set; } = null!;

    public string? Grupo { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }
}
