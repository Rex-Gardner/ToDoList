namespace Models.Errors
{
    /// <summary>
    /// Коды ошибок
    /// </summary>
    public static class ServiceErrorCodes
    {
        /// <summary>
        /// Не найдено
        /// </summary>
        public const string NotFound = "not-found";

        /// <summary>
        /// Нет доступа
        /// </summary>
        public const string NotAcceptable = "auth:not-acceptable";

        /// <summary>
        /// Не авторизован
        /// </summary>
        public const string Unauthorized = "auth:unauthorized";

        /// <summary>
        /// Некорректные учетные данные
        /// </summary>
        public const string InvalidCredentials = "auth:invalid-credentials";

        /// <summary>
        /// Некорректный запрос
        /// </summary>
        public const string BadRequest = "bad-request";
    }
}