using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoAlmacen.Models;

public partial class BitacoraMantenimiento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string? NumeroInventario { get; set; }

    public string? TipoMantenimiento { get; set; }

    public string? DescripcionMantenimiento { get; set; }

    public string? RefaccionUtilizada { get; set; }

    public string? FechaMantenimiento { get; set; }

    public string? FechaProgramada { get; set; }

    public virtual Materiale? NumeroInventarioNavigation { get; set; }
}
