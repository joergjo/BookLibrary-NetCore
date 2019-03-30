using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public interface IBookRepository
    {
        Task<Book> AddBookAsync(Book book);

        Task<IEnumerable<Book>> GetAllBooksAsync();

        Task<Book> FindBookAsync(string id);

        Task<bool> RemoveBookAsync(string id);

        Task<bool> UpdateBookAsync(string id, Book book);
    }
}
