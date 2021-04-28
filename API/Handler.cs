namespace API
{
    public abstract class Handler
    {
        protected readonly Identity Identity;

        protected Handler(Identity identity)
        {
            Identity = identity;
        }
    }
}