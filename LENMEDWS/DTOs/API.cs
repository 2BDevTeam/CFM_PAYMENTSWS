﻿
using Newtonsoft.Json;

namespace LENMEDWS.DTOs
{
    public class API
    {

 
        public string entity { get; set; }
        public string message { get; set; }

        public string status { get; set; }
       
        public List<Endpoint> endpoints { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
