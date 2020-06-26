using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stubble.Core.Builders;

namespace IdentityMvc.CoreServices.Services
{
    public class TemplateFormatter : ITemplateFormatter
    {
        public async Task<string> FormatAsync(string template, object data)
        {
            var stubble = new StubbleBuilder().Configure(s =>
            {
                s.SetIgnoreCaseOnKeyLookup(true);
            }).Build();

            return await stubble.RenderAsync(template, data);
        }
    }
}
