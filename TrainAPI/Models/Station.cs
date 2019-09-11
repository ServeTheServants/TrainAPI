namespace TrainAPI.Models
{
    public class Station
    {
        public Station()
        {

        }

        public Station(string station_name)
        {
            Station_name = station_name;
        }

        public int StationId { get; set; }
        public string Station_name { get; set; }
    }
}
