using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _serverNames = GetServerNames(database);
        }

        public MongoLibraryService(IMongoCollection<Book> books, TelemetryClient telemetryClient)
        {
            _books = books;
            _telemetryClient = telemetryClient;
            _serverNames = GetServerNames(_books.Database);
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
            }, nameof(FindAllAsync));
            return books;
        }

        public async Task<Book?> FindAsync(string id)
        {
            var books = await ExecuteAndTrack(async () =>
            {
                using var cursor = await _books.FindAsync(x => x.Id == id);
                return await cursor.ToListAsync();
            }, nameof(FindAsync));
            return books.SingleOrDefault();
        }

        public async Task<Book> AddAsync(Book book)
        {
            await ExecuteAndTrack(async () =>
            {
                await _books.InsertOneAsync(book);
                return Task.CompletedTask;
            }, nameof(AddAsync));
            return book;
        }

        public async Task<Book?> RemoveAsync(string id)
        {
            var book = await ExecuteAndTrack(async () =>
            {
                return await _books.FindOneAndDeleteAsync(x => x.Id == id);
            }, nameof(RemoveAsync));
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

            }, nameof(UpdateAsync));
            return updatedBook;
        }

        private async Task<TResult> ExecuteAndTrack<TResult>(Func<Task<TResult>> asyncFunc, string operationName) where TResult : class
        {
            using var operation = _telemetryClient.StartOperation<DependencyTelemetry>(operationName);
            try
            {
                var result = await asyncFunc();
                UpdateDependencyTelemetry(operation.Telemetry);
                return result;
            }
            catch (Exception)
            {
                UpdateDependencyTelemetry(operation.Telemetry, isSuccess: false);
                throw;
            }
        }

        private void UpdateDependencyTelemetry(DependencyTelemetry telemetry, bool isSuccess = true)
        {
            telemetry.Success = isSuccess;
            telemetry.Target = _serverNames;
            telemetry.Type = "MongoDB";
        }

        private static string GetServerNames(IMongoDatabase database)
        {
            var sb = new StringBuilder();
            foreach (var server in database.Client.Settings.Servers)
            {
                sb.AppendFormat("{0}:{1};", server.Host, server.Port);
            }
            return sb.ToString();
        }
    }
}
