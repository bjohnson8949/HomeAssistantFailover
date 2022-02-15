using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAssistantFailover
{
    internal class Parameters
    {

        //ToDo: import config data for below most likely enviromental variables or config file
        public string? homeAssistantIP
        {
            get
            {
                return Environment.GetEnvironmentVariable("homeAssistantIP");
            }
        }
        public string? deconzIP
        {
            get
            {
                return Environment.GetEnvironmentVariable("deconzIP");
            }
        }
        public string? deconzPort
        {
            get
            {
                string? port = Environment.GetEnvironmentVariable("deconzPort");

                if (string.IsNullOrEmpty(port))
                {
                    port = "80";
                }

                return port;
            }
        }
        public string? apiKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("apiKey");
            }
        }
        public bool runAsService
        {
            get
            {
                string? runAsServiceStr = Environment.GetEnvironmentVariable("runAsService");

                if (string.IsNullOrEmpty(runAsServiceStr) && !string.IsNullOrEmpty(apiKey))
                {
                    runAsServiceStr = "true";
                }
                else if (string.IsNullOrEmpty(apiKey))
                {
                    runAsServiceStr = "false";
                }
                else
                {
                    runAsServiceStr = "true";
                }

                bool runAsService = runAsServiceStr.ToLower() == "true" ? true : false;

                return runAsService;
            }
        }

        public string? turnOnGroups
        {
            get
            {
                return Environment.GetEnvironmentVariable("turnOnGroups");
            }
        }
        public string? turnOnLights
        {
            get
            {
                return Environment.GetEnvironmentVariable("turnOnLights");
            }
        }

        public int rerunSeconds
        {
            get
            {
                int seconds;
                if (!int.TryParse(Environment.GetEnvironmentVariable("rerunSeconds"), out seconds))
                {
                    seconds = 30;
                }

                return seconds;
            }
        }

        public void printVariables()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                object? val = property.GetGetMethod().Invoke(this, null);
                string valout;

                if (val == null)
                {
                    valout = "null";
                }
                else
                {
                    valout = val.ToString();
                }

                string output = string.Format("{0}: {1}", property.Name, valout);
                Console.WriteLine(output);
            }
        }

        public bool ValidateInputs()
        {
            bool atleastOneLightToChange = (!string.IsNullOrEmpty(turnOnGroups) || !string.IsNullOrEmpty(turnOnLights));

            bool missingRequireInput =
                        string.IsNullOrEmpty(homeAssistantIP) ||
                        string.IsNullOrEmpty(deconzIP) ||
                        !atleastOneLightToChange;

            return missingRequireInput;
        }
    }
}
