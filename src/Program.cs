using System.Net.NetworkInformation;
using HomeAssistantFailover;

Dictionary<string, deCONZ_REST_API.DeviceType> failoverDevices = new Dictionary<string, deCONZ_REST_API.DeviceType>();
Ping pingSender = new Ping();
bool firstRun = true;
bool serverStatus = false;

bool missingRequireInput = false;

//ToDo: import config data for below most likely enviromental variables or config file
string? homeAssistantIP = Environment.GetEnvironmentVariable("HomeAssistantIP");
string? deconzIP = Environment.GetEnvironmentVariable("deconzIP"); 
string? deconzPort = Environment.GetEnvironmentVariable("deconzPort");
string? apiKey = Environment.GetEnvironmentVariable("apiKey");
int rerunSeconds;

missingRequireInput = !int.TryParse(Environment.GetEnvironmentVariable("rerunSeconds"), out rerunSeconds) || 
                        string.IsNullOrEmpty(homeAssistantIP) || 
                        string.IsNullOrEmpty(deconzIP) || 
                        string.IsNullOrEmpty(deconzPort) || 
                        string.IsNullOrEmpty(deconzPort) || 
                        string.IsNullOrEmpty(Environment.GetEnvironmentVariable("runAsService"));

if(missingRequireInput)
{
    throw new Exception("Missing required input files");
    //ToDo: List out fields that are required and details of formating
}

bool runAsService = Environment.GetEnvironmentVariable("runAsService").ToLower() == "true" ? true : false;

//ToDo: load this from a config
failoverDevices.Add("Office Desk Corner Lamp", deCONZ_REST_API.DeviceType.Lights);
//failoverDevices.Add("Basement", deCONZ_REST_API.DeviceType.Groups);

deCONZ_REST_API deCONZ = new deCONZ_REST_API(deconzIP, deconzPort, apiKey);


while (runAsService || firstRun)
{
    
                     
#if DEBUG
    var pingReults = new { Status = IPStatus.BadRoute };
# else
    var pingReults = pingSender.Send(homeAssistantIP);
#endif

    if (pingReults.Status != IPStatus.Success)
    {
        //call  lights on event
        deCONZ.TurnOn(failoverDevices);
    }
    else if (pingReults.Status == IPStatus.Success && serverStatus == false)
    {
        serverStatus = true;
        //turn lights back off -> call home assistant webhook at let it decide what to do
    }

    //Rerun every 30 seconds
    if (runAsService)
    {
        Thread.Sleep(rerunSeconds * 1000);
    }

    firstRun = false;
}
