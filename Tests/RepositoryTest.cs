using FluentAssertions;
using API.Repositories;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using Shared.Models;
using Xunit;

namespace Tests
{
    public class RepositoryTest : DatabaseBaseTest
    {
        private readonly HouseRepository _repository;
        private House _newProduct;

        public RepositoryTest()
        {
            _repository = new HouseRepository(context);
            _newProduct = new House
            {
                Name = "Keyboard",
            };
        }

        // GIVEN-WHEN-THEN
        [Fact]
        public async void Given_NewProduct_WhenProductIsNotNull_Then_CreateHouseAsyncShouldReturnANewProduct()
        {
            // AAA
            // Arrange && Act
            var result = await _repository.CreateHouseAsync("gicadsa@popescu.com", _newProduct);

            //Assert 
            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void Given_EmptyProduct_WhenProductIsNotNull_Then_CreateHouseAsyncShouldReturnANewProduct()
        {
            var result = await _repository.CreateHouseAsync("ggadsa@popescu.com", new House());

            //Assert 
            result.Should().BeOfType<House>();
        }

        [Fact]
        public void Given_NewProduct_WhenProductIsNull_Then_CreateHouseAsyncShouldReturnThrowException()
        {
            _repository.Invoking(r => r.CreateHouseAsync(null, null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void MakeHouse_Then_GetHouseByIdAsyncShouldReturnTheHouseDetails()
        {
            var house = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", _newProduct);

            //Assert 
            house.Should().BeOfType<House>();

            var result = await _repository.GetHouseByIdAsync(house.Email, house.Id);

            //Assert 
            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void Given_EmptyHouse_Then_GetHouseByIdAsyncShouldBeNull()
        {
            var house = new House();


            var result = await _repository.GetHouseByIdAsync(house.Email, house.Id);
            result.Should().BeNull();
        }    
        [Fact]
        public async void MakeHouse_Then_DeleteHouseAsyncShouldReturnTheHouseDetails()
        {
            var house = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", _newProduct);


            var result = await _repository.DeleteHouseAsync(house.Email, house.Id);
            result.Should().BeOfType<House>();
        }
        
        [Fact]
        public async void Given_EmptyHouse_Then_DeleteHouseAsyncShouldBeNull()
        {
            var house = new House();


            var result = await _repository.GetHouseByIdAsync(house.Email, house.Id);
            result.Should().BeNull();
        }
        
        [Fact]
        public async void Given_HouseWithWrongId_Then_DeleteHouseAsyncShouldBeNull()
        {
            var house = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", _newProduct);


            var result = await _repository.GetHouseByIdAsync(house.Email, Guid.Parse("3f953890-20ad-48b5-a272-c0faa8f09ea3"));
            result.Should().BeNull();
        }
        
        
        [Fact]
        public async void Given_Email_Then_GetHousesAsyncShouldReturnAListOfHouses()
        {
            var house1 = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", new House());
            var house2 = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", new House());
            var house3 = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", new House());
            var house4 = await _repository.CreateHouseAsync("gdsicadsa@popescu.com", new House());


            var result = await _repository.GetHousesAsync(house1.Email);
            result.Should().BeOfType<List<House>>();
        }
        
        [Fact]
        public async void Given_Email_WithNoHouses_Then_GetHousesAsyncShouldReturnAnEmptyListOfHouses()
        {
            var email = "empty_email@gmail.com";


            var result = await _repository.GetHousesAsync(email);
            result.Should().BeOfType<List<House>>();
            result.Count().Should().Be(0);
        }
        
        
    }
}