using System;
using System.Collections.Generic;
using System.Linq;
using API.Repositories;
using FluentAssertions;
using FluentAssertions.Common;
using Shared.Models;
using Xunit;

namespace Tests.RepositoryTests
{
    public class ThermostatRepositoryTest : RepositoryTest
    {
        private readonly ThermostatRepository _thermostatRepository;
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly Thermostat _newThermostat;
        private const string Email = "paula@popescu.com";

        public ThermostatRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context, Mapper);
            _roomRepository = new RoomRepository(Context, Mapper);
            _thermostatRepository = new ThermostatRepository(Context, Mapper);
            _newThermostat = new Thermostat
            {
                Name = "Wall thermostat",
            };
        }

        [Fact]
        public async void
            GivenNewThermostat_WhenThermostatIsNotNull_ThenCreateThermostatAsyncShouldReturnNewThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var result = await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id, _newThermostat);

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsEmpty_ThenCreateThermostatAsyncShouldReturnNewThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var result =
                await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id,
                    new Thermostat {Name = "Test"});

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatExists_ThenGetThermostatByIdAsyncShouldReturnThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var thermostat =
                await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id,
                    new Thermostat {Name = "Test"});

            thermostat.Should().BeOfType<Thermostat>();

            var result =
                await _thermostatRepository.GetThermostatByIdAsync(house.Email, house.Id, room.Id, thermostat.Id);

            result.Should().BeOfType<Thermostat>();
            result.Should().IsSameOrEqualTo(thermostat);
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsEmpty_ThenGetThermostatByIdAsyncShouldThrowError()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = new Room {Name = "Test"};
            var thermostat = new Thermostat {Name = "Test"};

            _thermostatRepository.Invoking(r =>
                r.GetThermostatByIdAsync(house.Email, house.Id, room.Id, thermostat.Id));
        }

        [Fact]
        public async void GivenNewThermostat_WhenIdDoesNotExist_GetThermostatByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();

            var result = await _thermostatRepository.GetThermostatByIdAsync(house.Email, house.Id, room.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsNotNull_ThenDeleteThermostatAsyncShouldReturnThermostat()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var thermostat =
                await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id,
                    new Thermostat {Name = "Test"});

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            thermostat.Should().BeOfType<Thermostat>();

            var result =
                await _thermostatRepository.DeleteThermostatAsync(house.Email, house.Id, room.Id, thermostat.Id);

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenNewThermostat_WhenThermostatIsNotNull_ThenDeleteThermostatAsyncShouldThrowError()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var thermostat = new Thermostat {Name = "Test"};

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            thermostat.Should().BeOfType<Thermostat>();

            _thermostatRepository.Invoking(r =>
                r.GetThermostatByIdAsync(house.Email, house.Id, room.Id, thermostat.Id));
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetThermostatsAsyncShouldReturnListOfThermostats()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});


            await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id, new Thermostat {Name = "Test"});
            await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id, new Thermostat {Name = "Test"});
            await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id, new Thermostat {Name = "Test"});
            await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id, new Thermostat {Name = "Test"});
            await _thermostatRepository.CreateThermostatAsync(Email, house.Id, room.Id, new Thermostat {Name = "Test"});

            var result = await _thermostatRepository.GetThermostatsAsync(Email, house.Id, room.Id);

            result.Should().BeOfType<List<Thermostat>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetThermostatsAsyncShouldReturnEmptyListOfThermostats()
        {
            const string newEmail = "new@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(newEmail, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(newEmail, house.Id, new Room {Name = "Test"});


            var result = await _thermostatRepository.GetThermostatsAsync(newEmail, house.Id, room.Id);

            result.Count().Should().Be(0);
        }
    }
}