using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class Materiale
{
    public string NumeroInventario { get; set; } = null!;

    public string NombreMaterial { get; set; } = null!;

    public string? Descripcion { get; set; }

    public long? AnioMaterial { get; set; }

    public string? Estado { get; set; }

    public long? Cantidad { get; set; }

    public virtual ICollection<BitacoraMantenimiento> BitacoraMantenimientos { get; set; } = new List<BitacoraMantenimiento>();

    public virtual ICollection<HistorialPedido> HistorialPedidos { get; set; } = new List<HistorialPedido>();

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public virtual ICollection<ReporteDanio> ReporteDanios { get; set; } = new List<ReporteDanio>();

    public virtual ICollection<Solicitude> Solicitudes { get; set; } = new List<Solicitude>();
}
