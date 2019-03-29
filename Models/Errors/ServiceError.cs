namespace Models.Errors
{
    /// <summary>
    /// Сервисная ошибка
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// Идентификатор ошибки
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Системное сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Ресурс или тип, который инициировал ошибку
        /// </summary>        
        public string Target { get; set; }
    }
}