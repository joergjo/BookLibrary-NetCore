## Running on a Docker Host with Docker Compose
If you have a Docker host running or Docker locally installed on your machine, you don't even need the .NET Core runtime, SDK or Visual Studio 2017, nor an existing MongoDB host. Note that the included Compose files are meant for a single Docker host, not a Swarm cluster.

Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

### Windows 10 (Windows containers)
```   
$ cd \path\to\BookLibrary-NetCore
$ docker-compose -f docker-compose.yml -f docker-compose.override.windows.yml up
```

When running as Windows containers, you will need to use the NATted IP address of the `booklibrarynetcore_webapi_1` container to access the app. You can get this IP address by running 
```
docker inspect -f "{{ .NetworkSettings.Networks.booklibrarynetcore_default.IPAddress }}" <container-id>
``` 

Then open `http://<Container-IP>`. 

### Windows 10 (Linux containers)

```
$ cd \path\to\BookLibrary-NetCore
$ docker-compose up
```

Launch your web browser and open `http://localhost:5000`.
### Linux, macOS
```  
$ cd /path/to/BookLibrary-NetCore
$ docker-compose up
```  

Launch your web browser and open `http://localhost:5000`.