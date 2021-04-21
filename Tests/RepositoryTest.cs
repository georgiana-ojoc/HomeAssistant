using FluentAssertions;
using API.Repositories;
using System;
using Shared.Models;
using Xunit;

namespace Tests
{
    public class RepositoryTest : DatabaseBaseTest
    {
        private readonly HouseRepository _repository;
        private readonly House _newProduct;

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
    }
}