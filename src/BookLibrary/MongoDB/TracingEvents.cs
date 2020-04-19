namespace BookLibrary.MongoDB
{
    public static class TracingEvents
    {
        public const int TelemetryAddedToCache = 4000;
        public const int TelemetryRemovedFromCache = 4001;
        public const int ErrorAddingTelemetryToCache = 4900;
        public const int ErrorRemovingTelemetryFromCache = 4901;
    }
}
