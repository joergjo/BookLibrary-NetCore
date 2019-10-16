using System.Text;
using MongoDB.Driver;

namespace BookLibrary.Common
{
    public static class MongoExtensions
    {
        public static string GetServerNames(this IMongoDatabase database, string scheme = "mongodb://")
        {
            var sb = new StringBuilder();
            foreach (var server in database.Client.Settings.Servers)
            {
                sb.AppendFormat("{0}{1}:{2};", scheme, server.Host, server.Port);
            }
            return sb.ToString();
        }
    }
}
