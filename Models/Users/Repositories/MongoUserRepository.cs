using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.Users.Repositories
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> users;

        public MongoUserRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TodoDb");
            users = database.GetCollection<User>("Users");
        }
        
        public Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            
            var userWithSameLogin = users.Find(item => item.Login == creationInfo.Login).FirstOrDefault();

            if (userWithSameLogin != null)
            {
                throw new UserDuplicationException(userWithSameLogin.Login);
            }
            
            var id = Guid.NewGuid();
            var now = DateTime.Now;
            var user = new User(id, creationInfo.Login, creationInfo.PasswodHash, now);

            users.InsertOne(user);

            return Task.FromResult(user);
        }

        public Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = users.Find(item => item.Id == userId).FirstOrDefault();
            
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return Task.FromResult(user);
        }

        public Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = users.Find(item => item.Login == login).FirstOrDefault();
            
            if (user == null)
            {
                throw new UserNotFoundException(login);
            }

            return Task.FromResult(user);
        }
    }
}