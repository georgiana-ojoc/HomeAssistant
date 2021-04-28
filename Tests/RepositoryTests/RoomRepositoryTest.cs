using System;
using System.Collections.Generic;
using System.Linq;
using API.Repositories;
using FluentAssertions;
using Shared.Models;
using Xunit;

namespace Tests.RepositoryTests
{
    public class RoomRepositoryTest : RepositoryTest
    {
        private readonly HouseRepository _houseRepository;
        private readonly RoomRepository _roomRepository;
        private readonly Room _newRoom;
        private const string Email = "elena@popescu.com";

        public RoomRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context, Mapper);
            _roomRepository = new RoomRepository(Context, Mapper);
            _newRoom = new Room
            {
                Name = "Living room",
            };
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotNull_ThenCreateRoomAsyncShouldReturnNewRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
            var result = await _roomRepository.CreateRoomAsync(Email, house.Id, _newRoom);

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsEmpty_ThenCreateRoomAsyncShouldReturnNewRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
            var result = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room());

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomExists_ThenGetRoomByIdAsyncShouldReturnRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room());

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.GetRoomByIdAsync(house.Email, house.Id, room.Id);

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsEmpty_ThenGetRoomByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
            var room = new Room();

            var result = await _roomRepository.GetRoomByIdAsync(house.Email, house.Id, room.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewRoom_WhenIdDoesNotExist_GetRoomByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
            var room = new Room();

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.GetRoomByIdAsync(house.Email, house.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotNull_ThenDeleteRoomAsyncShouldReturnRoom()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
            var room = await _roomRepository.CreateRoomAsync(Email, house.Id, new Room());

            room.Should().BeOfType<Room>();

            var result = await _roomRepository.DeleteRoomAsync(house.Email, house.Id, room.Id);

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotNull_ThenDeleteRoomAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(Email, new House());
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
            const string newEmail = "new@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(newEmail, new House());


            var result = await _roomRepository.GetRoomsAsync(newEmail, house.Id);

            result.Count().Should().Be(0);
        }
    }
}