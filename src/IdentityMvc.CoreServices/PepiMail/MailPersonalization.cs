using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.PepiMail
{
    public class MailPersonalization
    {
        public string Recipient { get; set; }
        public object Attributes { get; set; }
        public IEnumerable<string> Recipient_cc { get; set; }
    }
}
