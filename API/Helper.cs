using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Models;

namespace API
{
    public static class Helper
    {
        public static string GetCronExpression(TimeSpan time, byte days)
        {
            if (days is 0 or 255)
            {
                return null;
            }

            string result = $"{time.Minutes} {time.Hours} * * ";
            int position = 1;
            while (days != 0)
            {
                if (days % 2 == 1)
                {
                    result += $"{position},";
                }

                position++;
                days /= 2;
            }

            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        public static async Task<Boolean> ChangeAsync(Guid scheduleId)
        {
            //TO DO
            try
            {
                string baseUrl = "https://localhost:5001/schedules/{scheduleId}";
            string doorCommandUrl = baseUrl+"/door_commands";
            string lightBulbCommandUrl = baseUrl+"/light_bulb_commands";
            string thermostatCommandUrl = baseUrl+"/thermostat_commands";
            HttpClient client = new HttpClient();
            
            HttpResponseMessage doorResponse = await client.GetAsync(doorCommandUrl);
            HttpResponseMessage lightBulbResponse = await client.GetAsync(lightBulbCommandUrl);
            HttpResponseMessage thermostatResponse = await client.GetAsync(thermostatCommandUrl);

            IEnumerable<DoorCommand> doorCommands = await doorResponse.Content.
                ReadFromJsonAsync<IEnumerable<DoorCommand>>();
            
            IEnumerable<LightBulbCommand>  lightBulbCommands = await lightBulbResponse.Content.
                ReadFromJsonAsync<IEnumerable<LightBulbCommand>>();
            
            IEnumerable<ThermostatCommand> thermostatCommands = await thermostatResponse.Content.
                ReadFromJsonAsync<IEnumerable<ThermostatCommand>>();


            if (doorCommands != null)
                foreach (DoorCommand doorCommand in doorCommands)
                {
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(new Dictionary<string, string>()
                    {
                        {"operation", "replace"},
                        {"path", "locked"},
                        {"value", doorCommand.Locked.ToString()}
                    });
                    string serializedObject = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PatchAsync(doorCommandUrl+"/"+doorCommand.Id,
                        patchBody);
                }

            if (lightBulbCommands != null)
                foreach (LightBulbCommand lightBulbCommand in lightBulbCommands)
                {
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(new Dictionary<string, string>()
                    {
                        {"operation", "replace"},
                        {"path", "intensity"},
                        {"value", lightBulbCommand.Intensity.ToString()}
                    });
                    patchList.Add(new Dictionary<string, string>()
                    {
                        {"operation", "replace"},
                        {"path", "color"},
                        {"value", lightBulbCommand.Color.ToString()}
                    });
                    string serializedObject = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PatchAsync(lightBulbCommandUrl+"/"+lightBulbCommand.Id
                        ,patchBody);
                    
                }

            if (thermostatCommands != null)
                foreach (ThermostatCommand thermostatCommand in thermostatCommands)
                {
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(new Dictionary<string, string>()
                    {
                        {"operation", "replace"},
                        {"path", "temperature"},
                        {"value", thermostatCommand.Temperature.ToString()}
                    });
                    string serializedObject = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PatchAsync(doorCommandUrl+"/"+thermostatCommand.Id,
                        patchBody);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            return true;
        }
        public static void ChangeAllInSchedule(Guid scheduleId)
        {
            Task.Run<Boolean>(async () => await ChangeAsync(scheduleId));

        }
    }
}