using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainAPI.Helpers
{
    public class RequestJSON
    {
        public RequestJSON(string date, string id, string remoteip, string url, string method, object requestbody)
        {
            Date = date;
            ID = id;
            RemoteIP = remoteip;
            Url = url;
            Method = method;
            RequestBody = requestbody;
        }
        public string Date { get; set; }
        
        public string ID { get; set; }

        public string RemoteIP { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }

        public object RequestBody { get; set; }
    }
}
