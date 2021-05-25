using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API;
using API.Models;
using API.Repositories;
using API.Requests;
using API.Responses;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

namespace Tests.RepositoryTests
{
    public class ThermostatCommandRepositoryTest : BaseRepositoryTest
    {
        #region GET_THERMOSTAT_COMMAND_ASYNC

        [Fact]
        public async void
            GivenScheduleId_WhenScheduleIdExists_ThenGetThermostatCommandsAsyncShouldReturnListOfThermostatCommands()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.GetThermostatCommandsAsync(
                "homeassistantgo@outlook.com", Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"));

            result.Should().BeOfType<List<ThermostatCommandResponse>>();
        }

        [Fact]
        public async void GivenScheduleId_WhenScheduleIdDoesNotExist_ThenGetThermostatCommandsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.GetThermostatCommandsAsync(
                "homeassistantgo@outlook.com", Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void
            GivenScheduleId_WhenScheduleIdIsEmpty_ThenGetThermostatCommandsAsyncShouldThrowArgumentException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetThermostatCommandsAsync("homeassistantgo@outlook.com",
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Schedule id cannot be empty.");
        }

        #endregion

        #region GET_THERMOSTAT_COMMAND_BY_ID_ASYNC

        [Fact]
        public async void
            GivenThermostatCommand_WhenThermostatCommandExists_ThenGetThermostatCommandByIdAsyncShouldReturnThermostatCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.GetThermostatCommandByIdAsync(
                "homeassistantgo@outlook.com", Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04"));

            result.Should().BeOfType<ThermostatCommand>();
        }

        [Fact]
        public async void
            GivenThermostatCommand_WhenThermostatCommandDoesNotExist_ThenGetThermostatCommandByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.GetThermostatCommandByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_THERMOSTAT_COMMAND_ASYNC

        [Fact]
        public async void
            GivenNewThermostatCommand_WhenThermostatCommandIsNotEmpty_ThenCreateThermostatCommandAsyncShouldReturnNewThermostatCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.CreateThermostatCommandAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new ThermostatCommand()
                {
                    ThermostatId = Guid.Parse("c207eada-509b-4655-9f99-d3be6786e895"),
                    Temperature = (decimal) 22.5
                });

            result.Should().BeOfType<ThermostatCommand>();
        }

        [Fact]
        public async void
            GivenNewThermostatCommand_WhenThermostatCommandIsEmpty_ThenCreateThermostatCommandAsyncShouldThrowArgumentException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateThermostatCommandAsync("homeassistantgo@outlook.com",
                    Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new ThermostatCommand());
            };

            await function.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Thermostat id cannot be empty.");
        }

        [Fact]
        public async void
            GivenNewThermostatCommand_WhenThermostatExists_ThenCreateThermostatCommandAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateThermostatCommandAsync("homeassistantgo@outlook.com",
                    Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new ThermostatCommand()
                    {
                        ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                        Temperature = (decimal) 22.5
                    });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a command for the specified thermostat in this " +
                             "schedule.");
        }

        #endregion

        #region PARTIAL_UPDATE_THERMOSTAT_COMMAND_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedThermostatCommand_WhenThermostatCommandExists_ThenPartialUpdatedThermostatCommandAsyncShouldReturnPartialUpdatedThermostatCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            JsonPatchDocument<ThermostatCommandRequest> thermostatCommandPatch =
                new JsonPatchDocument<ThermostatCommandRequest>();
            thermostatCommandPatch.Replace(t => t.Temperature, (decimal) 20.5);

            var result = await repository.PartialUpdateThermostatCommandAsync(
                "homeassistantgo@outlook.com", Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04"), thermostatCommandPatch);

            result.Should().BeOfType<ThermostatCommand>();
            result.Temperature.Should().Be((decimal) 20.5);
        }

        [Fact]
        public async void
            GivenPartialUpdatedThermostatCommand_WhenThermostatCommandDoesNotExist_ThenPartialUpdatedThermostatCommandAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            JsonPatchDocument<ThermostatCommandRequest> thermostatCommandPatch =
                new JsonPatchDocument<ThermostatCommandRequest>();
            thermostatCommandPatch.Replace(t => t.Temperature, (decimal) 20.5);

            var result = await repository.PartialUpdateThermostatCommandAsync("jane.doe@mail.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), thermostatCommandPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_THERMOSTAT_COMMAND_ASYNC

        [Fact]
        public async void
            GivenThermostatCommand_WhenThermostatCommandExists_ThenDeleteThermostatCommandAsyncShouldReturnThermostatCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.DeleteThermostatCommandAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04"));

            result.Should().BeOfType<ThermostatCommand>();
        }

        [Fact]
        public async void
            GivenThermostatCommand_WhenThermostatCommandDoesNotExist_ThenDeleteThermostatCommandAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            ThermostatCommandRepository repository = new ThermostatCommandRepository(context, mapper);

            var result = await repository.DeleteThermostatCommandAsync("jane.doe@mail.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion
    }
}