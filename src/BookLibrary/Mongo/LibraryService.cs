using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookLibrary.Mongo
{
    public class LibraryService : ILibraryService
    {
        private readonly IMongoCollection<Book> _books;

        public LibraryService(IMongoDatabase database, string collectionName = "books")
            : this(database.GetCollection<Book>(collectionName))
        {
        }

        public LibraryService(IMongoCollection<Book> books)
        {
            _books = books;
        }

        public async Task<List<Book>> FindAllAsync(int limit)
        {
            var options = new FindOptions<Book, Book> { Limit = limit };
            using var cursor = await _books.FindAsync(new BsonDocument(), options);
            return await cursor.ToListAsync();
        }

        public async Task<Book?> FindAsync(string id)
        {
            using var cursor = await _books.FindAsync(x => x.Id == id);
            var books = await cursor.ToListAsync();
            return books.SingleOrDefault();
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task<Book?> RemoveAsync(string id)
        {
            return await _books.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public async Task<Book?> UpdateAsync(string id, Book book)
        {
            var options = new FindOneAndUpdateOptions<Book, Book> { ReturnDocument = ReturnDocument.After };
            return await _books.FindOneAndUpdateAsync<Book>(
                x => x.Id == id,
                Builders<Book>.Update
                    .Set(x => x.Title, book.Title)
                    .Set(x => x.Author, book.Author)
                    .Set(x => x.ReleaseDate, book.ReleaseDate)
                    .Set(x => x.Keywords, book.Keywords),
                options);
        }
    }
}
