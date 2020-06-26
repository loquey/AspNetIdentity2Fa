using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.Models
{
    public class GoogleAuthenticatorViewModel
    {

        public string QRCodeData { get; set; }
        public string Code { get; set; }
    }
}
