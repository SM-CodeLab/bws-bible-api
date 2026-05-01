using Xunit;
using Bws.Bible.Core.Domain;

namespace Bws.Bible.Api.Tests.Core.Domain;

public class BibleDtoTests
{
    [Fact]
    public void TestBibleDtoConstructor()
    {
        var bible = new BibleDto();

        Assert.NotNull(bible.Books);
        Assert.Empty(bible.Books);
    }
}
