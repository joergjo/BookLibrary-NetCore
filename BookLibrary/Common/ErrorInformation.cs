using System;

namespace BookLibrary.Common
{
    public class ErrorInformation
    {
        public string RequestId { get; set; }

        public string Message { get; set; }

        public DateTimeOffset DateTime { get; set; }
    }
}
