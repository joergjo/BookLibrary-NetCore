using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookLibrary.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _books;

        public BookRepository(IMongoDatabase database)
        {
            if (database is null)
            {
                throw new ArgumentNullException(nameof(database));
            }
            _books = database.GetCollection<Book>("books");
        }

        public BookRepository(IMongoCollection<Book> books)
        {
            _books = books ?? throw new ArgumentNullException(nameof(books));
        }

        public async Task<List<Book>> FindAllAsync(int limit)
        {
            var options = new FindOptions<Book, Book>
            {
                Limit = limit
            };
            using var cursor = await _books.FindAsync(new BsonDocument(), options);
            var books = await cursor.ToListAsync();
            return books;
        }

        public async Task<Book> FindAsync(string id)
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

        public async Task<Book> RemoveAsync(string id)
        {
            var book = await _books.FindOneAndDeleteAsync(x => x.Id == id);
            return book;
        }

        public async Task<Book> UpdateAsync(string id, Book book)
        {
            var options = new FindOneAndUpdateOptions<Book, Book>
            {
                ReturnDocument = ReturnDocument.After
            };
            var updatedBook = await _books.FindOneAndUpdateAsync<Book>(
                x => x.Id == id,
                Builders<Book>.Update
                    .Set(x => x.Title, book.Title)
                    .Set(x => x.Author, book.Author)
                    .Set(x => x.ReleaseDate, book.ReleaseDate)
                    .Set(x => x.Keywords, book.Keywords),
                options);
            return updatedBook;
        }
    }
}
