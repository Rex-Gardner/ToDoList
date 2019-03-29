using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Users
{
    public class User
    {
        public User(Guid id, string login, string passwordHash, DateTime registeredAt)
        {
            Id = id;
            Login = login ?? throw new ArgumentNullException(nameof(login));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            RegisteredAt = registeredAt;
        }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; private set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [BsonElement("Login")]
        public string Login { get; private set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [BsonElement("RegisteredAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime RegisteredAt { get; private set; }
    }
}