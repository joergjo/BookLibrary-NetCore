# Book Library
[![Build Status](https://dev.azure.com/joergjooss/BookLibrary-NetCore/_apis/build/status/GitHub-CI?branchName=master)](https://dev.azure.com/joergjooss/BookLibrary-NetCore/_build/latest?definitionId=18&branchName=master)

This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](http://backbonejs.org/) with the following modifications and enhancements:
- The API has been implemented in C# 8 with ASP.NET Core MVC 3.0.
- Unit tests for the API using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4) have been added.
- Client-side dependencies (i.e. JavaScript and CSS libraries) dependencies are managed with [LibMan](https://github.com/aspnet/LibraryManager/). The Backbone SPA is bundled and minified using [gulp](http://gulpjs.com/). The gulpfile has been generated using [BundlerMinifier.Core](https://github.com/madskristensen/BundlerMinifier).
    >I'm not using BundlerMinifier.Core directly since it  doesn't create useful source maps. The generated gulpfile has been extended to support source map creation.
- [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) is being used for Application Performance Management, including [dependency tracking](https://docs.microsoft.com/en-us/azure/azure-monitor/app/custom-operations-tracking#outgoing-dependencies-tracking) for MongoDB operations.
- Docker Compose and Kubernetes YAML files have been added to run the app on a [Docker](https://www.docker.com/) host or in [Kubernetes](https://kubernetes.io/).
- This project uses [MongoDB](https://www.mongodb.com/) as database like the original Node.js version. Use whatever flavor of MongoDB works best for you. I recommend using either MongoDB Atlas, Cosmos DB's MongoDB API, or MongoDB 3.4 or newer.
- An [Azure Pipeline](https://azure.microsoft.com/en-us/services/devops/pipelines/) has been added for CI/CD. This is obviously _the_ pipeline I use myself for this project.

The latest version of this application is deployed at [https://booklibrary.joergjooss.de/](https://booklibrary.joergjooss.de/).

****
## Configuring the app
Please make sure to configure your MongoDB connection string in `appsettings.json`, as environment variable `ConnectionStrings__DefaultConnection`, or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets).

## Building and running the app

### Visual Studio 2019
>Requires [.NET Core SDK 3.0 or later](https://www.microsoft.com/net/download/core), [Visual Studio 2019 16.3.0 or later](https://www.visualstudio.com/download), [Node.js 10 LTS](https://nodejs.org/en/download/).

Open `BookLibrary-NetCore.sln` and either debug or run the solution.

>Visual Studio 2019 is the only version of Visual Studio that support .NET Core 3.

### Visual Studio Code
>Requires [.NET Core SDK 3.0 or later](https://www.microsoft.com/net/download/core), [Visual Studio Code 1.30 or later](https://www.visualstudio.com/download), [Node.js 10 LTS](https://nodejs.org/en/download/).

Open the folder `BookLibrary-NetCore` and run the `npm: Install Dependencies`, `build` and `min` tasks, then debug the app or run it from Code's integrated terminal.

### .NET Core SDK 3.0 only
>Requires [.NET Core SDK 3.0 or later](https://www.microsoft.com/net/download/core), [Node.js 10 LTS](https://nodejs.org/en/download/).

Open a terminal window withyour preferred shell  (i.e. PowerShell, CMD.exe, Bash etc.) and run the following commands:

### Windows 10
```
$ cd \path\to\BookLibrary-NetCore\src\BookLibrary
$ npm install
$ dotnet restore
$ node_modules\.bin\gulp min
$ dotnet run --no-launch-profile
```

### Linux, macOS
```
$ cd /path/to/BookLibrary-NetCore/src/BookLibrary
$ npm install
$ dotnet restore
$ node_modules/.bin/gulp min
$ dotnet run --no-launch-profile
```

Launch your web browser and load `http://localhost:5000`.

>I recommend using the `--no-launch-profile` option as shown above so `dotnet` properly honors any environment variable set in your shell.
>Otherwise, the app will launch with the included Visual Studio 2019 launch profile based on `launchSettings.json`.

### Building a [Docker](https://www.docker.com/community-edition) container
>Requires [Docker Desktop](https://store.docker.com/search?type=edition&offering=community)

To build a Docker container directly from the sample's source code, you can use the included Dockerfile. This Dockerfile uses a [multi-stage build](https://docs.docker.com/engine/userguide/eng-image/multistage-build/). Using the included Compose file, the app container will be built on the fly. A prebuilt Docker image for Linux is available at [Docker Hub](https://hub.docker.com/r/joergjo/booklibrary-netcore/).

### Deploying on a [Docker](https://www.docker.com/community-edition) host
Please see [DOCKE-COMPOSE.md](docs/DOCKER-COMPOSE.md) for details how to run the app with Docker Compose.

### Deploying on [Kubernetes](https://kubernetes.io)
Please see [KUBERNETES.md](docs/KUBERNETES.md) for details how to run the app in a Kubernetes cluster.

### CI/CD with [Azure Pipelines](https://azure.microsoft.com/en-us/services/devops/pipelines/)
Use the included pipeline [`.azure/pipelines/ci.yml`](./.azure/pipelines/ci.yml) to set up your own CI/CD pipeline for this project using Azure Pipelines. The included pipeline both runs for pull request validation and full builds of `master`, as well as a deployment job for [Azure App Services](https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-docker). Pull request validation uses a standard .NET Core SDK build and test, whereas a full build of master runs a multi-stage Docker build. Review the [`ci.yml`](.azure/pipelines/ci.yml) file for further instructions.
