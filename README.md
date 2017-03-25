# Book Library
This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](https://github.com/jashkenas/backbone). The Backbone.js SPA in this version is identical to the original, but its Node.js Web API has been 
rewritten in ASP.NET Core MVC, including unit tests using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4).

>##### Note
>This sample requires [Visual Studio 2017](https://www.visualstudio.com/download) or the [.NET Core SDK](https://www.microsoft.com/net/download/core) to build. 

****

Like the original, this version uses [MongoDB](https://www.mongodb.com/) as database. Make sure to configure your connection string in `appsettings.config` 
or in your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets):
- `MongoDB:DefaultConnection:Host`: Your MongoDB host name or IP address.
- `MongoDB:DefaultConnection:Database`: The MongoDB database to use (default is `library_database`).
- `MongoDB:DefaultConnection:Username`: The MongoDB user name. The user must have at least the `readWrite` [role](https://docs.mongodb.com/manual/reference/built-in-roles/#database-user-roles).
- `MongoDB:DefaultConnection:Password`: The MongoDB user's password.
- `MongoDB:DefaultConnection:UseTransportSecurity`: This setting is optional. If set to `true`, the MongoDB connection will be secured using [TLS](https://docs.mongodb.com/manual/tutorial/configure-ssl/). If not set or set to `false` the connection is **not** encrypted.

>##### Note
>Why not simply use a [MongoDB URI](https://docs.mongodb.com/manual/reference/connection-string/) as connection string? Because I've run into numerous problems with applications and drivers that can't deal with passwords
>containing punctuation characters or symbols like `@` in MongoDB URIs. 
