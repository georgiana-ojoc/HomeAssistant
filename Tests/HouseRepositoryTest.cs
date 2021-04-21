using FluentAssertions;
using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Xunit;

namespace Tests
{
    public class HouseRepositoryTest : Database
    {
        private readonly HouseRepository _repository;
        private readonly House _newHouse;

        public HouseRepositoryTest()
        {
            _repository = new HouseRepository(Context);
            _newHouse = new House
            {
                Name = "Apartment",
            };
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsNotNull_ThenCreateHouseAsyncShouldReturnNewHouse()
        {
            var result = await _repository.CreateHouseAsync("gicadsa@popescu.com", _newHouse);

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsEmpty_ThenCreateHouseAsyncShouldReturnNewHouse()
        {
            var result = await _repository.CreateHouseAsync("ggadsa@popescu.com", new House());

            result.Should().BeOfType<House>();
        }

        [Fact]
        public void GivenNewHouse_WhenHouseIsNull_ThenCreateHouseAsyncShouldThrowException()
        {
            _repository.Invoking(r => r.CreateHouseAsync(null, null)).Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseExists_ThenGetHouseByIdAsyncShouldReturnHouse()
        {
            var house = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", _newHouse);

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
        public async void GivenNewHouse_WhenHouseIsNotNull_ThenDeleteHouseAsyncShouldReturnHouse()
        {
            var house = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", _newHouse);


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
        public async void GivenNewHouse_WhenIdDoesNotExist_ThenDeleteHouseAsyncShouldReturnNull()
        {
            var house = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", _newHouse);
            
            var result = await _repository.GetHouseByIdAsync(house.Email, 
                Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));
            
            result.Should().BeNull();
        }


        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetHousesAsyncShouldReturnListOfHouses()
        {
            string email = "gdsicadsa@popescu.com";
            
            await _repository.CreateHouseAsync(email, new House());
            await _repository.CreateHouseAsync(email, new House());
            await _repository.CreateHouseAsync(email, new House());
            await _repository.CreateHouseAsync(email, new House());
            
            var result = await _repository.GetHousesAsync(email);
            
            result.Should().BeOfType<List<House>>();
        }

        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetHousesAsyncShouldReturnEmptyListOfHouses()
        {
            var email = "empty_email@gmail.com";
            
            var result = await _repository.GetHousesAsync(email);
            
            result.Count().Should().Be(0);
        }
    }
}