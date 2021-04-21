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
    public class LightBulbRepositoryTest : Database
    {
        private readonly LightBulbRepository _lightBulbRepository;
        private readonly RoomRepository _roomRepository;
        private readonly HouseRepository _houseRepository;
        private readonly LightBulb _newLightBulb;
        private readonly String email = "gagstydsa@popescu.com";

        public LightBulbRepositoryTest()
        {
            _houseRepository = new HouseRepository(Context);
            _roomRepository = new RoomRepository(Context);
            _lightBulbRepository = new LightBulbRepository(Context);
            _newLightBulb = new LightBulb
            {
                Name = "FrontlightBulb",
            };
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotNull_ThenCreateLightBulbAsyncShouldReturnNewLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var result = await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, _newLightBulb);

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsEmpty_ThenCreateLightBulbAsyncShouldReturnNewLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var result = await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNull_ThenCreateLightBulbAsyncShouldThrowException()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            _lightBulbRepository.Invoking(r => r.CreateLightBulbAsync(null, house.Id, room.Id, null)).Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbExists_ThenGetLightBulbByIdAsyncShouldReturnLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var lightBulb = await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());

            lightBulb.Should().BeOfType<LightBulb>();

            var result = await _lightBulbRepository.GetLightBulbByIdAsync(house.Email, house.Id, room.Id, lightBulb.Id);

            result.Should().BeOfType<LightBulb>();
            result.Should().IsSameOrEqualTo(lightBulb);
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsEmpty_ThenGetLightBulbByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = new Room();
            var lightBulb = new LightBulb();

            var result = await _lightBulbRepository.GetLightBulbByIdAsync(house.Email, house.Id, room.Id, lightBulb.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenIdDoesNotExist_GetLightBulbByIdAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();

            var result = await _lightBulbRepository.GetLightBulbByIdAsync(house.Email, house.Id, room.Id,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotNull_ThenDeleteLightBulbAsyncShouldReturnLightBulb()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var lightBulb = await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            lightBulb.Should().BeOfType<LightBulb>();

            var result = await _lightBulbRepository.DeleteLightBulbAsync(house.Email, house.Id, room.Id, lightBulb.Id);

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotNull_ThenDeleteLightBulbAsyncShouldReturnNull()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());
            var lightBulb = new LightBulb();

            house.Should().BeOfType<House>();
            room.Should().BeOfType<Room>();
            lightBulb.Should().BeOfType<LightBulb>();

            var result = await _lightBulbRepository.DeleteLightBulbAsync(house.Email, house.Id, room.Id, lightBulb.Id);
            
            result.Should().BeNull();
        }

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetLightBulbsAsyncShouldReturnListOfLightBulbs()
        {
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());



            await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());
            await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());
            await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());
            await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());
            await _lightBulbRepository.CreateLightBulbAsync(email, house.Id, room.Id, new LightBulb());

            var result = await _lightBulbRepository.GetLightBulbsAsync(email, house.Id, room.Id);

            result.Should().BeOfType<List<LightBulb>>();
        }


        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetLightBulbsAsyncShouldReturnEmptyListOfLightBulbs()
        {
            var email = "empty_email@gmail.com";
            var house = await _houseRepository.CreateHouseAsync(email, new House());
            var room = await _roomRepository.CreateRoomAsync(email, house.Id, new Room());


            var result = await _lightBulbRepository.GetLightBulbsAsync(email, house.Id, room.Id);

            result.Count().Should().Be(0);
        }
    }
}