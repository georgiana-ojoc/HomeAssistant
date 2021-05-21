using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Shared;
using Shared.Models;
using Shared.Requests;
using Shared.Responses;
using Xunit;

namespace Tests.RepositoryTests
{
    public class DoorCommandRepositoryTest : BaseRepositoryTest
    {
        #region GET_DOOR_COMMAND_ASYNC

        [Fact]
        public async void
            GivenScheduleId_WhenScheduleIdExists_ThenGetDoorCommandsAsyncShouldReturnListOfDoorCommands()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.GetDoorCommandsAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"));

            result.Should().BeOfType<List<DoorCommandResponse>>();
        }

        [Fact]
        public async void GivenScheduleId_WhenScheduleIdDoesNotExist_ThenGetDoorCommandsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.GetDoorCommandsAsync("homeassistantgo@outlook.com",
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void
            GivenScheduleId_WhenScheduleIdIsEmpty_ThenGetDoorCommandsAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetDoorCommandsAsync("homeassistantgo@outlook.com",
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Schedule id cannot be empty.");
        }

        #endregion

        #region GET_DOOR_COMMAND_BY_ID_ASYNC

        [Fact]
        public async void
            GivenDoorCommand_WhenDoorCommandExists_ThenGetDoorCommandByIdAsyncShouldReturnDoorCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.GetDoorCommandByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b"));

            result.Should().BeOfType<DoorCommand>();
        }

        [Fact]
        public async void
            GivenDoorCommand_WhenDoorCommandDoesNotExist_ThenGetDoorCommandByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.GetDoorCommandByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_DOOR_COMMAND_ASYNC

        [Fact]
        public async void
            GivenNewDoorCommand_WhenDoorCommandIsNotEmpty_ThenCreateDoorCommandAsyncShouldReturnNewDoorCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.CreateDoorCommandAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new DoorCommand()
                {
                    DoorId = Guid.Parse("3968c3e5-daee-4096-a6d4-11b640216591"),
                    Locked = true
                });

            result.Should().BeOfType<DoorCommand>();
        }

        [Fact]
        public async void
            GivenNewDoorCommand_WhenDoorCommandIsEmpty_ThenCreateDoorCommandAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateDoorCommandAsync("homeassistantgo@outlook.com",
                    Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new DoorCommand());
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Door id cannot be empty.");
        }

        [Fact]
        public async void
            GivenNewDoorCommand_WhenDoorExists_ThenCreateDoorCommandAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateDoorCommandAsync("homeassistantgo@outlook.com",
                    Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new DoorCommand()
                    {
                        DoorId = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba")
                    });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a command for the specified door in this schedule.");
        }

        #endregion

        #region PARTIAL_UPDATE_DOOR_COMMAND_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedDoorCommand_WhenDoorCommandExists_ThenPartialUpdatedDoorCommandAsyncShouldReturnPartialUpdatedDoorCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            JsonPatchDocument<DoorCommandRequest> doorCommandPatch =
                new JsonPatchDocument<DoorCommandRequest>();
            doorCommandPatch.Replace(d => d.Locked, false);

            var result = await repository.PartialUpdateDoorCommandAsync(
                "homeassistantgo@outlook.com", Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b"), doorCommandPatch);

            result.Should().BeOfType<DoorCommand>();
            result.Locked.Should().Be(false);
        }

        [Fact]
        public async void
            GivenPartialUpdatedDoorCommand_WhenDoorCommandDoesNotExist_ThenPartialUpdatedDoorCommandAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            JsonPatchDocument<DoorCommandRequest> doorCommandPatch =
                new JsonPatchDocument<DoorCommandRequest>();
            doorCommandPatch.Replace(d => d.Locked, false);

            var result = await repository.PartialUpdateDoorCommandAsync("jane.doe@mail.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), doorCommandPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_DOOR_COMMAND_ASYNC

        [Fact]
        public async void
            GivenDoorCommand_WhenDoorCommandExists_ThenDeleteDoorCommandAsyncShouldReturnDoorCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.DeleteDoorCommandAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b"));

            result.Should().BeOfType<DoorCommand>();
        }

        [Fact]
        public async void
            GivenDoorCommand_WhenDoorCommandDoesNotExist_ThenDeleteDoorCommandAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorCommandRepository repository = new DoorCommandRepository(context, mapper);

            var result = await repository.DeleteDoorCommandAsync("jane.doe@mail.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion
    }
}