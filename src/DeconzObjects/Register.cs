using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeAssistantFailover.DeconzObjects.Register
{
    public partial class Register
    {
        [JsonProperty("success")]
        public Success Success { get; set; }
    }

    public partial class Success
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public partial class Register
    {
        public static Register[] FromJson(string json) => JsonConvert.DeserializeObject<Register[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Register[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
