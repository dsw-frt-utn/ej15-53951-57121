using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data;

public class DoctorsDbContext : DbContext
{
    public DoctorsDbContext(DbContextOptions<DoctorsDbContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Speciality> Specialities => Set<Speciality>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.ToTable("Specialities");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Description).IsRequired().HasMaxLength(250);

            entity.HasData(
                new Speciality("Cardiologia", "Especialidad encargada de las enfermedades del corazon", Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                new Speciality("Pediatria", "Atencion medica integral para bebes y ninos", Guid.Parse("7bc9ef12-1234-5678-abcd-ef1234567890")),
                new Speciality("Traumatologia", "Estudio y tratamiento de lesiones oseas", Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d")));
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctors");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(d => d.IsActive).IsRequired();

            entity.HasOne(d => d.Speciality)
                .WithMany()
                .HasForeignKey(d => d.SpecialityId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
