using System;
using AutoMapper;
using Shared.Models;

namespace API.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly HomeAssistantContext Context;
        protected readonly IMapper Mapper;

        protected BaseRepository(HomeAssistantContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected void CheckString(string field, string name)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentNullException(name);
            }
        }

        protected void CheckGuid(Guid field, string name)
        {
            if (field == Guid.Empty)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}