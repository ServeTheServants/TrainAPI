using System.Collections.Generic;

namespace TrainAPI.Models
{
    public class Route
    {
        public Route()
        {

        }

        public Route(Train train)
        {
            Train = train;
        }

        public int RouteId { get; set; }
        public List<Schedule> Schedules { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }
    }
}
