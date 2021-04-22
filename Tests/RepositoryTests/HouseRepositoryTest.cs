using System;
using System.Collections.Generic;
using System.Linq;
using API.Repositories;
using FluentAssertions;
using Shared.Models;
using Xunit;

namespace Tests.RepositoryTests
{
    public class HouseRepositoryTest : Database
    {
        private readonly HouseRepository _repository;
        private readonly House _newHouse;
        private const string Email = "maria@popescu.com";

        public HouseRepositoryTest()
        {
            _repository = new HouseRepository(Context);
            _newHouse = new House
            {
                Name = "Apartment"
            };
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsNotNull_ThenCreateHouseAsyncShouldReturnNewHouse()
        {
            var result = await _repository.CreateHouseAsync(Email, _newHouse);

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsEmpty_ThenCreateHouseAsyncShouldReturnNewHouse()
        {
            var result = await _repository.CreateHouseAsync(Email, new House());

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseExists_ThenGetHouseByIdAsyncShouldReturnHouse()
        {
            var house = await _repository.CreateHouseAsync(Email, _newHouse);

            house.Should().BeOfType<House>();

            var result = await _repository.GetHouseByIdAsync(house.Email, house.Id);

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsEmpty_ThenGetHouseByIdAsyncShouldReturnNull()
        {
            var house = new House();

            var result = await _repository.GetHouseByIdAsync(house.Email, house.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenNewHouse_WhenIdDoesNotExist_GetHouseByIdAsyncShouldReturnNull()
        {
            var house = await _repository.CreateHouseAsync(Email, _newHouse);

            var result = await _repository.GetHouseByIdAsync(house.Email,
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));

            result.Should().BeNull();
        }


        [Fact]
        public async void GivenNewHouse_WhenHouseIsNotNull_ThenDeleteHouseAsyncShouldReturnHouse()
        {
            var house = await _repository.CreateHouseAsync(Email, _newHouse);


            var result = await _repository.DeleteHouseAsync(house.Email, house.Id);

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsNotNull_ThenDeleteHouseAsyncShouldReturnNull()
        {
            var house = new House();

            var result = await _repository.GetHouseByIdAsync(house.Email, house.Id);

            result.Should().BeNull();
        }


        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetHousesAsyncShouldReturnListOfHouses()
        {
            await _repository.CreateHouseAsync(Email, new House());
            await _repository.CreateHouseAsync(Email, new House());
            await _repository.CreateHouseAsync(Email, new House());
            await _repository.CreateHouseAsync(Email, new House());

            var result = await _repository.GetHousesAsync(Email);

            result.Should().BeOfType<List<House>>();
        }

        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetHousesAsyncShouldReturnEmptyListOfHouses()
        {
            const string newEmail = "new@gmail.com";

            var result = await _repository.GetHousesAsync(newEmail);

            result.Count().Should().Be(0);
        }
    }
}