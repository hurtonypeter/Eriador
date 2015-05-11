using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Framework.Models
{
    public class MobileLoginRequestModel
    {
        public string username { get; set; }

        public string password { get; set; }

        public bool rememberme { get; set; }
    }

    public class MobileLoginResponseModel : BaseJsonModel
    {
        public bool loginSuccess { get; set; }
    }
}
