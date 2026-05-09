using Xunit;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Api.Tests.Infrastructure.Data;

public class BookDataTests
{
    [Fact]
    public void TestBookDataConstructor()
    {
        var bible = new BookData() { IdBook = 1, Title = "Genèse" };

        Assert.NotNull(bible.Statistics);
    }
}
