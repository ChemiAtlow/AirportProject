﻿using Common.DTO;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    /// <summary>
    /// Service to handle the whole orchestra of the airport.
    /// </summary>
    public interface IAirportService
    {
        /// <summary>
        /// Get all <see cref="Airplane">airplanes</see> available.
        /// </summary>
        /// <returns>All airplanes in the system.</returns>
        IEnumerable<AirplaneDTO> GetAirplanes();
        /// <summary>
        /// Handle the situation of a new <see cref="Flight"/> arriving the airport.
        /// </summary>
        /// <param name="flight">The flight that has arrived.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous <see cref="Flight"/> handeling</returns>
        /// <exception cref="DbException">An error happened while attempting to save flight.</exception>
        /// <exception cref="Exception">An unknown error happened while attempting to save flight.</exception>
        Task HandleNewFlightArrivedAsync(Flight flight);
        /// <summary>
        /// Get the relavent data of a <see cref="ControlTower"/> with the a given name.
        /// </summary>
        /// <param name="name">The name of the control tower</param>
        /// <returns>The data of the control tower.</returns>
        /// <exception cref="KeyNotFoundException">No control tower with given name found.</exception>
        AirportDataDTO GetAirportData(string name);
        /// <summary>
        /// Get the <see cref="FlightHistory"/> of the a <see cref="Station"/> with a given id.
        /// </summary>
        /// <param name="stationId">Id of requested station.</param>
        /// <param name="startFrom">Row of history to start from. (pagination)</param>
        /// <param name="paginationLimit">The limit of items to fetch.</param>
        /// <returns>The flight history of the station.</returns>
        /// <exception cref="KeyNotFoundException">There is no station with requested Id.</exception>
        PaginatedDTO<FlightHistoryDTO> GetStationHistory(Guid stationId, int startFrom = 0, int paginationLimit = 15);
    }
}
