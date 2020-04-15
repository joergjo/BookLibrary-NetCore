using BookLibrary.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace BookLibrary.Mongo
{
    public static class BsonMapping
    {
        public static void Configure()
        {
            var conventions = new ConventionPack
            {
                new CamelCaseElementNameConvention()
            };

            ConventionRegistry.Register(
                $"{nameof(BookLibrary)}Convention",
                conventions,
                t => t == typeof(Book) || t == typeof(Keyword));

            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap
                    .SetIdGenerator(new StringObjectIdGenerator())
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Keyword>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(x => x.Name).SetElementName("keyword");
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
