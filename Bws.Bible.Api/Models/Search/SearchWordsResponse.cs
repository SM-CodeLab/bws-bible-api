using System.Collections.Generic;
using Bws.Bible.Api.Models.Search.Items;

namespace Bws.Bible.Api.Models.Search
{
    /// <summary>
    /// Résultat d'une recherche
    /// </summary>
    public class SearchWordsResponse : BaseApiResponse
    {
        /// <summary>
        /// Nombre de résultats trouvés
        /// </summary>
        public short CountResult { get; set; }

        /// <summary>
        /// Verset(s) où le(s) résultat(s) de recherche a(ont) été trouvé(s)
        /// </summary>
        public List<VerseFoundItem> SearchResult { get; set; }
    }
}
