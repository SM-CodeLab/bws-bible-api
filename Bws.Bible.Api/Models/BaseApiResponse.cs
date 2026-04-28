using Microsoft.AspNetCore.Mvc;
using System;

namespace Bws.Bible.Api.Models
{
    /// <summary>
    /// Classe mère des modèles de réponse de l'API Bible
    /// </summary>
    public abstract class BaseApiResponse
    {
        private DateTime Start;
        private DateTime? End;

        /// <summary>
        /// Le temps écoulé entre l'initialisation de la réponse et sa transformation en objet Json
        /// Ce champ n'a une valeur que si la mesure du temps a été activée par la méthode "ToJsonResult"
        /// </summary>
        public string ResponseTime
        {
            get { return End != null ? string.Format("{0} ms", (End.Value - Start).TotalMilliseconds) : null; }
            private set { }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public BaseApiResponse()
        {
            Start = DateTime.Now;
        }

        /// <summary>
        /// Transforme la réponse en résultat Json pour l'API
        /// </summary>
        /// <param name="withResponseTime">Activer la mesure du temps écoulé entre l'initialisation de la réponse et sa transformation en objet Json</param>
        /// <returns></returns>
        public JsonResult ToJsonResult(bool withResponseTime)
        {
            if (withResponseTime)
            {
                End = DateTime.Now;
            }
            return new JsonResult(this);
        }
    }
}
