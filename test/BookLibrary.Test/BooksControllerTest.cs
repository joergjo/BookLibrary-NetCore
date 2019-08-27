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
        [Fact]
        public async Task Get_Returns_All_Books()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.FindAllAsync(It.IsInRange(1, 100, Range.Inclusive)))
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
            var books = actionResult?.Value;

            // Assert
            Assert.Equal(5, books?.Count());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-500)]
        public async Task Get_With_Invalid_Limit_Returns_BadRequest(int limit)
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.FindAllAsync(It.IsInRange(1, 100, Range.Inclusive)))
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
            var actionResult = await controller.Get(limit);
            var badRequestResult = actionResult.Result as BadRequestResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult?.StatusCode);
        }

        [Fact]
        public async Task Get_With_Valid_Id_Returns_Single_Book()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.FindAsync("1"))
                .ReturnsAsync(new Book { Id = "1" });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Get("1");
            var book = actionResult.Value as Book;

            // Assert
            Assert.Equal("1", book?.Id);
        }

        [Fact]
        public async Task Get_With_Invalid_Id_Returns_HttpNotFound()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(default(Book));

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Get("42");
            var result = actionResult.Result as NotFoundResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }

        [Fact]
        public async Task Post_Sets_RouteData_To_Book()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.AddAsync(It.IsAny<Book>()))
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

        [Fact]
        public async Task Post_Returns_Book_With_Id()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.AddAsync(It.IsAny<Book>()))
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

        [Fact]
        public async Task Post_Adds_Book_To_Repository()
        {
            // Arrange
            var mockRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Book>()))
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

        [Fact]
        public async Task Put_With_Valid_Id_Returns_Same_Book()
        {
            // Arrange
            var book = new Book { Id = "1" };
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.UpdateAsync("1", book))
                .ReturnsAsync(new Book { Id = "1" });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Put("1", book);
            var actualBook = actionResult?.Value as Book;

            // Assert
            Assert.Equal("1", actualBook?.Id);
        }

        [Fact]
        public async Task Put_With_Valid_Id_Saves_Book_To_Repository()
        {
            // Arrange
            var mockRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            mockRepository
                .Setup(x => x.UpdateAsync("1", It.IsAny<Book>()))
                .ReturnsAsync(new Book { Id = "1" })
                .Verifiable();

            var controller = new BooksController(mockRepository.Object, stubLogger.Object);

            // Act
            await controller.Put("1", new Book());

            // Assert
            mockRepository.Verify();
        }

        [Fact]
        public async Task Put_With_Invalid_Id_Returns_HttpNotFound()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Book>()))
                .ReturnsAsync(default(Book));

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var actionResult = await controller.Put("42", new Book());
            var result = actionResult.Result as NotFoundResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }

        [Fact]
        public async Task Delete_With_Valid_Id_Returns_HttpNoContent()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                 .Setup(x => x.RemoveAsync(It.Is<string>(s => s == "1")))
                 .ReturnsAsync(new Book { Id = "1" });

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act
            var result = await controller.Delete("1") as StatusCodeResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);
        }

        [Fact]
        public async Task Delete_With_Invalid_Id_Returns_NotFound()
        {
            // Arrange
            var stubRepository = new Mock<IBookRepository>();
            var stubLogger = new Mock<ILogger<BooksController>>();

            stubRepository
                 .Setup(x => x.RemoveAsync(It.IsAny<string>()))
                 .ReturnsAsync(default(Book));

            var controller = new BooksController(stubRepository.Object, stubLogger.Object);

            // Act 
            var result = await controller.Delete("42") as NotFoundResult; ;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }
    }
}
