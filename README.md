# Book Library
This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](https://github.com/jashkenas/backbone):
- The Backbone SPA is now bundled and minified using [gulp](http://gulpjs.com/). Otherwise, it's identical to the original version.
- The API has been implemented with ASP.NET Core MVC running on .NET Core. 
- The API's data access layer has been built to be fully compatible with the Node.js version, hence you can use the same database for this version and the original.
- Unit tests for the API using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4) have been added.

****

Like the original, this version uses [MongoDB](https://www.mongodb.com/) as database. The implementation supports both MongoDB and MongoDB Atlas. Use whatever flavor of MongoDB works best for you. I recommend using either MongoDB or MongoDB Atlas version 3.4 or higher.  

Please make sure to configure your connection string in `appsettings.json` or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets).

>##### Building the app
>This app requires [Visual Studio 2017](https://www.visualstudio.com/download) or the [.NET Core SDK](https://www.microsoft.com/net/download/core) to build. 
>You can also use the included Docker Compose file to build the solution in a Docker container:
>
>`$ docker-compose -f docker-compose-build.yml up`
