using Xunit;
using Bws.Bible.Core.Domain;

namespace Bws.Bible.Api.Tests.Core.Domain
{
    public class BookDtoTests
    {
        [Fact]
        public void TestBookDtoConstructor()
        {
            var book = new BookDto();

            Assert.NotNull(book.Verses);
            Assert.Empty(book.Verses);
        }
    }
}
