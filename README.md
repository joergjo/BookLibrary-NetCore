# Book Library
This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](http://backbonejs.org/) with the following modifications and enhancements:
- The Backbone SPA is now bundled and minified using [gulp](http://gulpjs.com/). Otherwise, it's identical to the original version.
- The API has been implemented with ASP.NET Core MVC 2.0 running on .NET Core 2.0. 
- Unit tests for the API using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4) have been added.
- Docker Compose (`docker-compose.yml`) and Kubernetes (`booklibrary.yaml`) YAML files have been added to run the app in a Docker Container or as a service in Kubernetes on Linux. 

This version, like the original, uses [MongoDB](https://www.mongodb.com/) as database. Use whatever flavor of MongoDB works best for you. I recommend using either MongoDB Atlas, or MongoDB 3.4 or newer.

****
## Configuring the app
Please make sure to configure your connection string in `appsettings.json` or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets).
If you want to run the app in a Kubernetes cluster including minikube, open `booklibrary.yaml` and replace `<MONGOHOST>` with the FQDN or IP address of your MongoDB host or service. Note that the Kubernetes service definition does _not_ include a MongoDB pod. You will either need to have a running MongoDB instance outside of your Kubernetes cluster, or add a MongoDB pod to the service definition.

## Building and running the app
### [Visual Studio 2017 (version 15.3 or higher required)](https://www.visualstudio.com/download)
Open `BookLibrary-NetCore.sln` and debug or run the solution.

### [Visual Studio Code](https://code.visualstudio.com/)
Open the folder `BookLibrary-NetCore` and run the  `build` and `min` tasks, then debug the app or run it from Code's integrated terminal.

### [.NET Core SDK 2.0](https://www.microsoft.com/net/download/core)
Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

<code>
$ cd /path/to/BookLibrary-NetCore/BookLibrary<br />
</code> (Linux, macOS)
or<br />
<code>   
$ cd \path\to\BookLibrary-NetCore\BookLibrary<br />
</code> (Windows)<br />
<br />

<code>
$ npm install <br />
$ dotnet restore <br />
$ node_modules/.bin/gulp min</code> (Linux, macOS) or <br />
<code>$ node_modules\.bin\gulp min</code> (Windows) <br />
<code>
$ dotnet run --no-launch-profile
</code>

Launch your web browser and load `http://localhost:5000`. I recommend to use the `--no-launch-profile` option as shown above so `dotnet` properly honors any environment variable set in your shell. Otherwise, the app will launch just as if you had run it from within Visual Studio 2017 and its debug settings.

### Building as [Docker](https://www.docker.com/community-edition) container
To build a Docker container directly from the sample's source code, you can use the included `Dockerfile`. This Dockerfile uses a [multi-stage build](https://docs.docker.com/engine/userguide/eng-image/multistage-build/). If you run the sample in a Docker container (see below), the included Compose file will build the container on the fly. A prebuilt Docker image for Linux is available at [Docker Hub]( https://hub.docker.com/r/joergjo/booklibrary-netcore/). 

### Running in [Docker](https://www.docker.com/community-edition) container
If you have a Docker host running or Docker locally installed on your machine, you don't even need the .NET Core runtime, SDK or Visual Studio, nor an existing MongoDB host. 
Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

<code>
$ cd /path/to/BookLibrary-NetCore<br />
</code> (Linux, macOS)
or<br />
<code>   
$ cd \path\to\BookLibrary-NetCore<br />
</code> (Windows)<br />
<br />

<code>
$ docker-compose up
</code> (on Linux)
or <br />
<code>
$ docker-compose -f docker-compose.yml -f docker-compose.override.windows.yml up
</code> (on Windows)

Launch your web browser and load `http://localhost:5000` if you are running a Linux container. If you are running a Windows container, you will need to use your machine's IP address on the `DockerNAT` virtual Ethernet adapter instead of `localhost`. You can get this by running `ipconfig` (it's usually `10.0.75.1`).  

### [Kubernetes](https://kubernetes.io/)
You will need to have [kubectl](https://github.com/kubernetes/kubernetes/releases) installed locally in order to deploy this app. Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

<code>
$ cd /path/to/BookLibrary-NetCore<br />
</code> (Linux, macOS)
or<br />
<code>   
$ cd \path\to\BookLibrary-NetCore<br />
</code> (Windows)<br />
<br />

<code>
$ kubectl create -f booklibrary.yaml
</code>
