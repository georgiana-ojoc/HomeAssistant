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
    public class LightBulbRepositoryTest : BaseRepositoryTest
    {
        #region GET_LIGHT_BULB_ASYNC

        [Fact]
        public async void GivenRoomId_WhenRoomIdExists_ThenGetLightBulbsAsyncShouldReturnListOfLightBulbs()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.GetLightBulbsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"));

            result.Should().BeOfType<List<LightBulb>>();
        }

        [Fact]
        public async void GivenRoomId_WhenRoomIdDoesNotExist_ThenGetLightBulbsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.GetLightBulbsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenRoomId_WhenRoomIdIsEmpty_ThenGetLightBulbsAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetLightBulbsAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'room_id')");
        }

        #endregion

        #region GET_LIGHT_BULB_BY_ID_ASYNC

        [Fact]
        public async void GivenLightBulb_WhenLightBulbExists_ThenGetLightBulbByIdAsyncShouldReturnLightBulb()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.GetLightBulbByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"));

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenLightBulb_WhenLightBulbDoesNotExist_ThenGetLightBulbByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.GetLightBulbByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_LIGHT_BULB_ASYNC

        [Fact]
        public async void GivenNewLightBulb_WhenLightBulbIsNotEmpty_ThenCreateLightBulbAsyncShouldReturnNewLightBulb()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.CreateLightBulbAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new LightBulb()
                {
                    Name = "Chandelier"
                });

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void
            GivenNewLightBulb_WhenLightBulbIsEmpty_ThenCreateLightBulbAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateLightBulbAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new LightBulb());
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'name')");
        }

        #endregion

        #region PARTIAL_UPDATE_LIGHT_BULB_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedLightBulb_WhenLightBulbExists_ThenPartialUpdatedLightBulbAsyncShouldReturnPartialUpdatedLightBulb()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            JsonPatchDocument<LightBulbRequest> lightBulbPatch = new JsonPatchDocument<LightBulbRequest>();
            lightBulbPatch.Replace(lb => lb.Intensity, (byte) 255);

            var result = await repository.PartialUpdateLightBulbAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"), lightBulbPatch);

            result.Should().BeOfType<LightBulb>();
            result.Intensity.Should().Be(255);
        }

        [Fact]
        public async void
            GivenPartialUpdatedLightBulb_WhenLightBulbDoesNotExist_ThenPartialUpdatedLightBulbAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            JsonPatchDocument<LightBulbRequest> lightBulbPatch = new JsonPatchDocument<LightBulbRequest>();
            lightBulbPatch.Replace(lb => lb.Intensity, (byte) 255);

            var result = await repository.PartialUpdateLightBulbAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), lightBulbPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_LIGHT_BULB_ASYNC

        [Fact]
        public async void GivenLightBulb_WhenLightBulbExists_ThenDeleteLightBulbAsyncShouldReturnLightBulb()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.DeleteLightBulbAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"));

            result.Should().BeOfType<LightBulb>();
        }

        [Fact]
        public async void GivenLightBulb_WhenLightBulbDoesNotExist_ThenDeleteLightBulbAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            LightBulbRepository repository = new LightBulbRepository(context, mapper);

            var result = await repository.DeleteLightBulbAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"));

            result.Should().BeNull();
        }

        #endregion
    }
}