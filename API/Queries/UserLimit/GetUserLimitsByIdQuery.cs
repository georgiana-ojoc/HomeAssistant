using System;
using MediatR;

namespace API.Queries.UserLimit
{
    public class GetUserLimitByIdQuery : IRequest<Shared.Models.UserLimit>
    {
        public Guid Id { get; set; }
    }
}