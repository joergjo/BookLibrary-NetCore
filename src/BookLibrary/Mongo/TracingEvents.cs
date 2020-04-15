namespace BookLibrary.Mongo
{
    public static class TracingEvents
    {
        public const int SpanAddedToCache = 4000;
        public const int SpanRemovedFromCache = 4001;
        public const int ErrorAddingSpanToCache = 4900;
        public const int ErrorRemovingSpanFromCache = 4901;
    }
}
