using Xunit;
using System;
using Bws.Bible.Core.Extensions;

namespace Bws.Bible.Api.Tests.Core.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void TestTrimEndsForUrls()
    {
        string urlTestInput = "http://hostname.org/api-bible/LSG";
        string urlTestExpected = "http://hostname.org/api-bible";

        string urlTest1 = urlTestInput.TrimEnd("/LSG", StringComparison.InvariantCultureIgnoreCase);
        string urlTest2 = urlTestInput.TrimEnd("/lsg", StringComparison.InvariantCultureIgnoreCase);
        string urlTest3 = urlTestInput.TrimEnd("/Lsg", StringComparison.InvariantCultureIgnoreCase);
        string urlTest4 = urlTestInput.TrimEnd("/LSG", StringComparison.InvariantCulture);
        string urlTest5 = urlTestInput.TrimEnd("/lsg", StringComparison.InvariantCulture);
        string urlTest6 = urlTestInput.TrimEnd("/Lsg", StringComparison.InvariantCulture);
        string urlTest7 = urlTestInput.TrimEnd("LSG", StringComparison.InvariantCulture);

        Assert.Equal(urlTest1, urlTestExpected);
        Assert.Equal(urlTest2, urlTestExpected);
        Assert.Equal(urlTest3, urlTestExpected);
        Assert.Equal(urlTest4, urlTestExpected);
        Assert.Equal(urlTest5, urlTestInput);
        Assert.Equal(urlTest6, urlTestInput);
        Assert.Equal(urlTest7, string.Concat(urlTestExpected, "/"));
    }

    [Fact]
    public void TestCountWords()
    {
        string wordsTest1Input = "Au commencement, Dieu créa les cieux et la terre.";
        string wordsTest2Input = " Au  commencement,  Dieu  créa  les  cieux  et  la  terre. ";
        string wordsTest3Input = "  Au  commencement ,  Dieu  créa  les  cieux   et   la  terre .  ";
        string wordsTest4Input = "Aujourd'hui nous voyons au moyen d'un miroir, d'une manière obscure, mais alors nous verrons face à face; aujourd'hui je connais en partie, mais alors je connaîtrai comme j'ai été connu.";
        string wordsTest5Input = " ? !";
        string wordsTest6Input = string.Empty;
        string wordsTest7Input = "L'Éternel frappa les gens de Beth Schémesch, lorsqu'ils regardèrent l'arche de l'Éternel; il frappa [cinquante mille] soixante-dix hommes parmi le peuple. Et le peuple fut dans la désolation, parce que l'Éternel l'avait frappé d'une grande plaie.";

        Assert.Equal(9, wordsTest1Input.CountWords());
        Assert.Equal(9, wordsTest2Input.CountWords());
        Assert.Equal(9, wordsTest3Input.CountWords());
        Assert.Equal(30, wordsTest4Input.CountWords());
        Assert.Equal(0, wordsTest5Input.CountWords());
        Assert.Equal(0, wordsTest6Input.CountWords());
        Assert.Equal(36, wordsTest7Input.CountWords());
    }

    [Fact]
    public void TestRemoveDiacritics()
    {
        string input1 = "min: â, ê, î, ô, û, ä, ë, ï, ö, ü, à, ç, é, è, ù";
        string input2 = "MAJ: Â, Ê, Î, Ô, Û, Ä, Ë, Ï, Ö, Ü, À, Ç, É, È, Ù";

        string output1 = input1.RemoveDiacritics();
        string output2 = input2.RemoveDiacritics();

        Assert.Equal("min: a, e, i, o, u, a, e, i, o, u, a, c, e, e, u", output1);
        Assert.Equal("MAJ: A, E, I, O, U, A, E, I, O, U, A, C, E, E, U", output2);
    }
}
