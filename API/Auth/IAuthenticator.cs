using System.Threading;
using System.Threading.Tasks;
using Models.Users;

namespace API.Auth
{
    public interface IAuthenticator
    {
        Task<User> AuthenticateAsync(string login, string password, CancellationToken cancellationToken);
    }
}