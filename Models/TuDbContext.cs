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

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<HistorialPedido> HistorialPedidos { get; set; }

    public virtual DbSet<Materiale> Materiales { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Profesore> Profesores { get; set; }

    public virtual DbSet<ReporteDanio> ReporteDanios { get; set; }

    public virtual DbSet<Solicitude> Solicitudes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=ProyectoAlmace.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BitacoraMantenimiento>(entity =>
        {
            entity.ToTable("BitacoraMantenimiento");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.BitacoraMantenimientos).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Coordinadore>(entity =>
        {
            entity.HasIndex(e => e.NumeroIdentificacion, "IX_Coordinadores_NumeroIdentificacion").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Coordinadores).HasForeignKey(d => d.Idusuario);
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasIndex(e => e.Registro, "IX_Estudiantes_Registro").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Estudiantes).HasForeignKey(d => d.Idusuario);
        });

        modelBuilder.Entity<HistorialPedido>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EstadoPedido).HasDefaultValueSql("'Entregado'");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.HistorialPedidos).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.HistorialPedidos).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Materiale>(entity =>
        {
            entity.HasKey(e => e.NumeroInventario);

            entity.Property(e => e.Estado).HasDefaultValueSql("'Disponible'");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EstadoPrestamo).HasDefaultValueSql("'Pendiente'");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Prestamos).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.Prestamos).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Profesore>(entity =>
        {
            entity.HasIndex(e => e.Nomina, "IX_Profesores_Nomina").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Profesores).HasForeignKey(d => d.Idusuario);
        });

        modelBuilder.Entity<ReporteDanio>(entity =>
        {
            entity.ToTable("ReporteDanio");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.ReporteDanios).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.ReporteDanios).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Solicitude>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EstadoSolicitud).HasDefaultValueSql("'Pendiente'");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Solicitudes).HasForeignKey(d => d.Idusuario);

            entity.HasOne(d => d.NumeroInventarioNavigation).WithMany(p => p.Solicitudes).HasForeignKey(d => d.NumeroInventario);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
