﻿using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services
{
    public class AirportService : IAirportService
    {
        private readonly IRepository<Airplane> airplaneRepository;
        private readonly IRepository<Station> stationRepository;
        private readonly IRepository<ControlTower> controlTowerRepository;
        private readonly IRepository<Flight> flightRepository;
        private readonly IStationTreeBuilderService stationTreeBuilder;
        private readonly IFutureFlightNotifier notifier;

        private static bool InitialCreate = true;

        public AirportService(IRepository<Airplane> airplaneRepository,
            IRepository<Station> stationRepository,
            IRepository<ControlTower> controlTowerRepository,
            IRepository<Flight> flightRepository,
            IStationTreeBuilderService stationTreeBuilder,
            INotifier notifier)
        {
            this.airplaneRepository = airplaneRepository;
            this.stationRepository = stationRepository;
            this.controlTowerRepository = controlTowerRepository;
            this.flightRepository = flightRepository;
            this.stationTreeBuilder = stationTreeBuilder;
            this.notifier = notifier;
            CreateStationTrees();
            if (InitialCreate) InitializeUndeltFlights();
        }


        public IEnumerable<Airplane> GetAirplanes() => airplaneRepository.GetAll();

        public async Task HandleNewFlightArrivedAsync(Flight flight)
        {
            Flight dbFlight = await flightRepository.AddAsync(flight);
            notifier.NotifyFutureFlightAdded(dbFlight);

            SendFlightToControlTowerAtTime(flight);
        }

        private void InitializeUndeltFlights()
        {
            InitialCreate = false;
            IEnumerable<Flight> flights = flightRepository.GetAll();

            IEnumerable<Flight> undeltFlights = flights
                .Where(f => f.History?.Count <= 0)
                .OrderBy(f => f.PlannedTime);
            foreach (Flight flight in undeltFlights)
            {
                SendFlightToControlTowerAtTime(flight);
            }
        }

        private async void SendFlightToControlTowerAtTime(Flight flight)
        {
            string controlTowerName = flight.Direction == FlightDirection.Landing ? flight.To : flight.From;
            IGetFlights controlTowerService = stationTreeBuilder[controlTowerName] ??
                throw new ArgumentOutOfRangeException(nameof(flight), "Control tower does not exist");
            TimeSpan delayUntillFlight = flight.PlannedTime - DateTime.Now;
            if (delayUntillFlight > TimeSpan.Zero)
            {
                await Task.Delay(delayUntillFlight);
            }
            controlTowerService.FlightArrived(new FlightService(flight));
        }

        private void CreateStationTrees()
        {
            IEnumerable<ControlTower> controlTowers = controlTowerRepository.GetAll();
            IEnumerable<Station> stations = stationRepository.GetAll();
            stationTreeBuilder.BuildStationsTree(controlTowers, stations);
        }
    }
}