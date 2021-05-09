using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Shared;
using Shared.Models;
using Shared.Requests;
using Xunit;

namespace Tests.RepositoryTests
{
    public class ThermostatRepositoryTest : BaseRepositoryTest
    {
        #region GET_THERMOSTAT_ASYNC

        [Fact]
        public async void GivenRoomId_WhenRoomIdExists_ThenGetThermostatsAsyncShouldReturnListOfThermostats()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.GetThermostatsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"));

            result.Should().BeOfType<List<Thermostat>>();
        }

        [Fact]
        public async void GivenRoomId_WhenRoomIdDoesNotExist_ThenGetThermostatsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.GetThermostatsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenRoomId_WhenRoomIdIsEmpty_ThenGetThermostatsAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetThermostatsAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'room_id')");
        }

        #endregion

        #region GET_THERMOSTAT_BY_ID_ASYNC

        [Fact]
        public async void GivenThermostat_WhenThermostatExists_ThenGetThermostatByIdAsyncShouldReturnThermostat()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.GetThermostatByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"));

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenThermostat_WhenThermostatDoesNotExist_ThenGetThermostatByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.GetThermostatByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_THERMOSTAT_ASYNC

        [Fact]
        public async void
            GivenNewThermostat_WhenThermostatIsNotEmpty_ThenCreateThermostatAsyncShouldReturnNewThermostat()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.CreateThermostatAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new Thermostat()
                {
                    Name = "Wireless thermostat"
                });

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void
            GivenNewThermostat_WhenThermostatIsEmpty_ThenCreateThermostatAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateThermostatAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new Thermostat());
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'name')");
        }

        #endregion

        #region PARTIAL_UPDATE_THERMOSTAT_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedThermostat_WhenThermostatExists_ThenPartialUpdatedThermostatAsyncShouldReturnPartialUpdatedThermostat()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            JsonPatchDocument<ThermostatRequest> thermostatPatch = new JsonPatchDocument<ThermostatRequest>();
            thermostatPatch.Replace(t => t.Temperature, (decimal) 12.5);

            var result = await repository.PartialUpdateThermostatAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"), thermostatPatch);

            result.Should().BeOfType<Thermostat>();
            result.Temperature.Should().Be((decimal) 12.5);
        }

        [Fact]
        public async void
            GivenPartialUpdatedThermostat_WhenThermostatDoesNotExist_ThenPartialUpdatedThermostatAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            JsonPatchDocument<ThermostatRequest> thermostatPatch = new JsonPatchDocument<ThermostatRequest>();
            thermostatPatch.Replace(t => t.Temperature, (decimal) 12.5);

            var result = await repository.PartialUpdateThermostatAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), thermostatPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_THERMOSTAT_ASYNC

        [Fact]
        public async void GivenThermostat_WhenThermostatExists_ThenDeleteThermostatAsyncShouldReturnThermostat()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.DeleteThermostatAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"));

            result.Should().BeOfType<Thermostat>();
        }

        [Fact]
        public async void GivenThermostat_WhenThermostatDoesNotExist_ThenDeleteThermostatAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatRepository repository = new ThermostatRepository(context, mapper);

            var result = await repository.DeleteThermostatAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"));

            result.Should().BeNull();
        }

        #endregion
    }
}