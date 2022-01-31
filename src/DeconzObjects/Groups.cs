using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAssistantFailover.DeconzObjects.Group
{
    public class Action
    {
        public string alert { get; set; }
        public int bri { get; set; }
        public string colormode { get; set; }
        public int ct { get; set; }
        public string effect { get; set; }
        public int hue { get; set; }
        public bool on { get; set; }
        public int sat { get; set; }
        public object scene { get; set; }
        public List<int> xy { get; set; }
    }

    public class State
    {
        public bool all_on { get; set; }
        public bool any_on { get; set; }
    }

    public class Root
    {
        public Action action { get; set; }
        public List<object> devicemembership { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public List<string> lights { get; set; }
        public string name { get; set; }
        public List<object> scenes { get; set; }
        public State state { get; set; }
        public string type { get; set; }
    }
}
