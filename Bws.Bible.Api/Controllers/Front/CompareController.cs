using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bws.Bible.Core.Extensions;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Exceptions;
using Bws.Bible.Core.Exceptions.Enums;
using Bws.Bible.Core.Domain;
using Bws.Bible.Api.Models.Compare;
using Bws.Bible.Api.Models.Compare.Items;

namespace Bws.Bible.Api.Controllers.Front;

/// <summary>
/// Comparer la Bible
/// </summary>
[Route("compare")]
[ApiController]
public class CompareController : BaseApiController
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public CompareController(IApiSettings apiSettings, IBibleRepository bibleRepository, ILogger<CompareController> logger)
        : base(apiSettings, bibleRepository, logger)
    {
    }

    /// <summary>
    /// Liste des méthodes de comparaison
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CompareInfoResponse), 200)]
    public JsonResult CompareInfo()
    {
        string apiUrl = $"{ApiSettings.BaseUrl}{Request.Path.Value}";

        var response = new CompareInfoResponse(apiUrl, ApiSettings);

        return new JsonResult(response);
    }

    /// <summary>
    /// Comparer deux versions de la Bible
    /// </summary>
    /// <param name="bibleA">Identifiant de la première Bible</param>
    /// <param name="bibleB">Identifiant de la seconde Bible</param>
    [HttpGet("{bibleA}/{bibleB}")]
    [ProducesResponseType(typeof(CompareBiblesResponse), 200)]
    public JsonResult CompareTwoBibles(string bibleA, string bibleB)
    {
        if (!CheckIdBible(bibleA) || !CheckIdBible(bibleB))
        {
            return InvalidParameterResponse();
        }

        var result = new CompareBiblesResponse();

        BibleDto bibleAdto = null, bibleBdto = null;
        try
        {
            bibleAdto = BibleRepository.GetBible(bibleA, false);
            bibleBdto = BibleRepository.GetBible(bibleB, false);

            if (bibleAdto == null || bibleBdto == null)
            {
                return NoDataFoundResponse();
            }
        }
        catch (InfrastructureException e)
        {
            if (e.ErrorCode == EInfrastructureErrorCode.LoadBibleFull)
            {
                Logger.LogError($"BibleRepository.GetBible throws an InfrastructureException : [{e.ErrorCode}] {e.ErrorMessage}");
            }
            return NoDataFoundResponse();
        }

        result.IdBibleA = bibleA;
        result.IdBibleB = bibleB;

        result.BibleComparison = new BibleComparisonItem()
        {
            Name = $"{bibleAdto.Name} ({bibleAdto.Id}) {GetOperatorForStringComparison(bibleAdto.Name, bibleBdto.Name)} {bibleBdto.Name} ({bibleBdto.Id})",
            Language = $"{bibleAdto.Language} ({bibleAdto.Id}) {GetOperatorForStringComparison(bibleAdto.Language.ToString(), bibleBdto.Language.ToString())} {bibleBdto.Language} ({bibleBdto.Id})",
            ReleaseYear = $"{bibleAdto.ReleaseYear} ({bibleAdto.Id}) {GetOperatorForNumericComparison(bibleAdto.ReleaseYear, bibleBdto.ReleaseYear)} {bibleBdto.ReleaseYear} ({bibleBdto.Id})",
            Translator = $"{bibleAdto.Translator} ({bibleAdto.Id}) {GetOperatorForStringComparison(bibleAdto.Translator, bibleBdto.Translator)} {bibleBdto.Translator} ({bibleBdto.Id})",
            CountBooks = $"{bibleAdto.CountBooks} ({bibleAdto.Id}) {GetOperatorForNumericComparison(bibleAdto.CountBooks, bibleBdto.CountBooks)} {bibleBdto.CountBooks} ({bibleBdto.Id})",
            CountVerses = $"{bibleAdto.CountVerses} ({bibleAdto.Id}) {GetOperatorForNumericComparison(bibleAdto.CountVerses, bibleBdto.CountVerses)} {bibleBdto.CountVerses} ({bibleBdto.Id})",
            BooksComparison = new List<BibleComparisonBookItem>()
        };

        List<byte> idBooks = bibleAdto.Books.Select(a => a.IdBook).Union(bibleBdto.Books.Select(b => b.IdBook)).ToList();
        foreach (byte idBook in idBooks)
        {
            var bookA = bibleAdto.Books.FirstOrDefault(a => a.IdBook == idBook);
            var bookB = bibleBdto.Books.FirstOrDefault(b => b.IdBook == idBook);
            var bookComparison = new BibleComparisonBookItem();

            if (bookA != null && bookB != null)
            {
                if (bookA.CountChapters == bookB.CountChapters && bookA.CountVerses == bookB.CountVerses)
                {
                    continue;
                }

                bookComparison.BookTitle = GetResultOfBookTitleComparison(bookA.Title, bookB.Title, bibleAdto.Id, bibleBdto.Id);
                bookComparison.CountChapters = $"{bookA.CountChapters} ({bibleAdto.Id}) {GetOperatorForNumericComparison(bookA.CountChapters, bookB.CountChapters)} {bookB.CountChapters} ({bibleBdto.Id})";
                bookComparison.CountVerses = $"{bookA.CountVerses} ({bibleAdto.Id}) {GetOperatorForNumericComparison(bookA.CountVerses, bookB.CountVerses)} {bookB.CountVerses} ({bibleBdto.Id})";
            }
            if (bookA != null && bookB == null)
            {
                bookComparison.BookTitle = bookA.Title;
                bookComparison.CountChapters = $"{bookA.CountChapters} ({bibleAdto.Id}) > 0 ({bibleBdto.Id})";
                bookComparison.CountVerses = $"{bookA.CountVerses} ({bibleAdto.Id}) > 0 ({bibleBdto.Id})";
            }
            if (bookA == null && bookB != null)
            {
                bookComparison.BookTitle = bookB.Title;
                bookComparison.CountChapters = $"0 ({bibleAdto.Id}) < {bookB.CountChapters} ({bibleBdto.Id})";
                bookComparison.CountVerses = $"0 ({bibleAdto.Id}) < {bookB.CountVerses} ({bibleBdto.Id})";
            }

            bookComparison.IdBook = idBook;

            result.BibleComparison.BooksComparison.Add(bookComparison);
        }

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }


    /// <summary>
    /// Comparer un livre dans deux versions de la Bible
    /// </summary>
    /// <param name="bibleA">Identifiant de la première Bible</param>
    /// <param name="bibleB">Identifiant de la seconde Bible</param>
    /// <param name="book">Identifiant du livre</param>
    [HttpGet("{bibleA}/{bibleB}/{book}")]
    [ProducesResponseType(typeof(CompareBooksResponse), 200)]
    public JsonResult CompareTwoBooks(string bibleA, string bibleB, string book)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bibleA) || !CheckIdBible(bibleB) || !CheckIdBook(idBook))
        {
            return InvalidParameterResponse();
        }

        var result = new CompareBooksResponse();

        var dtoA = BibleRepository.GetBook(bibleA, idBook, true);
        var dtoB = BibleRepository.GetBook(bibleB, idBook, true);
        if (dtoA == null || dtoB == null)
        {
            return NoDataFoundResponse();
        }

        result.IdBibleA = bibleA;
        result.IdBibleB = bibleB;
        result.IdBook = idBook;

        int countWordsIntoBookA = CountWordsIntoVerses(dtoA.Verses);
        int countWordsIntoBookB = CountWordsIntoVerses(dtoB.Verses);
        result.BookComparison = new BookComparisonItem()
        {
            BookTitle = GetResultOfBookTitleComparison(dtoA.Title, dtoB.Title, bibleA, bibleB),
            CountChapters = $"{dtoA.CountChapters} ({bibleA}) {GetOperatorForNumericComparison(dtoA.CountChapters, dtoB.CountChapters)} {dtoB.CountChapters} ({bibleB})",
            CountVerses = $"{dtoA.CountVerses} ({bibleA}) {GetOperatorForNumericComparison(dtoA.CountChapters, dtoB.CountChapters)} {dtoB.CountVerses} ({bibleB})",
            CountWords = $"{countWordsIntoBookA} ({bibleA}) {GetOperatorForNumericComparison(countWordsIntoBookA, countWordsIntoBookB)} {countWordsIntoBookB} ({bibleB})",
        };

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Comparer un chapitre dans deux versions de la Bible
    /// </summary>
    /// <param name="bibleA">Identifiant de la première Bible</param>
    /// <param name="bibleB">Identifiant de la seconde Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    [HttpGet("{bibleA}/{bibleB}/{book}/{chapter:min(1):max(255)}")]
    [ProducesResponseType(typeof(CompareChaptersResponse), 200)]
    public JsonResult CompareTwoChapters(string bibleA, string bibleB, string book, byte chapter)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bibleA) || !CheckIdBible(bibleB) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var result = new CompareChaptersResponse();

        var dtoA = BibleRepository.GetBook(bibleA, idBook, true);
        var dtoB = BibleRepository.GetBook(bibleB, idBook, true);
        if (dtoA == null || dtoB == null)
        {
            return NoDataFoundResponse();
        }

        result.IdBibleA = bibleA;
        result.IdBibleB = bibleB;
        result.IdBook = idBook;
        result.IdChapter = chapter;
        result.BookTitle = GetResultOfBookTitleComparison(dtoA.Title, dtoB.Title, bibleA, bibleB);

        var versesChapterA = dtoA.Verses.Where(v => v.IdChapter == chapter);
        var versesChapterB = dtoB.Verses.Where(v => v.IdChapter == chapter);
        int countWordsIntoChapterA = CountWordsIntoVerses(versesChapterA);
        int countWordsIntoChapterB = CountWordsIntoVerses(versesChapterB);
        result.ChapterComparison = new ChapterComparisonItem()
        {
            CountVerses = $"{versesChapterA.Count()} ({bibleA}) {GetOperatorForNumericComparison(versesChapterA.Count(), versesChapterB.Count())} {versesChapterB.Count()} ({bibleB})",
            CountWords = $"{countWordsIntoChapterA} ({bibleA}) {GetOperatorForNumericComparison(countWordsIntoChapterA, countWordsIntoChapterB)} {countWordsIntoChapterB} ({bibleB})"
        };

        if (ApiSettings.CompareInterlinearVersesEnabled)
        {
            result.Verses = GetVersesInterlinear(bibleA, bibleB, versesChapterA.ToList(), versesChapterB.ToList());
        }

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Comparer un verset dans deux versions de la Bible
    /// </summary>
    /// <param name="bibleA">Identifiant de la première Bible</param>
    /// <param name="bibleB">Identifiant de la seconde Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="verse">Numéro du verset</param>
    [HttpGet("{bibleA}/{bibleB}/{book}/{chapter:min(1):max(255)}:{verse:min(1):max(255)}")]
    [ProducesResponseType(typeof(CompareVerseResponse), 200)]
    public JsonResult CompareTwoVerses(string bibleA, string bibleB, string book, byte chapter, byte verse)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bibleA) || !CheckIdBible(bibleB) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var result = new CompareVerseResponse()
        {
            IdBibleA = bibleA,
            IdBibleB = bibleB,
            IdBook = idBook,
            IdChapter = chapter,
            IdVerse = verse
        };

        var verseA = BibleRepository.GetVerses(bibleA, idBook, chapter, verse, verse);
        var verseB = BibleRepository.GetVerses(bibleB, idBook, chapter, verse, verse);
        if (verseA == null || verseB == null || !verseA.Any() || !verseB.Any())
        {
            return NoDataFoundResponse();
        }
        int countWordsIntoVerseA = CountWordsIntoVerses(verseA);
        int countWordsIntoVerseB = CountWordsIntoVerses(verseB);

        result.VerseComparison = new VerseComparisonItem()
        {
            CountWords = $"{countWordsIntoVerseA} ({bibleA}) {GetOperatorForNumericComparison(countWordsIntoVerseA, countWordsIntoVerseB)} {countWordsIntoVerseB} ({bibleB})"
        };

        if (ApiSettings.CompareInterlinearVersesEnabled)
        {
            result.Verses = GetVersesInterlinear(bibleA, bibleB, verseA, verseB);
        }

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }




    /// <summary>
    /// Comparer un groupe de versets dans deux versions de la Bible
    /// </summary>
    /// <param name="bibleA">Identifiant de la première Bible</param>
    /// <param name="bibleB">Identifiant de la seconde Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="firstVerse">Numéro du premier verset</param>
    /// <param name="lastVerse">Numéro du dernier verset</param>
    [HttpGet("{bibleA}/{bibleB}/{book}/{chapter:min(1):max(255)}:{firstVerse:min(1):max(255)}-{lastVerse:min(1):max(255)}")]
    [ProducesResponseType(typeof(CompareVerseResponse), 200)]
    public JsonResult CompareTwoGroupsOfVerses(string bibleA, string bibleB, string book, byte chapter, byte firstVerse, byte lastVerse)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bibleA) || !CheckIdBible(bibleB) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var result = new CompareVersesResponse()
        {
            IdBibleA = bibleA,
            IdBibleB = bibleB,
            IdBook = idBook,
            IdChapter = chapter,
            IdFirstVerse = firstVerse,
            IdLastVerse = lastVerse
        };

        var versesA = BibleRepository.GetVerses(bibleA, idBook, chapter, firstVerse, lastVerse);
        var versesB = BibleRepository.GetVerses(bibleB, idBook, chapter, firstVerse, lastVerse);
        if (versesA == null || versesB == null || !versesA.Any() || !versesB.Any())
        {
            return NoDataFoundResponse();
        }
        int countWordsIntoVerseA = CountWordsIntoVerses(versesA);
        int countWordsIntoVerseB = CountWordsIntoVerses(versesB);

        result.VersesComparison = new VerseComparisonItem()
        {
            CountWords = $"{countWordsIntoVerseA} ({bibleA}) {GetOperatorForNumericComparison(countWordsIntoVerseA, countWordsIntoVerseB)} {countWordsIntoVerseB} ({bibleB})"
        };

        if (ApiSettings.CompareInterlinearVersesEnabled)
        {
            result.Verses = GetVersesInterlinear(bibleA, bibleB, versesA, versesB);
        }

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    #region Private methods

    /// <summary>
    /// Obtenir l'opérateur de comparaison de 2 nombres
    /// </summary>
    private string GetOperatorForNumericComparison(int numberA, int numberB)
    {
        if (numberA == numberB)
        {
            return "=";
        }
        if (numberA > numberB)
        {
            return ">";
        }
        return "<";
    }

    /// <summary>
    /// Obtenir l'opérateur de comparaison de 2 chaines de caractères
    /// </summary>
    private string GetOperatorForStringComparison(string stringA, string stringB)
    {
        if (stringA.ToLowerInvariant() == stringB.ToLowerInvariant())
        {
            return "=";
        }
        return "<>";
    }

    /// <summary>
    /// Obtenir le résultat de la comparaison du titre d'un livre de la Bible
    /// </summary>
    private string GetResultOfBookTitleComparison(string stringA, string stringB, string idBibleA = null, string idBibleB = null)
    {
        if (stringA.ToLowerInvariant() == stringB.ToLowerInvariant())
        {
            return stringA;
        }
        if (!string.IsNullOrEmpty(idBibleA) && !string.IsNullOrEmpty(idBibleB))
        {
            return $"{stringA} ({idBibleA}) <> {stringB} ({idBibleB})";
        }
        return $"{stringA} <> {stringB}";
    }

    /// <summary>
    /// Compter le nombre de mots dans un verset
    /// </summary>
    private int CountWordsIntoVerses(IEnumerable<VerseDto> verses)
    {
        return verses.Sum(v => v.Text.CountWords());
    }

    /// <summary>
    /// Obtenir une liste de versets de 2 Bibles en mode interlinéaire
    /// </summary>
    private List<InterlinearVerseItem> GetVersesInterlinear(string idBibleA, string idBibleB, List<VerseDto> versesBibleA, List<VerseDto> versesBibleB)
    {
        var verses = new List<InterlinearVerseItem>();
        verses.AddRange(versesBibleA.Select(v => new InterlinearVerseItem() { IdBible = idBibleA, IdVerse = v.IdVerse, Verse = v.Text }));
        verses.AddRange(versesBibleB.Select(v => new InterlinearVerseItem() { IdBible = idBibleB, IdVerse = v.IdVerse, Verse = v.Text }));
        verses = verses.OrderBy(v => v.IdVerse).ThenBy(v => v.IdBible).ToList();
        return verses;
    }

    #endregion Private methods
}
