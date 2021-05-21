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
    public class LightBulbCommandRepositoryTest : BaseRepositoryTest
    {
        #region GET_LIGHT_BULB_COMMAND_ASYNC

        [Fact]
        public async void
            GivenScheduleId_WhenScheduleIdExists_ThenGetLightBulbCommandsAsyncShouldReturnListOfLightBulbCommands()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.GetLightBulbCommandsAsync(
                "homeassistantgo@outlook.com", Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"));

            result.Should().BeOfType<List<LightBulbCommandResponse>>();
        }

        [Fact]
        public async void GivenScheduleId_WhenScheduleIdDoesNotExist_ThenGetLightBulbCommandsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.GetLightBulbCommandsAsync(
                "homeassistantgo@outlook.com", Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void
            GivenScheduleId_WhenScheduleIdIsEmpty_ThenGetLightBulbCommandsAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetLightBulbCommandsAsync("homeassistantgo@outlook.com",
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Schedule id cannot be empty.");
        }

        #endregion

        #region GET_LIGHT_BULB_COMMAND_BY_ID_ASYNC

        [Fact]
        public async void
            GivenLightBulbCommand_WhenLightBulbCommandExists_ThenGetLightBulbCommandByIdAsyncShouldReturnLightBulbCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.GetLightBulbCommandByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("31048472-de9f-427e-b7af-7a3416928652"));

            result.Should().BeOfType<LightBulbCommand>();
        }

        [Fact]
        public async void
            GivenLightBulbCommand_WhenLightBulbCommandDoesNotExist_ThenGetLightBulbCommandByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.GetLightBulbCommandByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_LIGHT_BULB_COMMAND_ASYNC

        [Fact]
        public async void
            GivenNewLightBulbCommand_WhenLightBulbCommandIsNotEmpty_ThenCreateLightBulbCommandAsyncShouldReturnNewLightBulbCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.CreateLightBulbCommandAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new LightBulbCommand()
                {
                    LightBulbId = Guid.Parse("0365f802-bb3a-487a-997b-0d34c270a385"),
                    Color = 6556210,
                    Intensity = 50
                });

            result.Should().BeOfType<LightBulbCommand>();
        }

        [Fact]
        public async void
            GivenNewLightBulbCommand_WhenLightBulbCommandIsEmpty_ThenCreateLightBulbCommandAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateLightBulbCommandAsync("homeassistantgo@outlook.com",
                    Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new LightBulbCommand());
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Light bulb id cannot be empty.");
        }

        [Fact]
        public async void
            GivenNewLightBulbCommand_WhenLightBulbExists_ThenCreateLightBulbCommandAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateLightBulbCommandAsync("homeassistantgo@outlook.com",
                    Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), new LightBulbCommand()
                    {
                        LightBulbId = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a")
                    });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a command for the specified light bulb in this " +
                             "schedule.");
        }

        #endregion

        #region PARTIAL_UPDATE_LIGHT_BULB_COMMAND_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedLightBulbCommand_WhenLightBulbCommandExists_ThenPartialUpdatedLightBulbCommandAsyncShouldReturnPartialUpdatedLightBulbCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            JsonPatchDocument<LightBulbCommandRequest> lightBulbCommandPatch =
                new JsonPatchDocument<LightBulbCommandRequest>();
            lightBulbCommandPatch.Replace(lb => lb.Intensity, 100);

            var result = await repository.PartialUpdateLightBulbCommandAsync(
                "homeassistantgo@outlook.com", Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("31048472-de9f-427e-b7af-7a3416928652"), lightBulbCommandPatch);

            result.Should().BeOfType<LightBulbCommand>();
            result.Intensity.Should().Be(100);
        }

        [Fact]
        public async void
            GivenPartialUpdatedLightBulbCommand_WhenLightBulbCommandDoesNotExist_ThenPartialUpdatedLightBulbCommandAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            JsonPatchDocument<LightBulbCommandRequest> lightBulbCommandPatch =
                new JsonPatchDocument<LightBulbCommandRequest>();
            lightBulbCommandPatch.Replace(lb => lb.Intensity, 100);

            var result = await repository.PartialUpdateLightBulbCommandAsync("jane.doe@mail.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), lightBulbCommandPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_LIGHT_BULB_COMMAND_ASYNC

        [Fact]
        public async void
            GivenLightBulbCommand_WhenLightBulbCommandExists_ThenDeleteLightBulbCommandAsyncShouldReturnLightBulbCommand()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.DeleteLightBulbCommandAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("31048472-de9f-427e-b7af-7a3416928652"));

            result.Should().BeOfType<LightBulbCommand>();
        }

        [Fact]
        public async void
            GivenLightBulbCommand_WhenLightBulbCommandDoesNotExist_ThenDeleteLightBulbCommandAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbCommandRepository repository = new LightBulbCommandRepository(context, mapper);

            var result = await repository.DeleteLightBulbCommandAsync("jane.doe@mail.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion
    }
}