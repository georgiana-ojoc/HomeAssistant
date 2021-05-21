using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Requests;

namespace API.Commands.UserLimit
{
    public class PartialUpdateUserLimitCommand : IRequest<Shared.Models.UserLimit>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<UserLimitRequest> Patch { get; set; }
    }
}