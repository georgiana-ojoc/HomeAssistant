using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API;
using API.Models;
using API.Repositories;
using API.Requests;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

namespace Tests.RepositoryTests
{
    public class HouseRepositoryTest : BaseRepositoryTest
    {
        #region GET_HOUSE_ASYNC

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetHousesAsyncShouldReturnListOfHouses()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.GetHousesAsync("homeassistantgo@outlook.com");

            result.Should().BeOfType<List<House>>();
        }

        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetHousesAsyncShouldReturnEmptyListOfHouses()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.GetHousesAsync("jane.doe@mail.com");

            result.Count().Should().Be(0);
        }

        [Fact]
        public async void GivenEmail_WhenEmailIsEmpty_ThenGetHousesAsyncShouldThrowArgumentException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            Func<Task> function = async () => { await repository.GetHousesAsync(null); };

            await function.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Email cannot be empty.");
        }

        #endregion

        #region GET_HOUSE_BY_ID_ASYNC

        [Fact]
        public async void GivenHouse_WhenHouseExists_ThenGetHouseByIdAsyncShouldReturnHouse()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.GetHouseByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"));

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenHouse_WhenHouseDoesNotExist_ThenGetHouseByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.GetHouseByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_HOUSE_ASYNC

        [Fact]
        public async void GivenNewHouse_WhenHouseIsNotEmpty_ThenCreateHouseAsyncShouldReturnNewHouse()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.CreateHouseAsync("homeassistantgo@outlook.com", new House()
            {
                Name = "Penthouse"
            });

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenNewHouse_WhenHouseIsEmpty_ThenCreateHouseAsyncShouldThrowArgumentException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateHouseAsync("homeassistantgo@outlook.com", new House());
            };

            await function.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Name cannot be empty.");
        }

        [Fact]
        public async void GivenNewHouse_WhenNameExists_ThenCreateHouseAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateHouseAsync("homeassistantgo@outlook.com", new House()
                {
                    Name = "Apartment"
                });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a house with the specified name.");
        }

        #endregion

        #region PARTIAL_UPDATE_HOUSE_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedHouse_WhenHouseExists_ThenPartialUpdatedHouseAsyncShouldReturnPartialUpdatedHouse()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            JsonPatchDocument<HouseRequest> housePatch = new JsonPatchDocument<HouseRequest>();
            housePatch.Replace(h => h.Name, "Lake house");

            var result = await repository.PartialUpdateHouseAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"), housePatch);

            result.Should().BeOfType<House>();
            result.Name.Should().Be("Lake house");
        }

        [Fact]
        public async void GivenPartialUpdatedHouse_WhenHouseDoesNotExist_ThenPartialUpdatedHouseAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            JsonPatchDocument<HouseRequest> housePatch = new JsonPatchDocument<HouseRequest>();
            housePatch.Replace(h => h.Name, "Lake house");

            var result = await repository.PartialUpdateHouseAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"), housePatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_HOUSE_ASYNC

        [Fact]
        public async void GivenHouse_WhenHouseExists_ThenDeleteHouseAsyncShouldReturnHouse()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.DeleteHouseAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"));

            result.Should().BeOfType<House>();
        }

        [Fact]
        public async void GivenHouse_WhenHouseDoesNotExist_ThenDeleteHouseAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            HouseRepository repository = new HouseRepository(context, mapper);

            var result = await repository.DeleteHouseAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"));

            result.Should().BeNull();
        }

        #endregion
    }
}