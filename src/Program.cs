using System.Net.NetworkInformation;
using HomeAssistantFailover;

Dictionary<string, deCONZ_REST_API.DeviceType> failoverDevices = new Dictionary<string, deCONZ_REST_API.DeviceType>();
Ping pingSender = new Ping();
bool firstRun = true;
bool serverStatus = false;

Parameters Parameters = new Parameters();


if (Parameters.ValidateInputs())
{
    throw new Exception("Missing required input files");
    //ToDo: List out fields that are required and details of formating
}

if (Parameters.turnOnGroups != null)
{
    foreach (var group in Parameters.turnOnGroups.Split("|"))
    {
        failoverDevices.Add(group, deCONZ_REST_API.DeviceType.Groups);
    }
}

if (Parameters.turnOnLights != null)
{
    foreach (var light in Parameters.turnOnLights.Split("|"))
    {
        failoverDevices.Add(light, deCONZ_REST_API.DeviceType.Lights);
    }
}

deCONZ_REST_API deCONZ = new deCONZ_REST_API(Parameters.deconzIP, Parameters.deconzPort, Parameters.apiKey);


while (Parameters.runAsService || firstRun)
{

#if DEBUG
    var pingReults = new { Status = IPStatus.BadRoute };
# else
    var pingReults = pingSender.Send(Parameters.homeAssistantIP);
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
    if (Parameters.runAsService)
    {
        Thread.Sleep(Parameters.rerunSeconds * 1000);
    }

    firstRun = false;
}
