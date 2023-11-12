﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoAlmacen.Models;

public partial class HistorialPedido
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long? Idusuario { get; set; }

    public string? NumeroInventario { get; set; }

    public long? Cantidad { get; set; }

    public string? FechaPedido { get; set; }

    public string? EstadoPedido { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual Materiale? NumeroInventarioNavigation { get; set; }
}
