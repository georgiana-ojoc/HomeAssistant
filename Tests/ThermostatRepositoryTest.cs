using FluentAssertions;
using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using Shared.Models;
using Xunit;

namespace Tests
{
    public class ThermostatRepositoryTest : Database
    {
        private readonly ThermostatRepository _thermostatRepository;
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly Thermostat _newThermostat;
        private readonly String email = "gfsfdsa@popescu.com";

        public ThermostatRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context);
            _roomRepository = new RoomRepository(Context);
            _thermostatRepository = new ThermostatRepository(Context);
            _newThermostat = new Thermostat
            {
                Name = "Frontthermostat",
            };
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsNotNull_ThenCreateThermostatAsyncShouldReturnNewThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var result = await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, _newThermostat);

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsEmpty_ThenCreateThermostatAsyncShouldReturnNewThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var result = await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsNull_ThenCreateThermostatAsyncShouldThrowException()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            _thermostatRepository.Invoking(r => r.CreateThermostatAsync(null, house.Id, room.Id, null)).Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatExists_ThenGetThermostatByIdAsyncShouldReturnThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var thermostat = await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());

            thermostat.Should().BeOfType<Thermostat>();

            var result = await _thermostatRepository.GetThermostatByIdAsync(house.Email, house.Id, room.Id, thermostat.Id);

            result.Should().BeOfType<Thermostat>();
            result.Should().IsSameOrEqualTo(thermostat);
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsEmpty_ThenGetThermostatByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = new Room();
            var thermostat = new Thermostat();

            var result = await _thermostatRepository.GetThermostatByIdAsync(house.Email, house.Id, room.Id, thermostat.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewThermostat_WhenIdDoesNotExist_GetThermostatByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();

            var result = await _thermostatRepository.GetThermostatByIdAsync(house.Email, house.Id, room.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsNotNull_ThenDeleteThermostatAsyncShouldReturnThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var thermostat = await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            thermostat.Should().BeOfType<Thermostat>();

            var result = await _thermostatRepository.DeleteThermostatAsync(house.Email, house.Id, room.Id, thermostat.Id);

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsNotNull_ThenDeleteThermostatAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var thermostat = new Thermostat();

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            thermostat.Should().BeOfType<Thermostat>();

            var result = await _thermostatRepository.DeleteThermostatAsync(house.Email, house.Id, room.Id, thermostat.Id);
            
            result.Should().BeNull();
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetThermostatsAsyncShouldReturnListOfThermostats()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());



            await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());
            await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());
            await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());
            await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());
            await _thermostatRepository.CreateThermostatAsync(email, house.Id, room.Id, new Thermostat());

            var result = await _thermostatRepository.GetThermostatsAsync(email, house.Id, room.Id);

            result.Should().BeOfType<List<Thermostat>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetThermostatsAsyncShouldReturnEmptyListOfThermostats()
        {
            var email = "empty_email@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());


            var result = await _thermostatRepository.GetThermostatsAsync(email, house.Id, room.Id);

            result.Count().Should().Be(0);
        }
    }
}