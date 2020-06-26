using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.PepiMail
{
    public class EmailContainer
    {
        public IEnumerable<MailPersonalization> Personalizations { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public long? TemplateId { get; set; }
        public string ReplyToId { get; set; }
        public MailFrom From { get; set; }
        public IEnumerable<MailAttachement> Attachements { get; set; }
    }
}
