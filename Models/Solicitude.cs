using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Solicitude
{
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public long? Laboratorio { get; set; }

    public long? Profesor { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaSolicitud { get; set; }

    public string? HoraSolicitud { get; set; }

    public string? HoraRetorno { get; set; }

    public string? EstadoSolicitud { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual Laboratorio? LaboratorioNavigation { get; set; }

    public virtual ICollection<MaterialSolicitud> MaterialSolicituds { get; set; } = new List<MaterialSolicitud>();

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public virtual Profesore? ProfesorNavigation { get; set; }
}
