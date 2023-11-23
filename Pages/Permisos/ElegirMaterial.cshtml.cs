using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProyectoAlmacen.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class ElegirMaterialModel : PageModel
{
    private readonly TuDbContext _dbContext;

    public ElegirMaterialModel(TuDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public long SolicitudId { get; set; }

    public List<Inventario> MaterialesDisponibles { get; set; }
    public List<Inventario> MaterialesSeleccionados { get; set; }
    int[]? ListaRecuperada { get; set;}

    public void OnGet(long solicitudId)
    {
        SolicitudId = solicitudId;
        TempData["SolicitudId"] = solicitudId;
        RecuperarTemp();
        Console.WriteLine("SolicitudId: " + TempData["SolicitudId"]);
    }

  
    public IActionResult OnPostAddToSelected(long materialId, long solicitudId)
    {
        Console.WriteLine("OnPostAddToSelected: " + materialId);
        Console.WriteLine("SolicitudId: " + solicitudId);

        RecuperarTemp();
        // Recupera los materiales desde la base de datos
        var material = MaterialesDisponibles.FirstOrDefault(m => m.IdMaterial == materialId);

        if (material != null && material.CantidadDisponible > 0)
        {
            Console.WriteLine("Material: " + material.MaterialNombre);
            if (!ListaRecuperada.Contains((int)material.IdMaterial))
            {
                ListaRecuperada[material.IdMaterial - 1] = (int)material.IdMaterial;
            }
        }
        else
        {
            Console.WriteLine("Existe");
        }
        

        // Guarda los materiales seleccionados en TempData
        TempData["Seleccionado"] = ListaRecuperada;      


        return RedirectToPage("/Permisos/ElegirMaterial", new { solicitudId });
    }


    public IActionResult OnPostRemoveFromSelected(long materialId, long solicitudId)
    {
        Console.WriteLine("OnPostRemoveFromSelected: " + materialId);
        Console.WriteLine("SolicitudId: " + solicitudId);

        RecuperarTemp();
        // Recupera los materiales desde la base de datos
        var material = MaterialesSeleccionados.FirstOrDefault(m => m.IdMaterial == materialId);

        if (material != null)
        {
            Console.WriteLine("Material: " + material.MaterialNombre);
            if (ListaRecuperada.Contains((int)material.IdMaterial))
            {
                ListaRecuperada[material.IdMaterial - 1] = 0;
            }
        }
        else
        {
            Console.WriteLine("Existe");
        }
        

        // Guarda los materiales seleccionados en TempData
        TempData["Seleccionado"] = ListaRecuperada;      


        return RedirectToPage("/Permisos/ElegirMaterial", new { solicitudId });
    }




    void RecuperarTemp()
    {
    try{
        
        ListaRecuperada = TempData["Seleccionado"] as int[];
        if(ListaRecuperada != null){
            foreach (var item in ListaRecuperada)
            {
                Console.WriteLine("ListaRecuperada: " + item);
            }
            MaterialesSeleccionados = new List<Inventario>();
            MaterialesDisponibles = RecuperarMateriales();
            foreach (var item in ListaRecuperada)
            {
                var material = MaterialesDisponibles.FirstOrDefault(m => m.IdMaterial == item);
                if (material != null)
                {
                    MaterialesSeleccionados.Add(material);
                    MaterialesDisponibles.Remove(material);
                }
            }
        }
        else
        {
            Console.WriteLine("ListaRecuperada: null");
            MaterialesSeleccionados = new List<Inventario>();
            MaterialesDisponibles = RecuperarMateriales();  
            ListaRecuperada = new int[MaterialesDisponibles.ToList().Max(m => m.IdMaterial)];
        }

        TempData["Seleccionado"] = ListaRecuperada;
    }catch(Exception e){
        Console.WriteLine("Error " + e.Message);
    }
    }
    

    public IActionResult OnPostNext(long solicitudId)
    {
        Console.WriteLine("OnPost");
        // Redirige a otra página después de guardar (ajusta la ruta según tus necesidades)
        List<MaterialSolicitud> materiales = new List<MaterialSolicitud>();
        RecuperarTemp();
        foreach(var item in ListaRecuperada)
        {
            if(item != 0)
            {
                var material = _dbContext.Materiales.FirstOrDefault(m => m.DatosMateriales == item && m.Estado == "Disponible");
                if(material != null)
                {
                    materiales.Add(new MaterialSolicitud{
                        NumeroInventario = material.NumeroInventario,
                        Idsolicitud = SolicitudId,
                        Cantidad = 1            
                    });
                    Console.WriteLine("Material: " + material.NumeroInventario);
                    _dbContext.Materiales.Update(material); 
                    _dbContext.SaveChanges();
                }

            }
        }
        _dbContext.MaterialSolicituds.AddRange(materiales);
        _dbContext.SaveChanges();
        TempData["Seleccionado"] = null;
        
        return RedirectToPage("/Permisos/HistorialPermisos");
    }

    List<Inventario> RecuperarMateriales()
    {
        
        Solicitude? solicitude = _dbContext.Solicitudes.FirstOrDefault(s => s.Id == SolicitudId);
        try{
            var parametros = new[] {
                new SqliteParameter("@fechaSolicitud",solicitude.FechaSolicitud),
                new SqliteParameter("@horaSolicitud", solicitude.HoraSolicitud),
                new SqliteParameter("@horaRetorno", solicitude.HoraRetorno)
            };
            Console.WriteLine("FechaSolicitud: " + solicitude.FechaSolicitud);
            Console.WriteLine("HoraSolicitud: " + solicitude.HoraSolicitud);
            Console.WriteLine("HoraRetorno: " + solicitude.HoraRetorno);
        
        return _dbContext.Inventarios.FromSqlRaw(
            @"SELECT 
                DM.ID AS IdMaterial,
                DM.Nombre AS MaterialNombre,
                DM.Descripcion AS MaterialDescripcion,
                M.NumeroInventario AS NumeroInventarioMaterial,
                M.Estado,
            COUNT(M.NumeroInventario) AS CantidadDisponible
            FROM 
                DatosMateriales DM
            JOIN 
                Materiales M ON DM.ID = M.DatosMateriales
            WHERE 
                (M.Estado = 'Disponible' OR M.Estado = 'No disponible')
                AND NOT EXISTS (
                    SELECT 1
                    FROM Solicitudes S
                    JOIN MaterialSolicitud MS ON S.ID = MS.IDSolicitud
                    WHERE 
                        M.NumeroInventario = MS.NumeroInventario
                        AND (
                                S.FechaSolicitud = @fechaSolicitud
                                OR (
                                    S.FechaSolicitud = @fechaSolicitud
                                    AND (
                                        (
                                            S.HoraSolicitud <= @horaSolicitud
                                            AND S.HoraRetorno >= @horaRetorno
                                        )
                                        OR (
                                            S.HoraSolicitud >= @horaSolicitud
                                            AND S.HoraSolicitud <= @horaRetorno
                                        )
                                    )
                                )
                            )
                )
            GROUP BY M.DatosMateriales, M.Estado;
        ", parametros).ToList();
        }catch(Exception e){
            Console.WriteLine("Error " + e.Message);
            return new List<Inventario>();
        }
    }
}


