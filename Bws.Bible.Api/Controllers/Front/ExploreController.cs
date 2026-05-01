using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Extensions;
using Bws.Bible.Core.Exceptions;
using Bws.Bible.Core.Exceptions.Enums;
using Bws.Bible.Api.Models.Explore;
using Bws.Bible.Api.Models.Explore.Items;

namespace Bws.Bible.Api.Controllers.Front;

/// <summary>
/// Explorer la Bible
/// </summary>
[Route("explore")]
[ApiController]
public class ExploreController : BaseApiController
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public ExploreController(IApiSettings apiSettings, IBibleRepository bibleRepository, ILogger<ExploreController> logger)
        : base(apiSettings, bibleRepository, logger)
    {
    }

    /// <summary>
    /// Obtenir la liste des Bibles disponibles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ExploreBiblesResponse), 200)]
    public JsonResult GetBibles()
    {
        var result = new ExploreBiblesResponse();

        var bibles = BibleRepository.GetBibles(null, false);
        if (bibles == null || !bibles.Any())
        {
            return NoDataFoundResponse();
        }

        result.Bibles = new List<BibleInfoItem>();
        foreach (var bible in bibles)
        {
            result.Bibles.Add(new BibleInfoItem
            {
                IdBible = bible.Id,
                Name = bible.Name,
                Language = bible.Language.ToString(),
                Translator = bible.Translator,
                ReleaseYear = bible.ReleaseYear,
                GoToBible = ApiSettings.GenerateNavigationLinks ? new Uri(ApiBaseUri, $"{Request.Path.Value}/{bible.Id}").ToString() : null
            });
        }
        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Obtenir les informations sur une Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    [HttpGet("{bible}")]
    [ProducesResponseType(typeof(ExploreBibleResponse), 200)]
    public JsonResult GetBible(string bible)
    {
        if (!CheckIdBible(bible))
        {
            return InvalidParameterResponse();
        }

        var result = new ExploreBibleResponse();

        BibleDto dto = null;
        try
        {
            dto = BibleRepository.GetBible(bible, false);
            if (dto == null)
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

        result.Bible = new BibleItem()
        {
            IdBible = dto.Id,
            Language = dto.Language.ToString(),
            Name = dto.Name,
            ReleaseYear = dto.ReleaseYear,
            Translator = dto.Translator,
            CountVerses = dto.CountVerses,
            CountBooks = dto.CountBooks,
            Books = new List<BookInfoItem>(),
        };
        result.Bible.Books.AddRange(
            dto.Books.Select(b => new BookInfoItem()
            {
                IdBook = b.IdBook,
                Name = b.Title,
                CountChapters = b.CountChapters,
                CountVerses = b.CountVerses,
                GoToBook = ApiSettings.GenerateNavigationLinks ? new Uri(ApiBaseUri, $"{Request.Path.Value}/{b.IdBook}").ToString() : null
            })
        );
        result.Navigation = ApiSettings.GenerateNavigationLinks ? BuildNavigationForGetBible(dto.Id) : null;

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Obtenir les informations sur un livre de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    [HttpGet("{bible}/{book}")]
    [ProducesResponseType(typeof(ExploreBibleBookResponse), 200)]
    public JsonResult GetBibleBook(string bible, string book)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckIdBook(idBook))
        {
            return InvalidParameterResponse();
        }

        var result = new ExploreBibleBookResponse();

        var dto = BibleRepository.GetBook(bible, idBook, false);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        result.IdBible = bible;
        result.Book = new BookItem()
        {
            IdBook = dto.IdBook,
            Name = dto.Title,
            CountChapters = dto.CountChapters,
            CountVerses = dto.CountVerses,
            GoToFirstChapter = ApiSettings.GenerateNavigationLinks ? new Uri(ApiBaseUri, $"{Request.Path.Value}/1").ToString() : null
        };
        result.Navigation = ApiSettings.GenerateNavigationLinks ? BuildNavigationForGetBibleBook(bible, idBook, book) : null;

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Obtenir le chapitre d'un livre de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    [HttpGet("{bible}/{book}/{chapter:min(1):max(255)}")]
    [ProducesResponseType(typeof(ExploreBibleChapterResponse), 200)]
    public JsonResult GetBibleBookChapter(string bible, string book, byte chapter)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var result = new ExploreBibleChapterResponse();

        var dto = BibleRepository.GetBook(bible, idBook, true);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        result.IdBible = bible;
        result.IdBook = idBook;
        result.Chapter = new ChapterItem()
        {
            IdChapter = chapter,
            CountVerses = (byte)dto.Verses.Where(v => v.IdChapter == chapter).Count(),
            Verses = dto.Verses.Where(v => v.IdChapter == chapter).Select(v => new VerseItem() { IdVerse = v.IdVerse, Verse = v.Text }).ToList()
        };
        result.Navigation = ApiSettings.GenerateNavigationLinks ? BuildNavigationForGetBibleBookChapter(bible, book, chapter, dto.CountChapters) : null;

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Obtenir un verset de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="verse">Numéro du verset</param>
    /// <returns></returns>
    [HttpGet("{bible}/{book}/{chapter:min(1):max(255)}:{verse:min(1):max(255)}")]
    [ProducesResponseType(typeof(ExploreBibleVerseResponse), 200)]
    public JsonResult GetBibleVerse(string bible, string book, byte chapter, byte verse)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var result = new ExploreBibleVerseResponse();

        var dto = BibleRepository.GetVerses(bible, idBook, chapter, verse, verse);
        if (dto == null || !dto.Any())
        {
            return NoDataFoundResponse();
        }

        result.IdBible = bible;
        result.IdBook = idBook;
        result.IdChapter = chapter;
        result.Verse = new VerseItem()
        {
            IdVerse = verse,
            Verse = dto.FirstOrDefault(v => v.IdVerse == verse).Text
        };
        result.Navigation = ApiSettings.GenerateNavigationLinks ? BuildNavigationForGetBibleVerse(bible, book, chapter, verse) : null;

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Obtenir des versets de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="firstVerse">Numéro du premier verset</param>
    /// <param name="lastVerse">Numéro du dernier verset</param>
    /// <returns></returns>
    [HttpGet("{bible}/{book}/{chapter:min(1):max(255)}:{firstVerse:min(1):max(255)}-{lastVerse:min(1):max(255)}")]
    [ProducesResponseType(typeof(ExploreBibleVersesResponse), 200)]
    public JsonResult GetBibleVerses(string bible, string book, byte chapter, byte firstVerse, byte lastVerse)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckIdBook(idBook) || !CheckIdChapter(chapter) || firstVerse > lastVerse)
        {
            return InvalidParameterResponse();
        }

        var result = new ExploreBibleVersesResponse();

        var dto = BibleRepository.GetVerses(bible, idBook, chapter, firstVerse, lastVerse);
        if (dto == null || !dto.Any())
        {
            return NoDataFoundResponse();
        }

        result.IdBible = bible;
        result.IdBook = idBook;
        result.IdChapter = chapter;
        result.Verses = dto.Where(v => v.IdChapter == chapter).Select(v => new VerseItem() { IdVerse = v.IdVerse, Verse = v.Text }).ToList();
        result.Navigation = ApiSettings.GenerateNavigationLinks ? BuildNavigationForGetBibleVerses(bible, book, chapter, firstVerse, lastVerse) : null;

        return result.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    #region Build navigation private methods

    /// <summary>
    /// Générer les liens de navigation pour la méthode GetBible
    /// </summary>
    private BibleNavigationItem BuildNavigationForGetBible(string bible)
    {
        var navigation = new BibleNavigationItem()
        {
            GoToBibles = new Uri(ApiBaseUri, $"{Request.Path.Value}").ToString().TrimEnd($"/{bible}", StringComparison.InvariantCultureIgnoreCase)
        };
        return navigation;
    }

    /// <summary>
    /// Générer les liens de navigation pour la méthode GetBibleBook
    /// </summary>
    private BookNavigationItem BuildNavigationForGetBibleBook(string bible, byte idBook, string book)
    {
        string navigationBookBaseUrl = new Uri(ApiBaseUri, $"{Request.Path.Value}").ToString().TrimEnd($"/{book}", StringComparison.InvariantCulture);
        var navigation = new BookNavigationItem()
        {
            GoToNextBook = (idBook + 1) > 66 ? null : $"{navigationBookBaseUrl}/{idBook + 1}",
            GoToPreviousBook = (idBook - 1) < 1 ? null : $"{navigationBookBaseUrl}/{idBook - 1}",
            GoToBible = navigationBookBaseUrl,
            GoToBibles = new Uri(ApiBaseUri, $"{navigationBookBaseUrl}").ToString().TrimEnd($"/{bible}", StringComparison.InvariantCultureIgnoreCase),
        };
        return navigation;
    }

    /// <summary>
    /// Générer les liens de navigation pour la méthode GetBibleBookChapter
    /// </summary>
    private ChapterNavigationItem BuildNavigationForGetBibleBookChapter(string bible, string book, byte chapter, byte lastChapterOfBook)
    {
        string navigationChapterBaseUrl = new Uri(ApiBaseUri, $"{Request.Path.Value}").ToString().TrimEnd($"/{chapter}", StringComparison.InvariantCulture);
        var navigation = new ChapterNavigationItem()
        {
            GoToNextChapter = (chapter + 1) > lastChapterOfBook ? null : $"{navigationChapterBaseUrl}/{chapter + 1}",
            GoToPreviousChapter = (chapter - 1) < 1 ? null : $"{navigationChapterBaseUrl}/{chapter - 1}",
            GoToBook = navigationChapterBaseUrl,
            GoToBible = new Uri(ApiBaseUri, $"{navigationChapterBaseUrl}").ToString().TrimEnd($"/{book}", StringComparison.InvariantCulture),
            GoToBibles = new Uri(ApiBaseUri, $"{navigationChapterBaseUrl}").ToString().TrimEnd($"/{bible}/{book}", StringComparison.InvariantCultureIgnoreCase),
        };
        return navigation;
    }

    /// <summary>
    /// Générer les liens de navigation pour la méthode GetBibleVerse
    /// </summary>
    private VerseNavigationItem BuildNavigationForGetBibleVerse(string bible, string book, byte chapter, byte verse)
    {
        string navigationVerseBaseUrl = new Uri(ApiBaseUri, $"{Request.Path.Value}").ToString().TrimEnd($":{verse}", StringComparison.InvariantCulture);
        var navigation = new VerseNavigationItem()
        {
            GoToChapter = navigationVerseBaseUrl,
            GoToBook = new Uri(ApiBaseUri, $"{navigationVerseBaseUrl}").ToString().TrimEnd($"/{chapter}", StringComparison.InvariantCulture),
            GoToBible = new Uri(ApiBaseUri, $"{navigationVerseBaseUrl}").ToString().TrimEnd($"/{book}/{chapter}", StringComparison.InvariantCulture),
            GoToBibles = new Uri(ApiBaseUri, $"{navigationVerseBaseUrl}").ToString().TrimEnd($"/{bible}/{book}/{chapter}", StringComparison.InvariantCultureIgnoreCase),
        };
        return navigation;
    }

    /// <summary>
    /// Générer les liens de navigation pour la méthode GetBibleVerses
    /// </summary>
    private VerseNavigationItem BuildNavigationForGetBibleVerses(string bible, string book, byte chapter, byte firstVerse, byte lastVerse)
    {
        string navigationVersesBaseUrl = new Uri(ApiBaseUri, $"{Request.Path.Value}").ToString().TrimEnd($":{firstVerse}-{lastVerse}", StringComparison.InvariantCulture);
        var navigation = new VerseNavigationItem()
        {
            GoToChapter = navigationVersesBaseUrl,
            GoToBook = new Uri(ApiBaseUri, $"{navigationVersesBaseUrl}").ToString().TrimEnd($"/{chapter}", StringComparison.InvariantCulture),
            GoToBible = new Uri(ApiBaseUri, $"{navigationVersesBaseUrl}").ToString().TrimEnd($"/{book}/{chapter}", StringComparison.InvariantCulture),
            GoToBibles = new Uri(ApiBaseUri, $"{navigationVersesBaseUrl}").ToString().TrimEnd($"/{bible}/{book}/{chapter}", StringComparison.InvariantCultureIgnoreCase),
        };
        return navigation;
    }

    #endregion Build navigation private methods
}
