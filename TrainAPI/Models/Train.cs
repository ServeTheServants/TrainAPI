using System.Collections.Generic;

namespace TrainAPI.Models
{
    public class Train
    {
        public Train()
        {

        }

        public Train(string train_name)
        {
            Train_Name = train_name;
        }

        public int TrainId { get; set; }

        public string Train_Name { get; set; }

        public List<Route> Routes { get; set; }
    }
}
