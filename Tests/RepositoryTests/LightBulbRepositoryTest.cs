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
    public class LightBulbRepositoryTest : RepositoryTest
    {
        private readonly LightBulbRepository _lightBulbRepository;
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly LightBulb _newLightBulb;
        private const string Email = "ioana@popescu.com";

        public LightBulbRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context, Mapper);
            _roomRepository = new RoomRepository(Context, Mapper);
            _lightBulbRepository = new LightBulbRepository(Context, Mapper);
            _newLightBulb = new LightBulb
            {
                Name = "Lamp",
            };
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotNull_ThenCreateLightBulbAsyncShouldReturnNewLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var result = await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id, _newLightBulb);

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsEmpty_ThenCreateLightBulbAsyncShouldReturnNewLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var result =
                await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id,
                    new LightBulb {Name = "Test"});

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbExists_ThenGetLightBulbByIdAsyncShouldReturnLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var lightBulb =
                await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id,
                    new LightBulb {Name = "Test"});

            lightBulb.Should().BeOfType<LightBulb>();

            var result = await _lightBulbRepository.GetLightBulbByIdAsync(house.Email, house.Id, room.Id, lightBulb.Id);

            result.Should().BeOfType<LightBulb>();
            result.Should().IsSameOrEqualTo(lightBulb);
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsEmpty_ThenGetLightBulbByIdAsyncShouldThrowError()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = new Room {Name = "Test"};
            var lightBulb = new LightBulb {Name = "Test"};

            _lightBulbRepository.Invoking(r => r.GetLightBulbByIdAsync(house.Email, house.Id, room.Id, lightBulb.Id));
        }

        [Fact]
        public async void GivenNewLightBulb_WhenIdDoesNotExist_GetLightBulbByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();

            var result = await _lightBulbRepository.GetLightBulbByIdAsync(house.Email, house.Id, room.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotNull_ThenDeleteLightBulbAsyncShouldReturnLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var lightBulb =
                await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id,
                    new LightBulb {Name = "Test"});

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            lightBulb.Should().BeOfType<LightBulb>();

            var result = await _lightBulbRepository.DeleteLightBulbAsync(house.Email, house.Id, room.Id, lightBulb.Id);

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotNull_ThenDeleteLightBulbAsyncShouldThrowError()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var lightBulb = new LightBulb {Name = "Test"};

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            lightBulb.Should().BeOfType<LightBulb>();

            _lightBulbRepository.Invoking(r => r.DeleteLightBulbAsync(house.Email, house.Id, room.Id, lightBulb.Id));
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetLightBulbsAsyncShouldReturnListOfLightBulbs()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});


            await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id, new LightBulb {Name = "Test"});
            await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id, new LightBulb {Name = "Test"});
            await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id, new LightBulb {Name = "Test"});
            await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id, new LightBulb {Name = "Test"});
            await _lightBulbRepository.CreateLightBulbAsync(Email, house.Id, room.Id, new LightBulb {Name = "Test"});

            var result = await _lightBulbRepository.GetLightBulbsAsync(Email, house.Id, room.Id);

            result.Should().BeOfType<List<LightBulb>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetLightBulbsAsyncShouldReturnEmptyListOfLightBulbs()
        {
            const string newEmail = "new@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(newEmail, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(newEmail, house.Id, new Room {Name = "Test"});


            var result = await _lightBulbRepository.GetLightBulbsAsync(newEmail, house.Id, room.Id);

            result.Count().Should().Be(0);
        }
    }
}