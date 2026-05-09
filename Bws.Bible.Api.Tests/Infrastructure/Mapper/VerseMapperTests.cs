using Xunit;
using System.Collections.Generic;
using System.Linq;
using Bws.Bible.Core.Domain;
using Bws.Bible.Infrastructure.Data;
using Bws.Bible.Infrastructure.Mapper;

namespace Bws.Bible.Api.Tests.Infrastructure.Mapper;

public class VerseMapperTests
{
    [Fact]
    public void TestToVerseDto()
    {
        var verseData = new VerseData() { IdBook = 1, IdChapter = 1, IdVerse = 1, Text = "Au commencement, Dieu créa les cieux et la terre." };
        var verseDtoExpected = new VerseDto() { IdBook = 1, IdChapter = 1, IdVerse = 1, Text = "Au commencement, Dieu créa les cieux et la terre." };

        var verseDto = verseData.ToVerseDto();

        Assert.Equal(verseDtoExpected.IdBook, verseDto.IdBook);
        Assert.Equal(verseDtoExpected.IdChapter, verseDto.IdChapter);
        Assert.Equal(verseDtoExpected.IdVerse, verseDto.IdVerse);
        Assert.Equal(verseDtoExpected.Text, verseDto.Text);
    }

    [Fact]
    public void TestToVerseDtoMultiple()
    {
        var versesData = new List<VerseData>(){
            new VerseData() { IdBook = 1, IdChapter = 1, IdVerse = 1, Text = "Au commencement, Dieu créa les cieux et la terre." },
            new VerseData() { IdBook = 1, IdChapter = 1, IdVerse = 3, Text = "Dieu dit: Que la lumière soit! Et la lumière fut." }
        };
        var versesDtoExpected = new List<VerseDto> {
            new VerseDto() { IdBook = 1, IdChapter = 1, IdVerse = 1, Text = "Au commencement, Dieu créa les cieux et la terre." },
            new VerseDto() { IdBook = 1, IdChapter = 1, IdVerse = 3, Text = "Dieu dit: Que la lumière soit! Et la lumière fut." }
        };

        var versesDto = versesData.ToVerseDto().ToList();

        for (int i = 0; i < versesData.Count; i++)
        {
            Assert.Equal(versesDtoExpected[i].IdBook, versesDto[i].IdBook);
            Assert.Equal(versesDtoExpected[i].IdChapter, versesDto[i].IdChapter);
            Assert.Equal(versesDtoExpected[i].IdVerse, versesDto[i].IdVerse);
            Assert.Equal(versesDtoExpected[i].Text, versesDto[i].Text);
        }
    }
}