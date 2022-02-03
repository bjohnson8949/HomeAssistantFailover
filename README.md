# Home Assistant Failover - Beta


**The app is still in its early stages and has basic functionality.**


## Docker Enviromental Variables

| Key Name        | Requried | Description                                                        |
| --------------- | -------- | ------------------------------------------------------------------ |
| HomeAssistantIP | True     | Used to ping if the server is up                                   |
| deconzIP        | True     | IP of deCONZ server to connect                                     |
| deconzPort      | False    | deCONZ port used for connection and will default to 80             |
| apiKey          | False    | If you don't have API Key you will have to register app in phoscon |
| rerunSeconds    | True     | How frequenct to ping server to see if it is down                  |
| runAsService    | False    | App will test once and closed mainly for debugging                 |


## How to pair to phoscon
From the management UI for phoscon browse to gateway  
At the bottom center clicked advanced  
Click authenticate app


Run `Docker logs ha-failover` this will allow you to see your api key  
In the future I will add this to auto save


## Getting started

Run Pairing process above and then start container
```
docker pull bjohnson8949/home-assistant-failover:latest
docker run --name ha-failover -e HomeAssistantIP=ip.address -e deconzIP=ip.address -e rerunSeconds=15 -d bjohnson8949/home-assistant-failover:latest
```
