using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;
using Shared.Models;

namespace API
{
    public class CheckoutPrices
    {
        private static CheckoutPrices mInstance;
        public Dictionary<long, UserLimit> givenUserLimits;

        protected CheckoutPrices()
        {
            givenUserLimits = new Dictionary<long, UserLimit>();
            //TODO make them actually have a logic
            //TODO front end could read this values and show them in Checkout
            givenUserLimits.Add(500, new UserLimit(20, 20, 40, 40, 40, 20, 20));
            givenUserLimits.Add(2000, new UserLimit(0, 0, 0, 0, 0, 0, 0));
            givenUserLimits.Add(10000, new UserLimit(200, 20, 40, 40, 40, 20, 20));
        }

        public static CheckoutPrices getInstance()
        {
            if (mInstance == null)
            {
                mInstance = new CheckoutPrices();
            }

            return mInstance;
        }
    }
}