using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMailingNetService
    {
        Task SendEmailNetAsync(string mailTo, string subject, string body);
    

    }
}
