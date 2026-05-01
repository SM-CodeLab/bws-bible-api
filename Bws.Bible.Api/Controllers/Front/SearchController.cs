using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Api.Models.Search;
using Bws.Bible.Api.Models.Search.Items;

namespace Bws.Bible.Api.Controllers.Front;

/// <summary>
/// Rechercher dans la Bible
/// </summary>
[Route("search")]
[ApiController]
public class SearchController : BaseApiController
{
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public SearchController(IApiSettings apiSettings, IBibleRepository bibleRepository, ILogger<SearchController> logger)
        : base(apiSettings, bibleRepository, logger)
    {
    }

    /// <summary>
    /// Informations sur l'API "/Search"
    /// </summary>
    [HttpGet]
    public JsonResult SearchInfo()
    {
        string apiUrl = $"{ApiSettings.BaseUrl}{Request.Path.Value}";

        var response = new SearchInfoResponse(apiUrl, ApiSettings);

        return new JsonResult(response);
    }

    /// <summary>
    /// Rechercher des mots ou une expression dans la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="words">Mot(s) recherché(s)</param>
    [HttpGet("{bible}/{words}")]
    [ProducesResponseType(typeof(SearchWordsResponse), 200)]
    public JsonResult SearchWordsInBible(string bible, string words)
    {
        if (!CheckIdBible(bible) || !CheckSearchWords(words))
        {
            return InvalidParameterResponse();
        }

        var dto = BibleRepository.GetVersesBySearchingWords(words, bible);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        SearchWordsResponse response = GenerateSearchWordsResponse(dto, words);

        return response.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Rechercher des mots ou une expression dans un livre de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="words">Mot(s) recherché(s)</param>
    [HttpGet("{bible}/{book}/{words}")]
    [ProducesResponseType(typeof(SearchWordsResponse), 200)]
    public JsonResult SearchWordsInBook(string bible, string book, string words)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckSearchWords(words) || !CheckIdBook(idBook))
        {
            return InvalidParameterResponse();
        }

        var dto = BibleRepository.GetVersesBySearchingWords(words, bible, idBook);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        SearchWordsResponse response = GenerateSearchWordsResponse(dto, words);

        return response.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Rechercher des mots ou une expression dans le chapitre d'un livre de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="words">Mot(s) recherché(s)</param>
    [HttpGet("{bible}/{book}/{chapter:min(1):max(255)}/{words}")]
    [ProducesResponseType(typeof(SearchWordsResponse), 200)]
    public JsonResult SearchWordsInChapter(string bible, string book, byte chapter, string words)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckSearchWords(words) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var dto = BibleRepository.GetVersesBySearchingWords(words, bible, idBook, chapter);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        SearchWordsResponse response = GenerateSearchWordsResponse(dto, words);

        return response.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Rechercher des mots ou une expression dans un verset de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="verse">Numéro du verset</param>
    /// <param name="words">Mot(s) recherché(s)</param>
    /// <returns></returns>
    [HttpGet("{bible}/{book}/{chapter:min(1):max(255)}:{verse:min(1):max(255)}/{words}")]
    [ProducesResponseType(typeof(SearchWordsResponse), 200)]
    public JsonResult SearchWordsInVerse(string bible, string book, byte chapter, byte verse, string words)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckSearchWords(words) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var dto = BibleRepository.GetVersesBySearchingWords(words, bible, idBook, chapter, verse);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        SearchWordsResponse response = GenerateSearchWordsResponse(dto, words);

        return response.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    /// <summary>
    /// Rechercher des mots ou une expression dans des versets de la Bible
    /// </summary>
    /// <param name="bible">Identifiant de la Bible</param>
    /// <param name="book">Identifiant du livre</param>
    /// <param name="chapter">Numéro du chapitre</param>
    /// <param name="firstVerse">Numéro du premier verset</param>
    /// <param name="lastVerse">Numéro du dernier verset</param>
    /// <param name="words">Mot(s) recherché(s)</param>
    /// <returns></returns>
    [HttpGet("{bible}/{book}/{chapter:min(1):max(255)}:{firstVerse:min(1):max(255)}-{lastVerse:min(1):max(255)}/{words}")]
    [ProducesResponseType(typeof(SearchWordsResponse), 200)]
    public JsonResult SearchWordsInVerses(string bible, string book, byte chapter, byte firstVerse, byte lastVerse, string words)
    {
        byte idBook = ResolveIdBook(book);
        if (!CheckIdBible(bible) || !CheckSearchWords(words) || !CheckIdBook(idBook) || !CheckIdChapter(chapter))
        {
            return InvalidParameterResponse();
        }

        var dto = BibleRepository.GetVersesBySearchingWords(words, bible, idBook, chapter, firstVerse, lastVerse);
        if (dto == null)
        {
            return NoDataFoundResponse();
        }

        SearchWordsResponse response = GenerateSearchWordsResponse(dto, words);

        return response.ToJsonResult(ApiSettings.GenerateResponseTime);
    }

    #region Private methods

    /// <summary>
    /// Vérifier le nombre de mots dans la recherche
    /// </summary>
    private bool CheckSearchWords(string words)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            return false;
        }
        words = words.Trim(' ');
        if (words.Length < ApiSettings.SearchWordsMinLength || words.Length > ApiSettings.SearchWordsMaxLength)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Générer le résultat d'une recherche de mots
    /// </summary>
    private SearchWordsResponse GenerateSearchWordsResponse(List<VerseDto> resultDto, string searchWords)
    {
        var response = new SearchWordsResponse();
        response.SearchResult = new List<VerseFoundItem>();
        foreach (var row in resultDto)
        {
            var verse = new VerseFoundItem()
            {
                IdBook = row.IdBook,
                IdChapter = row.IdChapter,
                IdVerse = row.IdVerse,
                Verse = row.Text,
                CountResult = (byte)Regex.Matches(row.Text.ToLowerInvariant(), searchWords.ToLowerInvariant()).Count
            };
            response.CountResult = (short)(response.CountResult + verse.CountResult);
            response.SearchResult.Add(verse);
        }
        return response;
    }

    #endregion Private methods
}