using Shared.Models;

namespace API.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly HomeAssistantContext Context;

        protected BaseRepository(HomeAssistantContext context)
        {
            Context = context;
        }
    }
}