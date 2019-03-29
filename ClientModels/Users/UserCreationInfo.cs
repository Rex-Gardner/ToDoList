using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ClientModels.Users
{
    /// <summary>
    /// Информация для регистрации пользователя
    /// </summary>
    [DataContract]
    public class UserCreationInfo
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(24, MinimumLength=3)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(24, MinimumLength=3)]
        public string Password { get; set; }
    }
}