using System;
using System.Collections.Generic;

namespace ProyectoAlmacen.Models;

public partial class VistaSolicitude
{
    public long? SolicitudId { get; set; }

    public string? NombreUsuario { get; set; }

    public string? NombreLaboratorio { get; set; }

    public string? NominaProfesor { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaSolicitud { get; set; }

    public string? HoraSolicitud { get; set; }

    public string? HoraRetorno { get; set; }

    public string? EstadoSolicitud { get; set; }
}
