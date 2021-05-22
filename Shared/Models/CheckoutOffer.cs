using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class CheckoutOffer : BaseModel
    {
        [Required] public String OfferName { get; set; }
        [Required] public String OfferDescription { get; set; }

        internal ICollection<UserCheckoutOffer> UserCheckoutOffers { get; set; }
        [Required] public int OfferValue { get; set; }
        [Required] public int HouseLimit { get; set; }
        [Required] public int RoomLimit { get; set; }
        [Required] public int LightBulbLimit { get; set; }
        [Required] public int DoorLimit { get; set; }
        [Required] public int ThermostatLimit { get; set; }
        [Required] public int ScheduleLimit { get; set; }
        [Required] public int CommandLimit { get; set; }

        public List<KeyValuePair<String, String>> GetCustomEnumerator()
        {
            List<KeyValuePair<String, String>> list = new();
            list.Add(new KeyValuePair<String, String>("House limit : ", HouseLimit.ToString()));
            list.Add(new KeyValuePair<String, String>("Room limit : ", RoomLimit.ToString()));
            list.Add(new KeyValuePair<String, String>("Light bulb limit : ", LightBulbLimit.ToString()));
            list.Add(new KeyValuePair<String, String>("Door limit : ", DoorLimit.ToString()));
            list.Add(new KeyValuePair<String, String>("Thermostat limit : ", ThermostatLimit.ToString()));
            list.Add(new KeyValuePair<String, String>("Schedule limit : ", ScheduleLimit.ToString()));
            list.Add(new KeyValuePair<String, String>("Command limit : ", CommandLimit.ToString()));
            return list;
        }
    }
}