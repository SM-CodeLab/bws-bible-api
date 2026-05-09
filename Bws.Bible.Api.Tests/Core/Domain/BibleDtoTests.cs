using Xunit;
using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Domain.Enums;

namespace Bws.Bible.Api.Tests.Core.Domain;

public class BibleDtoTests
{
    [Fact]
    public void TestBibleDtoConstructor()
    {
        var bible = new BibleDto() { Id = "LSG", Name = "Bible Segond 1910", Translator = "Louis Segond", Language = ELanguage.FR };

        Assert.NotNull(bible.Books);
        Assert.Empty(bible.Books);
    }
}
