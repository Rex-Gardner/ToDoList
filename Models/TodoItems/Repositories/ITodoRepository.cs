using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.TodoItems.Repositories
{
    /// <summary>
    /// Интерфейс хранилища заметок
    /// </summary>
    public interface ITodoRepository
    {
        /// <summary>
        /// Создать новый заметок
        /// </summary>
        /// <param name="creationInfo">Информация для создания заметки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание заметки. Результат выполнения операции - информация о созданной заметке</returns>
        Task<TodoItem> CreateAsync(TodoCreationInfo creationInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Найти заметки
        /// </summary>
        /// <param name="searchInfo">Поисковый запрос</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск заметок. Результат выполнения - список найденных заметок</returns>
        Task<IReadOnlyList<TodoItem>> SearchAsync(TodoSearchInfo searchInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Запросить заметку
        /// </summary>
        /// <param name="todoId">Идентификатор заметки</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос заметки. Результат выполнения - заметка</returns>
        Task<TodoItem> GetAsync(Guid todoId, Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить заметку
        /// </summary>
        /// <param name="patchInfo">Описание изменений заметки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение заметки. Результат выполнения - актуальное состояние заметки</returns>
        Task<TodoItem> PatchAsync(TodoPatchInfo patchInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Удалить заметку
        /// </summary>
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="userId">Идентификатор пользователя (владельца заметки)</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление заметки</returns>
        Task RemoveAsync(Guid noteId, Guid userId, CancellationToken cancellationToken);
    }
}
