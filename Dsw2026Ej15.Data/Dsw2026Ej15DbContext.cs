using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data;

    public class Dsw2026Ej15DbContext: DbContext
    {
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specials { get; set; }


        public Dsw2026Ej15DbContext(DbContextOptions <Dsw2026Ej15DbContext> options): base(options)
        {


        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Doctor>(e =>
        {
            e.ToTable("Doctors");
            e.Property(p => p.Name).HasMaxLength(50).IsRequired();
            e.Property(P => P.LicenseNumber).HasMaxLength(50).IsRequired();
            e.HasIndex(p => p.LicenseNumber).IsUnique();
        });
        modelBuilder.Entity<Speciality>(e =>
        {
            e.ToTable("Speciality");
            e.Property(p => p.Name).HasMaxLength(50).IsRequired();
            e.Property(P => P.Description).HasMaxLength(50).IsRequired();

        });
    }





    }

