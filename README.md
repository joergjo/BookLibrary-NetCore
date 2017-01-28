# Book Library
This is an ASP.NET Core MVC port of [Addy Osmani's Book Library sample](https://github.com/addyosmani/backbone-fundamentals/tree/gh-pages/practicals/exercise-2)
for [Backbone.js](https://github.com/jashkenas/backbone). The Backbone.js SPA in this version is identical to the original, but its Node.js Web API has been 
rewritten in ASP.NET Core MVC, including unit tests using [xUnit.net](https://github.com/xunit/xunit) and [Moq](https://github.com/moq/moq4).
****
Like the original, this version uses MongoDB as database. Make sure to configure your connection string in `appsettings.config` 
or set up your [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets):
- `MongoDB:DefaultConnection:Host`: Your MongoDB host name or IP address.
- `MongoDB:DefaultConnection:Database`: The MongoDB database to use (default is `library_database`).
- `MongoDB:DefaultConnection:Username`: The MongoDB user name. The user must have at least the `readWrite` [role](https://docs.mongodb.com/manual/reference/built-in-roles/#database-user-roles).
- `MongoDB:DefaultConnection:Password`: The MongoDB user's password.
- `MongoDB:DefaultConnection:UseTransportSecurity`: This is optional. If set to `true`, the MongoDB connection will be secured using TLS 1.2. If not set or set to `false` it is **unsecure**.

>##### Implemenation note
>Why not simply use a MongoDB URI as connection string? Because I've run into numerous problems with applications and drivers that can't deal with passwords
>containing punctuation characters or symbols like `@` in MongoDB URIs. 