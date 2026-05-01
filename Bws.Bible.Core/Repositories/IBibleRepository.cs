using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Domain.Enums;

namespace Bws.Bible.Core.Repositories;

/// <summary>
/// Interface BibleRepository
/// </summary>
public interface IBibleRepository
{
    /// <summary>
    /// Informations sur les bibles (sans versets ni statistiques)
    /// </summary>
    /// <param name="language">(optionnel) Filtre par langue</param>
    /// <param name="forceReload">(optionnel) Recharger les informations d'entête des Bibles</param>
    /// <returns>BibleDto (Data Transfert Object)</returns>
    List<BibleDto> GetBibles(ELanguage? language, bool forceReload);

    /// <summary>
    /// Récupérer une Bible
    /// </summary>
    /// <param name="idBible">Identifiant de la Bible</param>
    /// <param name="withVerses">Récupérer les versets</param>
    /// <returns>BibleDto (Data Transfert Object)</returns>
    BibleDto? GetBible(string idBible, bool withVerses);

    /// <summary>
    /// Récupérer un livre de la Bible
    /// </summary>
    /// <param name="idBible">Identifiant de la Bible</param>
    /// <param name="idBook">Numéro du livre</param>
    /// <param name="withVerses">Récupérer les versets</param>
    /// <returns>BookDto (Data Transfert Object)</returns>
    BookDto? GetBook(string idBible, byte idBook, bool withVerses);

    /// <summary>
    /// Récupérer des versets de la Bible
    /// </summary>
    /// <param name="idBible">Identifiant de la Bible</param>
    /// <param name="idBook">Numéro du livre</param>
    /// <param name="idChapter">Numéro du chapitre</param>
    /// <param name="idFirstVerse">Numéro du premier verset</param>
    /// <param name="idLastVerse">Numéro du dernier verset</param>
    /// <returns>Liste de VerseDto (Data Transfert Object)</returns>
    List<VerseDto>? GetVerses(string idBible, byte idBook, byte idChapter, byte idFirstVerse, byte idLastVerse);

    /// <summary>
    /// Rechercher des versets de la Bible qui contiennent des mots spécifiques
    /// Les filtres par livre, par chapitre et par verset(s) sont optionnels mais :
    /// - Le filtre par livre est obligatoire pour filtrer par chapitre
    /// - Le filtre par chapitre est obligatoire pour filtrer par verset(s)
    /// </summary>
    /// <param name="words">Mots recherchés</param>
    /// <param name="idBible">Identifiant de la Bible</param>
    /// <param name="idBook">(optionnel) Numéro du livre</param>
    /// <param name="idChapter">(optionnel) Numéro du chapitre</param>
    /// <param name="idFirstVerse">(optionnel) Numéro du premier verset</param>
    /// <param name="idLastVerse">(optionnel) Numéro du dernier verset</param>
    /// <returns>Liste de VerseDto (Data Transfert Object)</returns>
    List<VerseDto>? GetVersesBySearchingWords(string words, string idBible, byte? idBook = null, byte? idChapter = null, byte? idFirstVerse = null, byte? idLastVerse = null);
}
