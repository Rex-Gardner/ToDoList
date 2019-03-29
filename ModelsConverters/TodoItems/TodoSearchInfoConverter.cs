using System;
using ModelTodo = Models.TodoItems;
using Client = ClientModels.TodoItems;

namespace ModelsConverters.TodoItems
{
    public static class TodoSearchInfoConverter
    {
        /// <summary>
        /// Переводит запрос за заметками из клиентсокой модели в серверную
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="clientSearchInfo">Запрос за заметками в клиентской модели</param>
        /// <returns>Запрос за заметками в серверной модели</returns>
        public static ModelTodo.TodoSearchInfo Convert(Guid userId, Client.TodoSearchInfo clientSearchInfo)
        {
            if (clientSearchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientSearchInfo));
            }

            ModelTodo.TodoPriority? priority = null;

            if (Enum.TryParse(clientSearchInfo.Priority, true, out ModelTodo.TodoPriority tmpPriority))
            {
                priority = tmpPriority;
            }
            
            ModelTodo.TodoState? state = null;
            
            if (Enum.TryParse(clientSearchInfo.State, true, out ModelTodo.TodoState tmpState))
            {
                state = tmpState;
            }
            
            ModelTodo.TodoSortBy? sortBy = null;

            if (Enum.TryParse(clientSearchInfo.SortBy, true, out ModelTodo.TodoSortBy tmpSortBy))
            {
                sortBy = tmpSortBy;
            }
            
            Models.SortOrder? sortOrder = null;
            
            if (Enum.TryParse(clientSearchInfo.SortOrder, true, out Models.SortOrder tmpSortOrder))
            {
                sortOrder = tmpSortOrder;
            }
            
            var modelQuery = new ModelTodo.TodoSearchInfo
            {
                UserId = userId,
                Offset = clientSearchInfo.Offset,
                Limit = clientSearchInfo.Limit,
                Priority = priority,
                State = state,
                CreatedFrom = clientSearchInfo.CreatedFrom,
                CreatedTo = clientSearchInfo.CreatedTo,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            return modelQuery;
        }
    }
}