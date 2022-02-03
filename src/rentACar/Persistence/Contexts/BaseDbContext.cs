﻿using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class BaseDbContext :DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public BaseDbContext(DbContextOptions options, IConfiguration configuration) :base(options)
        {
            Configuration = configuration;
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    base.OnConfiguring(optionsBuilder.UseSqlServer(Configuration.GetConnectionString("RentACarConnectionString")) );
            //}            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly() );

            modelBuilder.Entity<Brand>( b =>
            {
                b.ToTable("Brands").HasKey(k => k.Id);
                b.Property(p => p.Id).HasColumnName("Id");
                b.Property(p => p.Name).HasColumnName("Name");
                b.HasMany(p => p.Models);
            });

            modelBuilder.Entity<Color>(c =>
            {
                c.ToTable("Colors").HasKey(k => k.Id);
                c.Property(p => p.Id).HasColumnName("Id");
                c.Property(p => p.Name).HasColumnName("Name");
                c.HasMany(p => p.Cars);
            });

            modelBuilder.Entity<Fuel>(c =>
            {
                c.ToTable("Fuels").HasKey(k => k.Id);
                c.Property(p => p.Id).HasColumnName("Id");
                c.Property(p => p.Name).HasColumnName("Name");
                c.HasMany(p => p.Models);
            });

            modelBuilder.Entity<Transmission>(c =>
            {
                c.ToTable("Transmissions").HasKey(k => k.Id);
                c.Property(p => p.Id).HasColumnName("Id");
                c.Property(p => p.Name).HasColumnName("Name");
                c.HasMany(p => p.Models);
            });

            modelBuilder.Entity<Model>(m =>
            {
                m.ToTable("Models").HasKey(k => k.Id);
                m.Property(p => p.Id).HasColumnName("Id");
                m.Property(p => p.Name).HasColumnName("Name");
                m.Property(p => p.TransmissionId).HasColumnName("TransmissionId");
                m.Property(p => p.FuelId).HasColumnName("FuelId");
                m.Property(p => p.BrandId).HasColumnName("BrandId");
                m.Property(p => p.DailyPrice).HasColumnName("DailyPrice");
                m.Property(p => p.ImageUrl).HasColumnName("ImageUrl");
                m.HasMany(p => p.Cars);
                m.HasOne(p => p.Transmission);
                m.HasOne(p => p.Fuel);
                m.HasOne(p => p.Brand);
            });

            modelBuilder.Entity<Maintenance>(m =>
            {
                m.ToTable("Maintenances").HasKey(k => k.Id);
                m.Property(p => p.Id).HasColumnName("Id");
                m.Property(p => p.CarId).HasColumnName("CarId");
                m.Property(p => p.Description).HasColumnName("Description");
                m.Property(p => p.MaintenanceDate).HasColumnName("MaintenanceDate");
                m.Property(p => p.ReturnDate).HasColumnName("ReturnDate");
                m.HasOne(c => c.Car);
            });

            modelBuilder.Entity<Rental>( r =>
            {
                r.ToTable("Rentals").HasKey(r => r.Id);
                r.Property(p => p.Id).HasColumnName("Id");
                r.Property(p => p.CarId).HasColumnName("CarId");
                r.Property(p => p.RentDate).HasColumnName("RentDate");
                r.Property(p => p.ReturnDate).HasColumnName("ReturnDate");
                r.Property(p => p.ReturnedDate).HasColumnName("ReturnedDate");
                r.Property(p => p.RentedKilometer).HasColumnName("RentedKilometer");
                r.Property(p => p.ReturnedKilometer).HasColumnName("ReturnedKilometer");
            });

            modelBuilder.Entity<Car>(c =>
            {
                c.ToTable("Cars").HasKey(k => k.Id);
                c.Property(p => p.Id).HasColumnName("Id");
                c.Property(p => p.ModelId).HasColumnName("ModelId");
                c.Property(p => p.ColorId).HasColumnName("ColorId");
                c.Property(p => p.Plate).HasColumnName("Plate");
                c.Property(p => p.ModelYear).HasColumnName("ModelYear");
                c.Property(p => p.CarState).HasColumnName("CarState");
                c.HasOne(c => c.Model);
                c.HasOne(c => c.Color);
            });

            var brand1 = new Brand(1, "BMW");
            var brand2 = new Brand(2, "Mercedes");
            modelBuilder.Entity<Brand>().HasData(brand1, brand2);

            var color1 = new Color(1, "Yellow");
            var color2 = new Color(2, "Green");
            var color3 = new Color(3, "Blue");
            modelBuilder.Entity<Color>().HasData(color1, color2, color3);

            var transmission1 = new Transmission(1, "Manuel");
            var transmission2 = new Transmission(2, "Automatic");
            modelBuilder.Entity<Transmission>().HasData(transmission1, transmission2);

            var fuel1 = new Fuel(1, "Diesel");
            var fuel2 = new Fuel(2, "Gasoline");
            var fuel3 = new Fuel(3, "Electiricity");
            modelBuilder.Entity<Fuel>().HasData(fuel1, fuel2, fuel3);

            var model1 = new Model(1, "320", 250, 2, 2, 1, "https://www.arabahabercisi.com/wp-content/uploads/2020/01/BMW-320-dizel-Hibrit-Geliyor.jpg");
            var model2 = new Model(2, "C250", 300, 1, 2, 2, "https://www.autocar.co.uk/sites/autocar.co.uk/files/mercedes-c250-4.jpg");
            modelBuilder.Entity<Model>().HasData(model1, model2);

            var car1 = new Car(1, 1, 3, "34HUS256", 2020, CarState.Available);
            var car2 = new Car(2, 2, 1, "34HUS257", 2021, CarState.Available);
            modelBuilder.Entity<Car>().HasData(car1, car2);
        }
    }
}