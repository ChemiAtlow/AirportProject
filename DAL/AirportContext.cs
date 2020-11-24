﻿using Common.Models;
using DAL.DummyData;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AirportContext : DbContext
    {
        public AirportContext(DbContextOptions<AirportContext> options) : base(options) { }

        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightHistory> FlightHistories { get; set; }
        public virtual DbSet<ControlTower> ControlTowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetControlTowerNameToBeUnique(modelBuilder);
            SetStationFlightRelation(modelBuilder);
            DefineStationToStationRelation(modelBuilder);
            DefineStationToControlTowerRelation(modelBuilder);
            InjectPrePopulatedData(modelBuilder);
        }

        private void SetControlTowerNameToBeUnique(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControlTower>()
                .HasIndex(ct => ct.Name)
                .IsUnique();
        }

        private void SetStationFlightRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
                .HasOne(s => s.CurrentFlight)
                .WithOne(f => f.Station)
                .HasForeignKey<Station>(s => s.CurrentFlightId);
        }

        private void DefineStationToControlTowerRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StationControlTowerRelation>(entity =>
            {
                entity.HasKey(sctr => new { sctr.StationToId, sctr.Direction, sctr.ControlTowerId });
                entity
                    .HasOne(sctr => sctr.Station)
                    .WithOne(s => s.ControlTowerRelation)
                    .HasForeignKey<StationControlTowerRelation>(sctr => sctr.StationToId);
                entity
                    .HasOne(sctr => sctr.ControlTower)
                    .WithMany(ct => ct.FirstStations)
                    .HasForeignKey(sctr => sctr.ControlTowerId);
            });
        }

        private void DefineStationToStationRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StationRelation>(entity =>
            {
                entity.HasKey(sr => new { sr.StationToId, sr.StationFromId, sr.Direction });
                entity
                    .HasOne(sr => sr.StationFrom)
                    .WithMany(s => s.ChildrenStations)
                    .HasForeignKey(sr => sr.StationFromId);
                entity
                    .HasOne(sr => sr.StationTo)
                    .WithMany(s => s.ParentStations)
                    .HasForeignKey(sr => sr.StationToId);
            });
        }

        private void InjectPrePopulatedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airplane>().HasData(PrePopulateData.Airplanes);
            modelBuilder.Entity<Flight>().HasData(PrePopulateData.Flights);
            modelBuilder.Entity<Station>().HasData(PrePopulateData.Stations);
            modelBuilder.Entity<StationRelation>().HasData(PrePopulateData.StationRelations);
            modelBuilder.Entity<FlightHistory>().HasData(PrePopulateData.FlightHistories);
            modelBuilder.Entity<ControlTower>().HasData(PrePopulateData.ControlTowers);
            modelBuilder.Entity<StationControlTowerRelation>().HasData(PrePopulateData.StationControlTowerRelations);
        }
    }
}
