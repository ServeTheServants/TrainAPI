using System.ComponentModel.DataAnnotations;

namespace TrainAPI.Models
{
    public class Schedule
    {
        public Schedule()
        {

        }

        public Schedule(Route route, Station station, int stationNumberInRoute, int? arrivingDay, int? arrivingHour, int? arrivingMinute, int? dispatchingDay, int? dispatchingHour, int? dispatchingMinute)
        {
            Route = route;
            Station = station;
            StationNumberInRoute = stationNumberInRoute;
            ArrivingDay = arrivingDay;
            ArrivingHour = arrivingHour;
            ArrivingMinute = arrivingMinute;
            DispatchingDay = dispatchingDay;
            DispatchingHour = dispatchingHour;
            DispatchingMinute = dispatchingMinute;
        }

        public int ScheduleId { get; set; }

        [Range(1, 7)]
        public int? ArrivingDay { get; set; }
        [Range(0, 23)]
        public int? ArrivingHour { get; set; }
        [Range(0, 59)]
        public int? ArrivingMinute { get; set; }

        [Range(1, 7)]
        public int? DispatchingDay { get; set; }
        [Range(0, 23)]
        public int? DispatchingHour { get; set; }
        [Range(0, 59)]
        public int? DispatchingMinute { get; set; }

        public int RouteId { get; set; }
        public virtual Route Route { get; set; }

        public int StationId { get; set; }
        public Station Station { get; set; }
        [Range(1, int.MaxValue)]
        public int StationNumberInRoute { get; set; }
    }
}
