using System;
using System.Threading;
using System.Threading.Tasks;
using Models.Users;
using Models.Users.Repositories;

namespace API.Auth
{
    public class Authenticator : IAuthenticator
    {
        private readonly IUserRepository userRepository;

        public Authenticator(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> AuthenticateAsync(string login, string password, CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            cancellationToken.ThrowIfCancellationRequested();

            User user = null;

            try
            {
                user = await userRepository.GetAsync(login, cancellationToken).ConfigureAwait(false);
            }
            catch (UserNotFoundException)
            {
                throw new AuthenticationException();
            }

            var passwordHash = PasswordEncoder.Encode(password);

            if (!user.PasswordHash.Equals(passwordHash))
            {
                throw new AuthenticationException();
            }

            return user;
        }
    }
}