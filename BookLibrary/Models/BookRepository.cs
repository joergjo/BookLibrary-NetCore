using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Api.Models
{
    public class BookRepository : IBookRepository
    {
        private IMongoCollection<Book> _books;

        public BookRepository(IMongoCollection<Book> books)
        {
            _books = books ?? throw new ArgumentNullException(nameof(books));
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var cursor = await _books.FindAsync(new BsonDocument());
            IEnumerable<Book> books = await cursor.ToListAsync();
            return books;
        }

        public async Task<Book> GetBookAsync(string id)
        {
            var cursor = await _books.FindAsync(x => x.Id == id);
            var books = await cursor.ToListAsync();
            return books.SingleOrDefault();
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task<bool> RemoveBookAsync(string id)
        {
            var result = await _books.DeleteOneAsync(x => x.Id == id);
            return (result.IsAcknowledged && result.DeletedCount == 1);
        }

        public async Task<bool> UpdateBookAsync(string id, Book book)
        {
            var result = await _books.UpdateOneAsync(x => x.Id == id,
                Builders<Book>.Update
                    .Set(x => x.Title, book.Title)
                    .Set(x => x.Author, book.Author)
                    .Set(x => x.ReleaseDate, book.ReleaseDate)
                    .Set(x => x.Keywords, book.Keywords));
            return (result.IsAcknowledged && result.ModifiedCount == 1);
        }
    }
}