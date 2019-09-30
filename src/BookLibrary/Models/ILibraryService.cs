using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public interface ILibraryService
    {
        Task<Book> AddAsync(Book book);

        Task<List<Book>> FindAllAsync(int limit = 100);

        Task<Book> FindAsync(string id);

        Task<Book> RemoveAsync(string id);

        Task<Book> UpdateAsync(string id, Book book);
    }
}
