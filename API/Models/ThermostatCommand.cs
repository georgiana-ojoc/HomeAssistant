#nullable disable

namespace API.Models
{
    public class ThermostatCommand
    {
        public int Id { get; set; }
        public int ThermostatId { get; set; }
        public int ScheduleId { get; set; }
        public decimal Temperature { get; set; }

        public Thermostat Thermostat { get; set; }
        public Schedule Schedule { get; set; }
    }
}