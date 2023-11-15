using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Usuario
{
    public long Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public long TipoUsuario { get; set; }

    public virtual ICollection<Coordinadore> Coordinadores { get; set; } = new List<Coordinadore>();

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<Profesore> Profesores { get; set; } = new List<Profesore>();

    public virtual ICollection<ReporteDanio> ReporteDanios { get; set; } = new List<ReporteDanio>();

    public virtual ICollection<Solicitude> Solicitudes { get; set; } = new List<Solicitude>();

    public virtual TipoUsuario TipoUsuarioNavigation { get; set; } = null!;
}
