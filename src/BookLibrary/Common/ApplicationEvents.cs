namespace BookLibrary.Common
{
    public static class ApplicationEvents
    {
        public const int LibraryQueried = 1000;
        public const int BookQueried = 1001;
        public const int BookCreated = 1002;
        public const int BookUpdated = 1003;
        public const int BookDeleted = 1004;

        public const int BookNotFound = 2000;

        public const int UnhandledExceptionOccurred = 9999;
    }
}
