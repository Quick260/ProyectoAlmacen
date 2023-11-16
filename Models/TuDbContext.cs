using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAlmacen.Models;

public partial class TuDbContext : DbContext
{
    public TuDbContext()
    {
    }

    public TuDbContext(DbContextOptions<TuDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BitacoraMantenimiento> BitacoraMantenimientos { get; set; }

    public virtual DbSet<Coordinadore> Coordinadores { get; set; }

    public virtual DbSet<DatosMateriale> DatosMateriales { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<HistorialPedido> HistorialPedidos { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Laboratorio> Laboratorios { get; set; }

    public virtual DbSet<MaterialSolicitud> MaterialSolicituds { get; set; }

    public virtual DbSet<Materiale> Materiales { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Profesore> Profesores { get; set; }

    public virtual DbSet<ReporteDanio> ReporteDanios { get; set; }

    public virtual DbSet<Solicitude> Solicitudes { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VistaMaterialesSolicitud> VistaMaterialesSolicituds { get; set; }

    public virtual DbSet<VistaSolicitude> VistaSolicitudes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=ProyectoAlmace.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BitacoraMantenimiento>(entity =>
        {
            entity.ToTable("BitacoraMantenimiento");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.BitacoraMantenimientos).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Coordinadore>(entity =>
        {
            entity.HasIndex(e => e.NumeroIdentificacion, "IX_Coordinadores_NumeroIdentificacion").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Coordinadores).HasForeignKey(d => d.Idusuario);
        });

        modelBuilder.Entity<DatosMateriale>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_DatosMateriales_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasIndex(e => e.Registro, "IX_Estudiantes_Registro").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Estudiantes).HasForeignKey(d => d.Idusuario);
        });

        modelBuilder.Entity<HistorialPedido>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EstadoPedido).HasDefaultValueSql("'Entregado'");
            entity.Property(e => e.Idsolicitud).HasColumnName("IDSolicitud");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.HistorialPedidos)
                .HasForeignKey(d => d.Idsolicitud)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("inventario");
        });

        modelBuilder.Entity<Laboratorio>(entity =>
        {
            entity.HasIndex(e => e.CodigoLaboratorio, "IX_Laboratorios_CodigoLaboratorio").IsUnique();

            entity.HasIndex(e => e.Id, "IX_Laboratorios_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<MaterialSolicitud>(entity =>
        {
            entity.ToTable("MaterialSolicitud");

            entity.HasIndex(e => e.Id, "IX_MaterialSolicitud_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idsolicitud).HasColumnName("IDSolicitud");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.MaterialSolicituds)
                .HasForeignKey(d => d.Idsolicitud)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.MaterialSolicituds).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Materiale>(entity =>
        {
            entity.HasKey(e => e.NumeroInventario);

            entity.Property(e => e.Estado).HasDefaultValueSql("'Disponible'");

            entity.HasOne(d => d.DatosMaterialesNavigation).WithMany(p => p.Materiales)
                .HasForeignKey(d => d.DatosMateriales)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EstadoPrestamo).HasDefaultValueSql("'Pendiente'");
            entity.Property(e => e.Idsolicitud).HasColumnName("IDSolicitud");

            entity.HasOne(d => d.IdsolicitudNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.Idsolicitud)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Profesore>(entity =>
        {
            entity.HasIndex(e => e.Nomina, "IX_Profesores_Nomina").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Profesores).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.SalonesAsignadosNavigation).WithMany(p => p.Profesores)
                .HasForeignKey(d => d.SalonesAsignados)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ReporteDanio>(entity =>
        {
            entity.ToTable("ReporteDanio");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.ReporteDanios).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.ReporteDanios).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Solicitude>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EstadoSolicitud).HasDefaultValueSql("'Pendiente'");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(datetime('now', 'localtime'))");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Solicitudes).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.LaboratorioNavigation).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.Laboratorio)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.ProfesorNavigation).WithMany(p => p.Solicitudes)
                .HasForeignKey(d => d.Profesor)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.ToTable("TipoUsuario");

            entity.HasIndex(e => e.Id, "IX_TipoUsuario_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TipoUsuario1).HasColumnName("TipoUsuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.TipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoUsuario)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<VistaMaterialesSolicitud>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaMaterialesSolicitud");

            entity.Property(e => e.MaterialSolicitudId).HasColumnName("MaterialSolicitudID");
            entity.Property(e => e.SolicitudId).HasColumnName("SolicitudID");
        });

        modelBuilder.Entity<VistaSolicitude>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaSolicitudes");

            entity.Property(e => e.SolicitudId).HasColumnName("SolicitudID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
