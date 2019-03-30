# Book Library
[![Build Status](https://joergjooss.visualstudio.com/BookLibrary-NetCore/_apis/build/status/BookLibrary-NetCore-Container-GitHub-CI?branchName=master)](https://joergjooss.visualstudio.com/BookLibrary-NetCore/_build/latest?definitionId=9&branchName=master)

This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](http://backbonejs.org/) with the following modifications and enhancements:
- The Backbone SPA is now bundled and minified using [gulp](http://gulpjs.com/). Otherwise, it's identical to the original version.
- The API has been implemented with ASP.NET Core MVC 2.2 running on .NET Core 2.2. 
- Unit tests for the API using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4) have been added.
- Client-side dependencies (i.e. JavaScript and CSS libraries) are managed with [LibMan](https://github.com/aspnet/LibraryManager/).
- Built-time JavaScript dependencies are managed with [NPM](https://www.npmjs.com/). 
    >Unfortunately, LibMan is no option for build-time tools, and [BundlerMinifier.Core](https://github.com/madskristensen/BundlerMinifier) doesn't create useful source maps.   
- Docker Compose and Kubernetes YAML files have been added to run the app in a Docker Container or as a service in Azure Kubernetes Service (AKS). 
- An ARM template has been added that allows to deploy the app in Service Fabric Mesh.
Like the original, this version uses [MongoDB](https://www.mongodb.com/) as database. Use whatever flavor of MongoDB works best for you. I recommend using either MongoDB Atlas, or MongoDB 3.4 or newer.

****
## Configuring the app
Please make sure to configure your MongoDB connection string in `appsettings.json`, as environment variable `ConnectionStrings__DefaultConnection`, or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets).

## Building and running the app

### Visual Studio 2017
>Requires [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/core), [Visual Studio 2017 15.9 or later](https://www.visualstudio.com/download), [Node.js 10 LTS](https://nodejs.org/en/download/).

Open `BookLibrary-NetCore.sln` and either debug or run the solution.

### Visual Studio Code
>Requires [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/core), [Visual Studio Code 1.30 or later](https://www.visualstudio.com/download), [Node.js 10 LTS](https://nodejs.org/en/download/).
 
Open the folder `BookLibrary-NetCore` and run the `npm: Install Dependencies`, `build` and `min` tasks, then debug the app or run it from Code's integrated terminal.

### .NET Core SDK 2.2 only
>Requires [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/core), [Node.js 10 LTS](https://nodejs.org/en/download/).

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
>Otherwise, the app will launch with the included Visual Studio 2017 launch profile based on `launchSettings.json`.

### Building a [Docker](https://www.docker.com/community-edition) container
>Requires [Docker Community Edition](https://store.docker.com/search?type=edition&offering=community)

To build a Docker container directly from the sample's source code, you can use the included `Dockerfile`. This Dockerfile uses a [multi-stage build](https://docs.docker.com/engine/userguide/eng-image/multistage-build/). Using the included Compose file, the app container will be built on the fly. A prebuilt Docker image for Linux is available at [Docker Hub](https://hub.docker.com/r/joergjo/booklibrary-netcore/). 

### Deploying on a [Docker](https://www.docker.com/community-edition) host
Please see [DOCKE-COMPOSE.md](docs/DOCKER-COMPOSE.md) for details how to run the app with Docker Compose.

### Deploying on [Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/)
Please see [KUBERNETES.md](docs/KUBERNETES.md) for details how to run the app in an Azure Kubernetes Service (AKS) cluster or other Kubernetes environments.
