using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMailingNetService
    {
        Task SendEmailNetAsync(string mailTo, string subject, string body);
    

    }
}
