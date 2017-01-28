namespace BookLibrary.Api.Common
{
    public class MongoClientOptions
    {
        public string Host { get; set; }

        public int? Port { get; set; }

        public string Database { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool? UseTransportSecurity { get; set; }
    }
}
