using FluentAssertions;
using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Xunit;

namespace Tests
{
    public class RoomRepositoryTest : Database
    {
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly Room _newRoom;
        private readonly String email = "ghsdsa@popescu.com";

        public RoomRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context);
            _roomRepository = new RoomRepository(Context);
            _newRoom = new Room
            {
                Name = "LivingRoom",
            };
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotNull_ThenCreateRoomAsyncShouldReturnNewRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var result = await _roomRepository.CreateRoomAsync(email, house.Id, _newRoom);

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsEmpty_ThenCreateRoomAsyncShouldReturnNewRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var result = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNull_ThenCreateRoomAsyncShouldThrowException()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            _roomRepository.Invoking(r => r.CreateRoomAsync(null, house.Id, null)).Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomExists_ThenGetRoomByIdAsyncShouldReturnRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.GetRoomByIdAsync(house.Email, house.Id, room.Id);

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsEmpty_ThenGetRoomByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = new Room();

            var result = await _roomRepository.GetRoomByIdAsync(house.Email, house.Id, room.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewRoom_WhenIdDoesNotExist_GetRoomByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = new Room();

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.GetRoomByIdAsync(house.Email, house.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotNull_ThenDeleteRoomAsyncShouldReturnRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.DeleteRoomAsync(house.Email, house.Id, room.Id);

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotNull_ThenDeleteRoomAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = new Room();

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.DeleteRoomAsync(house.Email, house.Id, room.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetRoomsAsyncShouldReturnListOfRooms()
        {
            string email = "gdsicadsa@popescu.com";
            var house = await _houseRepository.CreateHouseAsync(email, new House());


            await _roomRepository.CreateRoomAsync(house.Email, house.Id, new Room());
            await _roomRepository.CreateRoomAsync(house.Email, house.Id, new Room());
            await _roomRepository.CreateRoomAsync(house.Email, house.Id, new Room());
            await _roomRepository.CreateRoomAsync(house.Email, house.Id, new Room());
            await _roomRepository.CreateRoomAsync(house.Email, house.Id, new Room());

            var result = await _roomRepository.GetRoomsAsync(email, house.Id);

            result.Should().BeOfType<List<Room>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetRoomsAsyncShouldReturnEmptyListOfRooms()
        {
            var email = "empty_email@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(email, new House());


            var result = await _roomRepository.GetRoomsAsync(email, house.Id);

            result.Count().Should().Be(0);
        }
    }
}