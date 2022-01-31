using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAssistantFailover
{
    public class deCONZ_REST_API
    {
        //Rest service connection info
        private string serverip { get; set; }
        private string serverport { get; set; }
        private string apiKey { get; set; }

        public deCONZ_REST_API(string ip, string port, string? key)
        {
            serverip = ip;
            serverport = port;

            //If we don't have api key register with deconz
            if (!string.IsNullOrEmpty(key))
            {
                apiKey = key;
            }
            else
            {
                Task.Run(() => this.register()).Wait();
            }
        }

        //Process to get api key from deCONZ https://dresden-elektronik.github.io/deconz-rest-doc/endpoints/configuration/#parameters
        public async Task register()
        {
            string registeredKey = string.Empty;
            string urlPath = string.Format("http://{0}:{1}/api", serverip, serverport);
            string payload = "{ \"devicetype\": \"HomeAssistantFailover\" }";

            var getData = http_helpers.Post(urlPath, payload);
            getData.Wait();

            var myDeserializedClass = HomeAssistantFailover.DeconzObjects.Register.Register.FromJson(getData.Result);
            apiKey = myDeserializedClass.First().Success.Username.Trim();

            Console.WriteLine(apiKey);
            //ToDo: save this API Key
        }

        //This get device id for groups and lights 
        //group - https://dresden-elektronik.github.io/deconz-rest-doc/endpoints/groups/#get-all-groups
        //lights - https://dresden-elektronik.github.io/deconz-rest-doc/endpoints/lights/#get-all-lights
        private int? GetID(string name, DeviceType type)
        {
            int? deviceID = null;

            string url = urlBuilder(type, ActionType.Get);
            var getData = http_helpers.Get(url);
            getData.Wait();

            var json = JObject.Parse(getData.Result);
            foreach (var entry in json)
            {
                dynamic search_group;

                int id;
                if (int.TryParse(entry.Key, out id))
                {
                    switch (type)
                    {
                        case DeviceType.Lights:
                            search_group = json[id.ToString()].ToObject<HomeAssistantFailover.DeconzObjects.Light.Root>();
                            break;
                        case DeviceType.Groups:
                            search_group = json[id.ToString()].ToObject<HomeAssistantFailover.DeconzObjects.Group.Root>();
                            break;
                        default:
                            search_group = null;
                            break;
                    }

                    if (search_group != null && search_group.name.ToLower() == name.ToLower())
                    {
                        deviceID = id;
                    }
                }
            }

            return deviceID;
        }

        //Turn on lights\groups
        //https://dresden-elektronik.github.io/deconz-rest-doc/endpoints/groups/#set-group-attributes
        //https://dresden-elektronik.github.io/deconz-rest-doc/endpoints/lights/#set-light-state
        public bool TurnOn(Dictionary<string, DeviceType> Devices)
        {
            bool turnOn = false;
            int? deviceID = null;

            foreach (var name in Devices.Keys)
            {
                DeviceType type = Devices[name];

                deviceID = GetID(name, type);

                if (deviceID != null)
                {
                    string url = urlBuilder(type, ActionType.Set, deviceID.ToString());
                    string payload = getStatePayload(State.On);

                    http_helpers.Put(url, payload).Wait();
                    //ToDo: parse lights turned on
                }
            }

            return turnOn;
        }

        private string? urlBuilder(DeviceType type, ActionType action, string? objectID = null)
        {
            string? url = null;
            EndpointInfo? info = getGroupNameAndEndpoint(type, action);

            if (info != null)
            {
                switch (action)
                {
                    case ActionType.Set:
                        url = string.Format("http://{0}:{1}/api/{2}/{3}/{4}/{5}", serverip, serverport, apiKey, info.Type, objectID, info.Endpoint);
                        break;
                    case ActionType.Get:
                        url = string.Format("http://{0}:{1}/api/{2}/{3}", serverip, serverport, apiKey, info.Type);
                        break;
                    default:
                        url = string.Format("http://{0}:{1}/api/{2}/", serverip, serverport, apiKey);
                        break;
                }
            }

            return url;
        }

        private class EndpointInfo
        {
            public EndpointInfo(string type, string? endpoint = null)
            {
                Type = type;
                Endpoint = endpoint;
            }

            public string Type { get; set; }
            public string? Endpoint { get; set; }
        }

        private EndpointInfo? getGroupNameAndEndpoint(DeviceType type, ActionType action)
        {
            EndpointInfo? info = null;

            if (action == ActionType.Set)
            {
                switch (type)
                {
                    case DeviceType.Lights:
                        info = new EndpointInfo("lights", "state");
                        break;
                    case DeviceType.Groups:
                        info = new EndpointInfo("groups", "action");
                        break;
                    default:
                        break;
                }
            }
            else if (action == ActionType.Get)
            {
                switch (type)
                {
                    case DeviceType.Lights:
                        info = new EndpointInfo("lights");
                        break;
                    case DeviceType.Groups:
                        info = new EndpointInfo("groups");
                        break;
                    default:
                        break;
                }
            }


            return info;
        }

        private string getStatePayload(State state)
        {
            string payload = string.Empty;

            switch (state)
            {
                case State.On:
                    payload = "{ \"on\": true, \"bri\": 255 }";
                    break;
                case State.Off:
                    payload = "{ \"on\": false }";
                    break;
                default:
                    break;
            }

            return payload;
        }

        public enum State
        {
            On,
            Off
        }

        public enum DeviceType
        {
            Lights,
            Groups
        }

        public enum ActionType
        {
            Get,
            Set
        }

    }
}
