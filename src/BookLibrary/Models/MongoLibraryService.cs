using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Common;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookLibrary.Models
{
    public class MongoLibraryService : ILibraryService
    {
        private readonly IMongoCollection<Book> _books;
        private readonly TelemetryClient _telemetryClient;
        private readonly string _serverNames;

        public static string DefaultCollectionName => "books";

        public MongoLibraryService(IMongoDatabase database, TelemetryClient telemetryClient)
        {
            _books = database.GetCollection<Book>(DefaultCollectionName);
            _telemetryClient = telemetryClient;
            _serverNames = database.GetServerNames();
        }

        public MongoLibraryService(IMongoCollection<Book> books, TelemetryClient telemetryClient)
        {
            _books = books;
            _telemetryClient = telemetryClient;
            _serverNames = _books.Database.GetServerNames();
        }

        public async Task<List<Book>> FindAllAsync(int limit)
        {
            var books = await ExecuteAndTrack(async () =>
            {
                var options = new FindOptions<Book, Book>
                {
                    Limit = limit
                };
                using var cursor = await _books.FindAsync(new BsonDocument(), options);
                return await cursor.ToListAsync();
            }, nameof(_books.FindAsync));
            return books;
        }

        public async Task<Book?> FindAsync(string id)
        {
            var books = await ExecuteAndTrack(async () =>
            {
                using var cursor = await _books.FindAsync(x => x.Id == id);
                return await cursor.ToListAsync();
            }, nameof(_books.FindAsync));
            return books.SingleOrDefault();
        }

        public async Task<Book> AddAsync(Book book)
        {
            await ExecuteAndTrack(async () =>
            {
                await _books.InsertOneAsync(book);
                return Task.CompletedTask;
            }, nameof(_books.InsertOneAsync));
            return book;
        }

        public async Task<Book?> RemoveAsync(string id)
        {
            var book = await ExecuteAndTrack(async () =>
            {
                return await _books.FindOneAndDeleteAsync(x => x.Id == id);
            }, nameof(_books.FindOneAndDeleteAsync));
            return book;
        }

        public async Task<Book?> UpdateAsync(string id, Book book)
        {
            var updatedBook = await ExecuteAndTrack(async () =>
            {
                var options = new FindOneAndUpdateOptions<Book, Book>
                {
                    ReturnDocument = ReturnDocument.After
                };
                return await _books.FindOneAndUpdateAsync<Book>(
                    x => x.Id == id,
                    Builders<Book>.Update
                        .Set(x => x.Title, book.Title)
                        .Set(x => x.Author, book.Author)
                        .Set(x => x.ReleaseDate, book.ReleaseDate)
                        .Set(x => x.Keywords, book.Keywords),
                    options);

            }, nameof(_books.FindOneAndUpdateAsync));
            return updatedBook;
        }

        private async Task<TResult> ExecuteAndTrack<TResult>(Func<Task<TResult>> asyncFunc, string operationName)
            where TResult : class
        {
            using var operation = _telemetryClient.StartOperation<DependencyTelemetry>(_serverNames);
            try
            {
                var result = await asyncFunc();
                UpdateDependencyTelemetry(operation.Telemetry, operationName);
                return result;
            }
            catch (Exception)
            {
                UpdateDependencyTelemetry(operation.Telemetry, operationName, isSuccess: false);
                throw;
            }

            void UpdateDependencyTelemetry(DependencyTelemetry telemetry, string operationName, bool isSuccess = true)
            {
                telemetry.Success = isSuccess;
                telemetry.Type = "MongoDB";
                telemetry.Target = $"{telemetry.Type}:{_serverNames}";
                telemetry.Data = operationName;
            }
        }
    }
}
