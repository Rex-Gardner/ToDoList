using System;

namespace Models.Users
{
    /// <summary>
    /// Исключение, которое возникает при попытке создать существующего пользователя
    /// </summary>
    public class UserDuplicationException : Exception
    {
        /// <summary>
        /// Инициализировать эксземпляр исключения по логину пользователя
        /// </summary>
        /// <param name="login"></param>
        public UserDuplicationException(string login)
            : base($"User with login \"{login}\" is already exist.")
        {
        }
    }
}