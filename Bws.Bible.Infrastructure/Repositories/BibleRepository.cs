using System.Text;
using Microsoft.Extensions.Logging;
using Bws.Bible.Core.Configuration;
using Bws.Bible.Core.Domain;
using Bws.Bible.Core.Domain.Enums;
using Bws.Bible.Core.Exceptions;
using Bws.Bible.Core.Exceptions.Enums;
using Bws.Bible.Core.Repositories;
using Bws.Bible.Infrastructure.Data;
using Bws.Bible.Infrastructure.Mapper;

namespace Bws.Bible.Infrastructure.Repositories
{
    public class BibleRepository : IBibleRepository
    {
        private readonly IInfrastructureSettings _infrastructureSettings;
        private readonly Encoding _defaultEncoding;
        private readonly ILogger _logger;
        private List<BibleData> _inMemoryBibles;
        private List<BibleData> _inMemoryBiblesPreface;

        public BibleRepository(IInfrastructureSettings infrastructureSettings, ILogger<BibleRepository> logger)
        {
            _infrastructureSettings = infrastructureSettings;
            _defaultEncoding = Encoding.UTF8;
            _logger = logger;
            _inMemoryBibles = new List<BibleData>();
            _inMemoryBiblesPreface = new List<BibleData>();

            if(_infrastructureSettings.LoadBiblesAtStartup)
            {
                string[] filePaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _infrastructureSettings.StoragePath), $"*.{_infrastructureSettings.BibleFileExtension}", SearchOption.TopDirectoryOnly);
                foreach (string filePath in filePaths)
                {
                    _inMemoryBiblesPreface.Add(LoadBibleHeader(filePath));
                    _inMemoryBibles.Add(LoadBibleFull(filePath));
                }
            }
        }

        /// <summary>
        /// Informations sur les bibles (sans versets ni statistiques)
        /// </summary>
        /// <param name="language">(facultatif) Filtre par langue</param>
        /// <param name="forceReload">(facultatif) Recharger les informations d'entête des Bibles</param>
        /// <returns>BibleDto (Data Transfert Object)</returns>
        public List<BibleDto> GetBibles(ELanguage? language, bool forceReload)
        {
            if (!_inMemoryBiblesPreface.Any() || forceReload)
            {
                string[] filePaths = Directory.GetFiles(_infrastructureSettings.StoragePath, $"*.{_infrastructureSettings.BibleFileExtension}", SearchOption.TopDirectoryOnly);

                foreach (string filePath in filePaths)
                {
                    _inMemoryBiblesPreface.Add(LoadBibleHeader(filePath));
                }
            }

            if (language != null)
            {
                return _inMemoryBiblesPreface.ToBibleDto().Where(b => b.Language == language.Value).ToList();
            }

            return _inMemoryBiblesPreface.ToBibleDto().ToList();
        }

        /// <summary>
        /// Récupérer une Bible
        /// </summary>
        /// <param name="idBible">Identifiant de la Bible</param>
        /// <param name="withVerses">Récupérer les versets</param>
        /// <returns>BibleDto (Data Transfert Object)</returns>
        public BibleDto GetBible(string idBible, bool withVerses)
        {
            if (!CheckBible(idBible, true))
            {
                return null;
            }
            return _inMemoryBibles.FirstOrDefault(b => b.Id == idBible).ToBibleDto(withVerses);
        }

        /// <summary>
        /// Récupérer un livre de la Bible
        /// </summary>
        /// <param name="idBible">Identifiant de la Bible</param>
        /// <param name="idBook">Numéro du livre</param>
        /// <param name="withVerses">Récupérer les versets</param>
        /// <returns>BookDto (Data Transfert Object)</returns>
        public BookDto GetBook(string idBible, byte idBook, bool withVerses)
        {
            if (!CheckBible(idBible, true))
            {
                return null;
            }
            if (_inMemoryBibles.First(b => b.Id == idBible).Books.Any(b => b.IdBook == idBook))
            {
                return _inMemoryBibles.First(b => b.Id == idBible).Books.FirstOrDefault(b => b.IdBook == idBook).ToBookDto(_inMemoryBibles.First(b => b.Id == idBible), withVerses);
            }
            return null;
        }

        /// <summary>
        /// Récupérer des versets de la Bible
        /// </summary>
        /// <param name="idBible">Identifiant de la Bible</param>
        /// <param name="idBook">Numéro du livre</param>
        /// <param name="idChapter">Numéro du chapitre</param>
        /// <param name="idFirstVerse">Numéro du premier verset</param>
        /// <param name="idLastVerse">Numéro du dernier verset</param>
        /// <returns>Liste de VerseDto (Data Transfert Object)</returns>
        public List<VerseDto> GetVerses(string idBible, byte idBook, byte idChapter, byte idFirstVerse, byte idLastVerse)
        {
            if (!CheckBible(idBible, true) || idFirstVerse > idLastVerse)
            {
                return null;
            }
            if (_inMemoryBibles.First(b => b.Id == idBible).Verses.Any(v => v.IdBook == idBook && v.IdChapter == idChapter && v.IdVerse == idFirstVerse))
            {
                return _inMemoryBibles
                    .First(b => b.Id == idBible)
                    .Verses.Where(v =>
                        v.IdBook == idBook &&
                        v.IdChapter == idChapter &&
                        v.IdVerse >= idFirstVerse &&
                        v.IdVerse <= idLastVerse
                    ).ToVerseDto().ToList();
            }
            return null;
        }

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
        public List<VerseDto> GetVersesBySearchingWords(string words, string idBible, byte? idBook = null, byte? idChapter = null, byte? idFirstVerse = null, byte? idLastVerse = null)
        {
            if (string.IsNullOrWhiteSpace(words)
                || !CheckBible(idBible, true)
                || (idFirstVerse != null && idLastVerse != null && idFirstVerse.Value > idLastVerse.Value))
            {
                return null;
            }

            IEnumerable<VerseData> query = _inMemoryBibles.First(b => b.Id == idBible).Verses;
            if (idBook != null)
            {
                query = query.Where(v => v.IdBook == idBook.Value);
                if (idChapter != null)
                {
                    query = query.Where(v => v.IdChapter == idChapter.Value);
                    if (idFirstVerse != null && idLastVerse == null)
                    {
                        query = query.Where(v => v.IdVerse == idFirstVerse.Value);
                    }
                    if (idFirstVerse != null && idLastVerse != null)
                    {
                        query = query.Where(v => v.IdVerse >= idFirstVerse.Value && v.IdVerse <= idLastVerse.Value);
                    }
                }
            }
            query = query.Where(v => v.Text.Contains(words, System.StringComparison.InvariantCultureIgnoreCase));

            return query.ToVerseDto().ToList();
        }

        #region Private Methods

        //Vérifier si une Bible est présente via son identifiant (avec chargement en mémoire si autoload == true)
        private bool CheckBible(string idBible, bool autoload)
        {
            if (_inMemoryBibles.Any(b => b.Id == idBible))
            {
                return true;
            }
            if (autoload)
            {
                var biblePath = $"{_infrastructureSettings.StoragePath}{Path.DirectorySeparatorChar}{idBible}.{_infrastructureSettings.BibleFileExtension}";
                _inMemoryBibles.Add(LoadBibleFull(biblePath));
                if (_inMemoryBibles.Any(b => b.Id == idBible))
                {
                    return true;
                }
            }
            return false;
        }

        //Chargement des informations d'entête d'une Bible (lecture de la 1ère ligne du fichier)
        private BibleData LoadBibleHeader(string bibleFilePath)
        {
            _logger.LogInformation($"Load bible header from {bibleFilePath}");

            if (!File.Exists(bibleFilePath))
            {
                throw new InfrastructureException() { ErrorCode = EInfrastructureErrorCode.LoadBibleHeader };
            }

            var bible = new BibleData();
            bible.Id = Path.GetFileNameWithoutExtension(bibleFilePath);

            using (StreamReader reader = new StreamReader(bibleFilePath, _defaultEncoding))
            {
                ReadAndLoadBibleHeader(reader, bible);
                return bible;
            }
        }

        //Chargement complet d'une Bible (lecture du fichier complet)
        private BibleData LoadBibleFull(string bibleFilePath)
        {
            _logger.LogInformation($"Load bible full from {bibleFilePath}");

            if (!File.Exists(bibleFilePath))
            {
                throw new InfrastructureException()
                {
                    ErrorCode = EInfrastructureErrorCode.LoadBibleFull,
                    ErrorMessage = "Bible introuvable"
                };
            }

            var bible = new BibleData();
            bible.Id = Path.GetFileNameWithoutExtension(bibleFilePath);
            _logger.LogInformation($"Chargement de la Bible {bible.Id}");

            using (StreamReader reader = new StreamReader(bibleFilePath, _defaultEncoding))
            {
                ReadAndLoadBibleHeader(reader, bible);
                ReadAndLoadBibleBooksList(reader, bible);
                ReadAndLoadBibleVerses(reader, bible);
                ComputeStatistics(bible);
                _logger.LogInformation($"Chargement de la Bible {bible.Id} terminé");
                return bible;
            }
        }

        //Lit et charge en mémoire les informations d'en-tête de la Bible (index 0)
        private void ReadAndLoadBibleHeader(StreamReader reader, BibleData bible)
        {
            GoToLine(reader, 0);

            string line = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] data = line.Split(_infrastructureSettings.DelimiterSeparatedValues);
                bible.Name = data[1];
                bible.Language = data[2];
                bible.Translator = data[3];
                bible.ReleaseYear = short.Parse(data[4]);
            }
        }

        //Lit et charge en mémoire les informations sur les livres de la Bible (index 1 à 66)
        private void ReadAndLoadBibleBooksList(StreamReader reader, BibleData bible)
        {
            byte index = 1;
            GoToLine(reader, index);

            string line;
            while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()) && index <= 66)
            {
                string[] data = line.Split(_infrastructureSettings.DelimiterSeparatedValues);
                var book = new BookData()
                {
                    IdBook = byte.Parse(data[0]),
                    Title = data[1],
                };
                bible.Books.Add(book);
                index++;
            }
        }

        //Lit et charge en mémoire les versets de la Bible (à partir de l'index 67)
        private void ReadAndLoadBibleVerses(StreamReader reader, BibleData bible)
        {
            byte index = 67;
            GoToLine(reader, index);

            string line;
            while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
            {
                string[] data = line.Split(_infrastructureSettings.DelimiterSeparatedValues);
                var verse = new VerseData()
                {
                    IdBook = byte.Parse(data[0]),
                    IdChapter = byte.Parse(data[1]),
                    IdVerse = byte.Parse(data[2]),
                    Text = data[3]
                };
                bible.Verses.Add(verse);
            }
        }

        //Déplace le lecteur du flux à la ligne n°{lineNumber}
        private void GoToLine(StreamReader reader, int lineNumber)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < lineNumber; i++)
            {
                reader.ReadLine();
            }
        }

        // Calculer toutes les données statistiques de la Bible (stockage en mémoire, le repository étant persistant)
        private void ComputeStatistics(BibleData bible)
        {
            ComputeBibleStatistics(bible);
            foreach (var book in bible.Books)
            {
                ComputeBibleBookStatistics(bible, book);
            }
        }

        // Calculer le nombre de livres et de versets présents dans la Bible
        private void ComputeBibleStatistics(BibleData bible)
        {
            bible.Statistics.CountBooks = (byte)bible.Verses.Select(v => v.IdBook).Distinct().Count();
            bible.Statistics.CountVerses = (short)bible.Verses.Count();
        }

        // Calculer le nombre de chapitres et de versets présents dans un livre de la Bible
        private void ComputeBibleBookStatistics(BibleData bible, BookData book)
        {
            book.Statistics.CountChapters = (byte)bible.Verses.Where(v => v.IdBook == book.IdBook).Select(v => v.IdChapter).Distinct().Count();
            book.Statistics.CountVerses = (short)bible.Verses.Where(v => v.IdBook == book.IdBook).Count();
        }

        #endregion Private Methods
    }
}
