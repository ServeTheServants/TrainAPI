using Newtonsoft.Json.Linq;

namespace TrainAPI.Helpers
{
    public class ResponseJSON
    {
        public ResponseJSON(string date, string id, string remoteip, int statuscode, object responsebody)
        {
            Date = date;
            ID = id;
            RemoteIP = remoteip;
            StatusCode = statuscode;
            ResponseBody = responsebody;
        }
        public string Date { get; set; }
        public string ID { get; set; }

        public string RemoteIP { get; set; }

        public int StatusCode { get; set; }
        public object ResponseBody { get; set; }
    }
}
