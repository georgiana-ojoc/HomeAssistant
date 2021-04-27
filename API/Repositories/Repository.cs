using Shared.Models;

namespace API.Repositories
{
    public abstract class Repository
    {
        protected readonly HomeAssistantContext Context;

        protected Repository(HomeAssistantContext context)
        {
            Context = context;
        }
    }
}