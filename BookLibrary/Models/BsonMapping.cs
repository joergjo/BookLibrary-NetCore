using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BookLibrary.Api.Models
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
                nameof(BookLibrary),
                conventions,
                t => t.FullName.StartsWith(typeof(Book).Namespace));

            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(m => m.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<Keyword>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(x => x.Name).SetElementName("keyword");
            });
        }
    }
}
