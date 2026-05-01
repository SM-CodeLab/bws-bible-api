using Xunit;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Api.Tests.Infrastructure.Data;

public class BookDataTests
{
    [Fact]
    public void TestBookDataConstructor()
    {
        var bible = new BookData();

        Assert.NotNull(bible.Statistics);
    }
}
