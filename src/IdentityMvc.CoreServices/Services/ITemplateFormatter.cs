using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.Services
{
    public interface ITemplateFormatter
    {
        Task<string> FormatAsync(string template, object data);
    }
}