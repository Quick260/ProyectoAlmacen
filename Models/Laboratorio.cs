using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Laboratorio
{
    public long Id { get; set; }

    public string? NombreLaboratorio { get; set; }

    public string CodigoLaboratorio { get; set; } = null!;

    public virtual ICollection<Profesore> Profesores { get; set; } = new List<Profesore>();

    public virtual ICollection<Solicitude> Solicitudes { get; set; } = new List<Solicitude>();
}
