using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public interface IBookRepository
    {
        Task<Book> AddBookAsync(Book book);

        Task<IEnumerable<Book>> GetAllBooksAsync();

        Task<Book> FindBookAsync(string id);

        Task<Book> RemoveBookAsync(string id);

        Task<Book> UpdateBookAsync(string id, Book book);
    }
}
