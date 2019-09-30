using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MongoDB.Bson;

namespace BookLibrary.Common
{
    public class ObjectIdConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[routeKey] is string oid)
            {
                return ObjectId.TryParse(oid, out _);
            };

            return false;
        }
    }
}
