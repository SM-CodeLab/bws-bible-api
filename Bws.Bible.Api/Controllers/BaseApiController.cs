using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Api.Routing;
using Bws.Bible.Api.Models.Common;
using System.Net;

namespace Bws.Bible.Api.Controllers
{
    /// <summary>
    /// Classe mère des contrôleurs de l'API Bible
    /// </summary>
    public abstract class BaseApiController : ControllerBase
    {
        protected IBibleRepository BibleRepository;
        protected IApiSettings ApiSettings;
        protected readonly Uri ApiBaseUri;
        protected readonly ILogger<BaseApiController> Logger;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public BaseApiController(IApiSettings apiSettings, IBibleRepository bibleRepository, ILogger<BaseApiController> logger)
        {
            ApiSettings = apiSettings;
            BibleRepository = bibleRepository;
            ApiBaseUri = new Uri(apiSettings.BaseUrl);
            Logger = logger;
        }

        /// <summary>
        /// Générer une réponse générique lorsque les paramètres sont invalide (= erreur http 400)
        /// </summary>
        protected JsonResult InvalidParameterResponse()
        {
            var response = new InvalidParameterResponse
            {
                Errors = new List<string> { "Bad request : Invalid parameters detected" }, //TODO : Use .resx file for the text message
            };

            return new JsonResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        /// <summary>
        /// Générer une réponse générique lorsqu'aucune donnée n'a été trouvée (= erreur http 404)
        /// </summary>
        protected JsonResult NoDataFoundResponse()
        {
            var response = new NotFoundResponse
            {
                Message = "No data found for your request" //TODO : Use .resx file for the text message
            };

            return new JsonResult(response) { StatusCode = (int)HttpStatusCode.NotFound };
        }

        /// <summary>
        /// Vérifier la validité d'un identifiant de Bible
        /// </summary>
        /// <param name="bible">Identifiant de Bible</param>
        /// <returns>Retourne "true" si l'identifiant de Bible est valide.</returns>
        protected bool CheckIdBible(string bible)
        {
            Regex myRegex = new Regex(ApiSettings.RegexBibleIdentifier);
            return myRegex.IsMatch(bible);
        }

        /// <summary>
        /// Obtenir le numéro d'un livre en résolvant son titre ou son abréviation
        /// </summary>
        /// <param name="book">Titre ou abréviation d'un livre de la Bible</param>
        /// <returns>Retourne le numéro du livre (entre 1 et 66), ou 0 en cas d'échec.</returns>
        protected byte ResolveIdBook(string book)
        {
            return BookResolver.GetBookNumber(book);
        }

        /// <summary>
        /// Vérifier la validité d'un numéro de livre
        /// </summary>
        /// <param name="idBook">Numéro de livre</param>
        /// <returns>Retourne "true" si le numéro de livre est valide.</returns>
        protected bool CheckIdBook(byte idBook)
        {
            if (idBook < 1 || idBook > 66)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Vérifier la validité d'un numéro de chapitre
        /// </summary>
        /// <param name="idChapter">Numéro de chapitre</param>
        /// <returns>Retourne "true" si le numéro de chapitre est valide.</returns>
        protected bool CheckIdChapter(byte idChapter)
        {
            if (idChapter < 1 || idChapter > 150)
                return false;
            else
                return true;
        }
    }
}
