using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Framework.Models
{
    public class BaseJsonModel
    {
        /// <summary>
        /// Ha vmi hiba történt igaz, ha a kérés sikeres volt(modell feltöltve), akkor hamis
        /// </summary>
        public bool error
        {
            get
            {
                return !string.IsNullOrWhiteSpace(errorMessage);
            }
        }

        /// <summary>
        /// A hiba részletes szövege
        /// </summary>
        public string errorMessage;

        /// <summary>
        /// Ha a kérés sikeres volt, és új url-re kell ugrani JS-ből, akkor ide kell
        /// </summary>
        public string redirectToUrl;

        public BaseJsonModel()
        { }

        public BaseJsonModel(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }
    }
}
