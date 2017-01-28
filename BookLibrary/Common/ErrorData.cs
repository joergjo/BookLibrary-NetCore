using System;

namespace BookLibrary.Api.Common
{
    public class ErrorData
    {
        public string Message { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public Uri RequestUri { get; set; }

        public Guid ErrorId { get; set; }

        public Exception Exception { get; set; }
    }
}
