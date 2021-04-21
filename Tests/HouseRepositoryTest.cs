using FluentAssertions;
using API.Repositories;
using System;
using Shared.Models;
using Xunit;

namespace Tests
{
    public class RepositoryTest : Database
    {
        private readonly HouseRepository _repository;
        private readonly House _newHouse;

        public RepositoryTest()
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
        public async void GivenEmptyHouse_WhenHouseIsNotNull_ThenCreateHouseAsyncShouldReturnNewHouse()
        {
            var result = await _repository.CreateHouseAsync("ggadsa@popescu.com", new House());
            
            result.Should().BeOfType<House>();
        }

        [Fact]
        public void Given_NewHouse_WhenHouseIsNull_ThenCreateHouseAsyncShouldThrowException()
        {
            _repository.Invoking(r => r.CreateHouseAsync(null, null)).Should()
                .Throw<ArgumentNullException>();
        }
    }
}