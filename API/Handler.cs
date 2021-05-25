using System;

namespace API
{
    public abstract class Handler
    {
        protected readonly Identity Identity;

        protected Handler(Identity identity)
        {
            Identity = identity;
        }
        
        protected void CheckEmail()
        {
            if (!Identity.Email.Equals("homeassistantgo@outlook.com"))
            {
                throw new ArgumentException("You are not the administrator.");
            }
        }
    }
}