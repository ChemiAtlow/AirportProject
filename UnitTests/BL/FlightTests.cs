﻿using BL.Services;
using Common.Interfaces;
using Common.Models;
using System;
using Xunit;

namespace UnitTests.BL
{
    public class FlightTests
    {
        [Fact]
        public void FlightServiceThrowsIfFlightIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>("flight", () => new FlightService(null));
            Assert.NotNull(ex);

        }

        [Fact]
        public void FlightServiceThrowsIfNegativeTime()
        {
            IFlightService flight = new FlightService(new());
            var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => flight.StartWaitingInStationAsync(-1));
            Assert.NotNull(ex);

        }

        [Fact]
        public void FlightServiceNotifiesAfterAmountOfTime()
        {
            IFlightService flight = new FlightService(new());

            var evt = Assert.RaisesAsync<EventArgs>(
                xHandler => flight.ReadyToContinue += xHandler,
                xHandler => flight.ReadyToContinue -= xHandler,
                () => flight.StartWaitingInStationAsync(0));
        }

        [Fact]
        public void FlightServiceIsFlaggedAsReadyOnlyWhenReady()
        {

            IFlightService flight = new FlightService(new());
            Assert.False(flight.IsReadyToContinue);
            flight.StartWaitingInStationAsync(0);
            Assert.True(flight.IsReadyToContinue);
        }
    }
}
