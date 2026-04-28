using Xunit;
using Bws.Bible.Infrastructure.Data;

namespace Bws.Bible.Api.Tests.Infrastructure.Data
{
    public class BibleDataTests
    {
        [Fact]
        public void TestBibleDataConstructor()
        {
            var bible = new BibleData();

            Assert.NotNull(bible.Verses);
            Assert.Empty(bible.Verses);

            Assert.NotNull(bible.Books);
            Assert.Empty(bible.Books);

            Assert.NotNull(bible.Statistics);

        }

        [Fact]
        public void TestBibleDataDispose()
        {
            var bible = new BibleData();

            bible.Verses.Add(new VerseData() { IdBook = 1, IdChapter = 1, IdVerse = 1, Text = "Au commencement, Dieu créa les cieux et la terre." });
            bible.Books.Add(new BookData() { IdBook = 1, Title = "Genèse", Statistics = new BookData.ComputedData() { CountChapters = 50, CountVerses = 1533 } });

            bible.Dispose();

            Assert.NotNull(bible.Verses);
            Assert.Empty(bible.Verses);

            Assert.NotNull(bible.Books);
            Assert.Empty(bible.Books);
        }
    }
}
