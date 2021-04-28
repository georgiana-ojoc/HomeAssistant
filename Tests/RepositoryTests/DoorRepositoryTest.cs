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
    public class DoorRepositoryTest : RepositoryTest
    {
        private readonly DoorRepository _doorRepository;
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly Door _newDoor;
        private const string Email = "ioana@popescu.com";

        public DoorRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context, Mapper);
            _roomRepository = new RoomRepository(Context, Mapper);
            _doorRepository = new DoorRepository(Context, Mapper);
            _newDoor = new Door
            {
                Name = "Front door",
            };
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotNull_ThenCreateDoorAsyncShouldReturnNewDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var result = await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, _newDoor);

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsEmpty_ThenCreateDoorAsyncShouldReturnNewDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var result = await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorExists_ThenGetDoorByIdAsyncShouldReturnDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var door = await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});

            door.Should().BeOfType<Door>();

            var result = await _doorRepository.GetDoorByIdAsync(house.Email, house.Id, room.Id, door.Id);

            result.Should().BeOfType<Door>();
            result.Should().IsSameOrEqualTo(door);
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsEmpty_ThenGetDoorByIdAsyncShouldThrowError()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = new Room {Name = "Test"};
            var door = new Door {Name = "Test"};

            _doorRepository.Invoking(r => r.GetDoorByIdAsync(house.Email, house.Id, room.Id, door.Id));
        }

        [Fact]
        public async void GivenNewDoor_WhenIdDoesNotExist_GetDoorByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();

            var result = await _doorRepository.GetDoorByIdAsync(house.Email, house.Id, room.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotNull_ThenDeleteDoorAsyncShouldReturnDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var door = await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            door.Should().BeOfType<Door>();

            var result = await _doorRepository.DeleteDoorAsync(house.Email, house.Id, room.Id, door.Id);

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotNull_ThenDeleteDoorAsyncShouldThrowError()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});
            var door = new Door {Name = "Test"};

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            door.Should().BeOfType<Door>();

            _doorRepository.Invoking(r => r.DeleteDoorAsync(house.Email, house.Id, room.Id, door.Id));
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetDoorsAsyncShouldReturnListOfDoors()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room {Name = "Test"});


            await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});
            await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});
            await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});
            await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});
            await _doorRepository.CreateDoorAsync(Email, house.Id, room.Id, new Door {Name = "Test"});

            var result = await _doorRepository.GetDoorsAsync(Email, house.Id, room.Id);

            result.Should().BeOfType<List<Door>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetDoorsAsyncShouldReturnEmptyListOfDoors()
        {
            const string newEmail = "new@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(newEmail, new House {Name = "Test"});
            var room = await _roomRepository.CreateRoomAsync(newEmail, house.Id, new Room {Name = "Test"});


            var result = await _doorRepository.GetDoorsAsync(newEmail, house.Id, room.Id);

            result.Count().Should().Be(0);
        }
    }
}