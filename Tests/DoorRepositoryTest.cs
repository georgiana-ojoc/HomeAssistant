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
    public class DoorRepositoryTest : Database
    {
        private readonly DoorRepository _doorRepository;
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly Door _newDoor;
        private readonly String email = "gaicassadsa@popescu.com";

        public DoorRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context);
            _roomRepository = new RoomRepository(Context);
            _doorRepository = new DoorRepository(Context);
            _newDoor = new Door
            {
                Name = "Frontdoor",
            };
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotNull_ThenCreateDoorAsyncShouldReturnNewDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var result = await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, _newDoor);

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsEmpty_ThenCreateDoorAsyncShouldReturnNewDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var result = await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNull_ThenCreateDoorAsyncShouldThrowException()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            _doorRepository.Invoking(r => r.CreateDoorAsync(null, house.Id, room.Id, null)).Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorExists_ThenGetDoorByIdAsyncShouldReturnDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var door = await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());

            door.Should().BeOfType<Door>();

            var result = await _doorRepository.GetDoorByIdAsync(house.Email, house.Id, room.Id, door.Id);

            result.Should().BeOfType<Door>();
            result.Should().IsSameOrEqualTo(door);
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsEmpty_ThenGetDoorByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = new Room();
            var door = new Door();

            var result = await _doorRepository.GetDoorByIdAsync(house.Email, house.Id, room.Id, door.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewDoor_WhenIdDoesNotExist_GetDoorByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();

            var result = await _doorRepository.GetDoorByIdAsync(house.Email, house.Id, room.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotNull_ThenDeleteDoorAsyncShouldReturnDoor()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var door = await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            door.Should().BeOfType<Door>();

            var result = await _doorRepository.DeleteDoorAsync(house.Email, house.Id, room.Id, door.Id);

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotNull_ThenDeleteDoorAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var door = new Door();

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            door.Should().BeOfType<Door>();

            var result = await _doorRepository.DeleteDoorAsync(house.Email, house.Id, room.Id, door.Id);
            
            result.Should().BeNull();
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetDoorsAsyncShouldReturnListOfDoors()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());



            await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());
            await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());
            await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());
            await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());
            await _doorRepository.CreateDoorAsync(email, house.Id, room.Id, new Door());

            var result = await _doorRepository.GetDoorsAsync(email, house.Id, room.Id);

            result.Should().BeOfType<List<Door>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetDoorsAsyncShouldReturnEmptyListOfDoors()
        {
            var email = "empty_email@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());


            var result = await _doorRepository.GetDoorsAsync(email, house.Id, room.Id);

            result.Count().Should().Be(0);
        }
    }
}