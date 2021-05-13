using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared
{
    public class Helper
    {
        private readonly HomeAssistantContext _context;

        public Helper(HomeAssistantContext context)
        {
            _context = context;
        }

        public static string GetCronExpression(string time, byte days)
        {
            if (!TimeSpan.TryParse(time, CultureInfo.InvariantCulture, out TimeSpan newTime))
            {
                return null;
            }

            if (days is 0 or 255)
            {
                return null;
            }

            StringBuilder result = new StringBuilder($"{newTime.Minutes} {newTime.Hours} * * ");
            int position = 1;
            while (days != 0)
            {
                if (days % 2 == 1)
                {
                    result.Append($"{position},");
                }

                position++;
                days /= 2;
            }

            result = result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public void Change(Guid id)
        {
            Task.Run(async () => await ChangeAsync(id));
        }

        private async Task<bool> ChangeAsync(Guid id)
        {
            await ChangeLightBulbsAsync(id);
            await ChangeDoorsAsync(id);
            await ChangeThermostatsAsync(id);
            return true;
        }

        public async Task ChangeLightBulbsAsync(Guid id)
        {
            IEnumerable<LightBulbCommand> lightBulbCommands = await _context.LightBulbCommands
                .Where(lightBulbCommand => lightBulbCommand.ScheduleId == id).ToListAsync();
            foreach (LightBulbCommand lightBulbCommand in lightBulbCommands)
            {
                LightBulb lightBulb = await _context.LightBulbs.FirstOrDefaultAsync(lb => lb.Id ==
                    lightBulbCommand.LightBulbId);
                if (lightBulb == null)
                {
                    continue;
                }

                lightBulb.Color = lightBulbCommand.Color;
                lightBulb.Intensity = lightBulbCommand.Intensity;
                _context.LightBulbs.Update(lightBulb);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ChangeDoorsAsync(Guid id)
        {
            IEnumerable<DoorCommand> doorCommands = await _context.DoorCommands
                .Where(doorCommand => doorCommand.ScheduleId == id).ToListAsync();
            foreach (DoorCommand doorCommand in doorCommands)
            {
                Door door = await _context.Doors.FirstOrDefaultAsync(d => d.Id == doorCommand.DoorId);
                if (door == null)
                {
                    continue;
                }

                door.Locked = doorCommand.Locked;
                _context.Doors.Update(door);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ChangeThermostatsAsync(Guid id)
        {
            IEnumerable<ThermostatCommand> thermostatCommands = await _context.ThermostatCommands
                .Where(thermostatCommand => thermostatCommand.ScheduleId == id).ToListAsync();
            foreach (ThermostatCommand thermostatCommand in thermostatCommands)
            {
                Thermostat thermostat = await _context.Thermostats.FirstOrDefaultAsync(t => t.Id ==
                    thermostatCommand.ThermostatId);
                if (thermostat == null)
                {
                    continue;
                }

                thermostat.Temperature = thermostatCommand.Temperature;
                _context.Thermostats.Update(thermostat);
                await _context.SaveChangesAsync();
            }
        }
    }
}