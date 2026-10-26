using Microsoft.EntityFrameworkCore;
using PortalElectoral.Models;
using System.Reflection.Emit;
namespace PortalElectoral.Data;

public class ElectoralDbContext : DbContext
{
    public ElectoralDbContext(
    DbContextOptions<ElectoralDbContext> options): base(options)
    {
    }
    public DbSet<Eleccion> Elecciones => Set<Eleccion>();
    public DbSet<Provincia> Provincias => Set<Provincia>();
    public DbSet<PartidoPolitico> Partidos => Set<PartidoPolitico>();
    public DbSet<Candidatura> Candidaturas => Set<Candidatura>();
    public DbSet<Candidato> Candidatos => Set<Candidato>();
    public DbSet<PropuestaElectoral> PropuestasElectorales
    => Set<PropuestaElectoral>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Eleccion>(entity =>
        {
            entity.ToTable("elecciones");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
            .HasMaxLength(120)
            .IsRequired();
            entity.Property(e => e.Estado)
            .HasMaxLength(30)
            .IsRequired();
        });
        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.ToTable("provincias");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre)
            .HasMaxLength(100)
            .IsRequired();
            entity.Property(p => p.ComunidadAutonoma)
            .HasMaxLength(100)
            .IsRequired();
            entity.HasIndex(p => p.Nombre)
            .IsUnique();
        });
        modelBuilder.Entity<PartidoPolitico>(entity =>
        {
            entity.ToTable("partidos_politicos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre)
            .HasMaxLength(120)
            .IsRequired();
            entity.Property(p => p.Siglas)
            .HasMaxLength(15)
            .IsRequired();
            entity.Property(p => p.Descripcion)
            .HasMaxLength(500)
            .IsRequired();
            entity.HasIndex(p => p.Siglas)
            .IsUnique();
        });
        modelBuilder.Entity<Candidatura>(entity =>
        {
            entity.ToTable("candidaturas");
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new
            {
                c.EleccionId,
                c.ProvinciaId,
                c.PartidoPoliticoId
            }).IsUnique();
            entity.HasOne(c => c.Eleccion)
            .WithMany(e => e.Candidaturas)
            .HasForeignKey(c => c.EleccionId)
            .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.Provincia)
            .WithMany(p => p.Candidaturas)
            .HasForeignKey(c => c.ProvinciaId)
            .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.PartidoPolitico)
            .WithMany(p => p.Candidaturas)
            .HasForeignKey(c => c.PartidoPoliticoId)
            .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Candidato>(entity =>
        {
            entity.ToTable("candidatos");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nombre)
            .HasMaxLength(100)
            .IsRequired();
            entity.Property(c => c.Apellidos)
            .HasMaxLength(150)
            .IsRequired();
            entity.HasIndex(c => new
            {
                c.CandidaturaId,
                c.OrdenLista
            }).IsUnique();
            entity.HasOne(c => c.Candidatura)
            .WithMany(ca => ca.Candidatos)
            .HasForeignKey(c => c.CandidaturaId)
            .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<PropuestaElectoral>(entity =>
        {
            entity.ToTable("propuestas_electorales");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Titulo)
            .HasMaxLength(150)
            .IsRequired();
            entity.Property(p => p.Area)
            .HasMaxLength(80)
            .IsRequired();
            entity.Property(p => p.Descripcion)
            .HasMaxLength(1000)
            .IsRequired();
            entity.HasOne(p => p.PartidoPolitico)
            .WithMany(pa => pa.Propuestas)
            .HasForeignKey(p => p.PartidoPoliticoId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
