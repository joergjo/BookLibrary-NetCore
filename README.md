# Book Library
This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](http://backbonejs.org/) with the following modifications and enhancements:
- The Backbone SPA is now bundled and minified using [gulp](http://gulpjs.com/). Otherwise, it's identical to the original version.
- The API has been implemented with ASP.NET Core MVC running on .NET Core. 
- The API's data access layer in .NET Core has been built to be fully compatible with the Node.js version, hence you can use the same database for this version and the original Node.js backend.
- Unit tests for the API using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4) have been added.
- Docker Compose (`docker-compose.yml`) and Kubernetes (`booklibrary.yaml`) YAML files have been added to run the app in a Docker Container or as a service in Kubernetes on Linux. 

This version, like the original, uses [MongoDB](https://www.mongodb.com/) as database. Use whatever flavor of MongoDB works best for you. I recommend using either MongoDB or MongoDB Atlas version 3.4 or higher.  

****
## Configuring the app
Please make sure to configure your connection string in `appsettings.json` or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets).

If you want to run the app in a Docker host, open `docker-compose.yml` and replace `<MONGOHOST>` with the FQDN or IP address of your MongoDB host or service.

If you want to run the app in a Kubernetes cluster including minikube, open 'booklibrary.yaml` and replace `<MONGOHOST>` with the FQDN or IP address of your MongoDB host or service. 

## Building and running the app
### [Visual Studio 2017](https://www.visualstudio.com/download)
Open `BookLibrary-NetCore.sln` and debug or run the solution.

### [Visual Studio Code](https://code.visualstudio.com/)
Open the folder `BookLibrary-NetCore` and run the Build task, then debug the app or run it from Code's integrated terminal.

### [.NET Core SDK](https://www.microsoft.com/net/download/core)
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
$ dotnet restore<br />
$ dotnet run
</code>

### [Docker](https://www.docker.com/community-edition)
If you have a Docker host running or Docker locally installed on your machine, you don't even need the .NET Core runtime, SDK or Visual Studio. Open a command line or shell window (i.e. PowerShell, Console, Bash etc.) and run the following commands:

<code>
$ cd /path/to/BookLibrary-NetCore<br />
</code> (Linux, macOS)
or<br />
<code>   
$ cd \path\to\BookLibrary-NetCore<br />
</code> (Windows)<br />
<br />

<code>
$ docker-compose -f docker-compose-build.yml up<br />
$ docker-compose -f docker-compose.yml up
</code>

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
