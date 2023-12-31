﻿using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data;

public class VAndDCargoesDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public VAndDCargoesDbContext(DbContextOptions<VAndDCargoesDbContext> options)
        : base(options)
    {
        if (!this.Database.IsRelational())
        {
            this.Database.EnsureCreated(); 
        }
    }

    public DbSet<Truck> Trucks { get; set; } = null!;

    public DbSet<Trailer> Trailers { get; set; } = null!;

    public DbSet<Cargo> Cargoes { get; set; } = null!;

    public DbSet<Driver> Drivers { get; set; } = null!;

    public DbSet<Course> Courses { get; set; } = null!;

    public DbSet<Repairment> Repairments { get; set; } = null!;

    public DbSet<DriversTrucks> DriversTrucks { get; set; } = null!;

    public DbSet<DriversTrailers> DriversTrailers { get; set; } = null!;

    public DbSet<DriversCargoes> DriversCargoes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder.UseLazyLoadingProxies());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Assembly assembly = Assembly.GetAssembly(typeof(VAndDCargoesDbContext)) ?? Assembly.GetExecutingAssembly();

        builder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(builder);
    }
}

