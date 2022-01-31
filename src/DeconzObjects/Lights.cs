using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeAssistantFailover.DeconzObjects.Light
{ 
    public class Pointsymbol
    {
    }

    public class State
    {
        public string alert { get; set; }
        public int bri { get; set; }
        public string effect { get; set; }
        public bool on { get; set; }
        public bool reachable { get; set; }
    }

    public class Root
    {
        public string etag { get; set; }
        public bool hascolor { get; set; }
        public string manufacturer { get; set; }
        public string modelid { get; set; }
        public string name { get; set; }
        public Pointsymbol pointsymbol { get; set; }
        public State state { get; set; }
        public string swversion { get; set; }
        public string type { get; set; }
        public string uniqueid { get; set; }
    }
}
