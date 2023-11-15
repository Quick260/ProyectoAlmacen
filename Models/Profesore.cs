using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Profesore
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string Nomina { get; set; } = null!;

    public string? MateriasImpartidas { get; set; }

    public long? SalonesAsignados { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual Laboratorio? SalonesAsignadosNavigation { get; set; }

    public virtual ICollection<Solicitude> Solicitudes { get; set; } = new List<Solicitude>();
}
