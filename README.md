# Book Library
This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](http://backbonejs.org/) with the following modifications and enhancements:
- The Backbone SPA is now bundled and minified using [gulp](http://gulpjs.com/). Otherwise, it's identical to the original version.
- The API has been implemented with ASP.NET Core MVC 2.1 running on .NET Core 2.1. 
- Unit tests for the API using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4) have been added.
- Docker Compose and Kubernetes YAML files have been added to run the app in a Docker Container or as a service in Azure Kubernetes Service (AKS). 
- An ARM template has been added that allows to deploy the app in Service Fabric Mesh.
Like the original, this version uses [MongoDB](https://www.mongodb.com/) as database. Use whatever flavor of MongoDB works best for you. I recommend using either MongoDB Atlas, or MongoDB 3.4 or newer.

****
## Configuring the app
Please make sure to configure your MongoDB connection string in `appsettings.json`, as environment variable  `ConnectionStrings__DefaultConnection`, or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets).

## Building and running the app
### [Visual Studio 2017 (version 15.7 or higher recommended)](https://www.visualstudio.com/download)
Open `BookLibrary-NetCore.sln` and debug or run the solution.

### [Visual Studio Code](https://code.visualstudio.com/)
Open the folder `BookLibrary-NetCore` and run the  `build` and `min` tasks, then debug the app or run it from Code's integrated terminal.

### [.NET Core SDK 2.1](https://www.microsoft.com/net/download/core)
Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

### Windows 10
```
$ cd \path\to\BookLibrary-NetCore\BookLibrary
$ npm install
$ dotnet restore
$ node_modules\.bin\gulp min
$ dotnet run --no-launch-profile
```

### Linux, macOS
```
$ cd /path/to/BookLibrary-NetCore/BookLibrary
$ npm install
$ dotnet restore
$ node_modules/.bin/gulp min
$ dotnet run --no-launch-profile
```

Launch your web browser and load `http://localhost:5000`. 

>I recommend using the `--no-launch-profile` option as shown above so `dotnet` properly honors any environment variable set in your shell. 
>Otherwise, the app will launch with the same settings as though you had run it from Visual Studio 2017, using the settings defined in `launchSettings.json`.

### Building a [Docker](https://www.docker.com/community-edition) container
To build a Docker container directly from the sample's source code, you can use the included `Dockerfile`. This Dockerfile uses a [multi-stage build](https://docs.docker.com/engine/userguide/eng-image/multistage-build/). If you run the sample in a Docker container (see below), the included Compose file will build the container on the fly. A prebuilt Docker image for Linux is available at [Docker Hub](https://hub.docker.com/r/joergjo/booklibrary-netcore/). 

### Deploying on a [Docker](https://www.docker.com/community-edition) host
Please see [DOCKE-COMPOSE.md](DOCKER-COMPOSE.md) for details how to run the app with Docker Compose.

### Deploying on [Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/)
Please see [AKS.md](AKS.md) for details how to run the app in an Azure Kubernetes Service (AKS) cluster or other Kubernetes environments.

### Deploying as [Service Fabric Mesh](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/) application
Please see [SFMESH.md](SFMESH.md) for details how to run the app in Service Fabric Mesh.
