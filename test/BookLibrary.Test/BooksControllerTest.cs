using BookLibrary.Controllers;
using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookLibrary.Test
{
    public class BooksControllerTest
    {
        [Fact(DisplayName = "BooksControllerTest.Get_Returns_All_Books")]
        public async Task Get_Returns_All_Books()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.GetAllBooksAsync())
                .ReturnsAsync(
                    new List<Book>
                    {
                        new Book { Id = "1" },
                        new Book { Id = "2" },
                        new Book { Id = "3" },
                        new Book { Id = "4" },
                        new Book { Id = "5" }
                    });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Get();
            var books = actionResult?.Value as IEnumerable<Book>;

            // Assert
            Assert.Equal(5, books?.Count());
        }

        [Fact(DisplayName = "BooksControllerTest.Get_With_Valid_Id_Returns_Single_Book")]
        public async Task Get_With_Valid_Id_Returns_Single_Book()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.FindBookAsync("1"))
                .ReturnsAsync(new Book { Id = "1" });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Get("1");
            var book = actionResult.Value as Book;

            // Assert
            Assert.Equal("1", book?.Id);
        }

        [Fact(DisplayName = "BooksControllerTest.Get_With_Invalid_Id_Returns_HttpNotFound")]
        public async Task Get_With_Invalid_Id_Returns_HttpNotFound()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.FindBookAsync(It.IsAny<string>()))
                .ReturnsAsync(default(Book));

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Get("42");
            var result = actionResult.Result as NotFoundResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }

        [Fact(DisplayName = "BooksControllerTest.Post_Sets_RouteData_To_Book")]
        public async Task Post_Sets_RouteData_To_Book()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.AddBookAsync(It.IsAny<Book>()))
                .Returns((Book b) =>
                {
                    b.Id = "42";
                    return Task.FromResult(b);
                });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Post(new Book());
            var result = actionResult.Result as CreatedAtRouteResult;

            // Assert
            Assert.Contains(new KeyValuePair<string, object>("id", "42"), result?.RouteValues);
        }

        [Fact(DisplayName = "BooksControllerTest.Post_Returns_Book_With_Id")]
        public async Task Post_Returns_Book_With_Id()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.AddBookAsync(It.IsAny<Book>()))
                .Returns((Book b) =>
                {
                    b.Id = "42";
                    return Task.FromResult(b);
                });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Post(new Book());
            var result = actionResult.Result as CreatedAtRouteResult;
            var book = result?.Value as Book;

            // Assert
            Assert.Equal("42", book?.Id);
        }

        [Fact(DisplayName = "BooksControllerTest.Post_Adds_Book_To_Repository")]
        public async Task Post_Adds_Book_To_Repository()
        {
            // Arrange
            var mockRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            mockRepository
                .Setup(x => x.AddBookAsync(It.IsAny<Book>()))
                .Returns((Book b) =>
                {
                    b.Id = "42";
                    return Task.FromResult(b);
                })
                .Verifiable();

            var controller = new BooksController(mockRepository.Object, stubLogger.Object);

            // Act
            await controller.Post(new Book());

            // Assert
            mockRepository.Verify();
        }

        [Fact(DisplayName = "BooksControllerTest.Put_With_Valid_Id_Returns_Same_Book")]
        public async Task Put_With_Valid_Id_Returns_Same_Book()
        {
            // Arrange
            var book = new Book { Id = "1" };
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.UpdateBookAsync("1", book))
                .ReturnsAsync(new Book { Id = "1" });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Put("1", book);
            var actualBook = actionResult?.Value as Book;

            // Assert
            Assert.Equal("1", actualBook?.Id);
        }

        [Fact(DisplayName = "BooksControllerTest.Put_With_Valid_Id_Saves_Book_To_Repository")]
        public async Task Put_With_Valid_Id_Saves_Book_To_Repository()
        {
            // Arrange
            var mockRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            mockRepository
                .Setup(x => x.UpdateBookAsync("1", It.IsAny<Book>()))
                .ReturnsAsync(new Book { Id = "1" })
                .Verifiable();

            var controller = new BooksController(mockRepository.Object, stubLogger.Object);

            // Act
            await controller.Put("1", new Book());

            // Assert
            mockRepository.Verify();
        }

        [Fact(DisplayName = "BooksControllerTest.Put_With_Invalid_Id_Returns_HttpNotFound")]
        public async Task Put_With_Invalid_Id_Returns_HttpNotFound()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.UpdateBookAsync(It.IsAny<string>(), It.IsAny<Book>()))
                .ReturnsAsync(default(Book));

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Put("42", new Book());
            var result = actionResult.Result as NotFoundResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }

        [Fact(DisplayName = "BooksControllerTest.Delete_With_Valid_Id_Returns_HttpNoContent")]
        public async Task Delete_With_Valid_Id_Returns_HttpNoContent()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                 .Setup(x => x.RemoveBookAsync(It.Is<string>(s => s == "1")))
                 .ReturnsAsync(new Book { Id = "1" });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var result = await controller.Delete("1") as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);
        }

        [Fact(DisplayName = "BooksControllerTest.Delete_With_Invalid_Id_Returns_HttpNotFound")]
        public async Task Delete_With_Invalid_Id_Returns_NotFound()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                 .Setup(x => x.RemoveBookAsync(It.IsAny<string>()))
                 .ReturnsAsync(default(Book));

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act 
            var result = await controller.Delete("42") as NotFoundResult; ;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }
    }
}
