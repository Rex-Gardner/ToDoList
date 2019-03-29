using System;
using Model = Models.TodoItems;
using Client = ClientModels.TodoItems;

namespace ModelsConverters.TodoItems
{
    public static class TodoItemConverter
    {
        public static Client.TodoItem Convert(Model.TodoItem modelTodoItem)
        {
            if (modelTodoItem == null)
            {
                throw new ArgumentNullException(nameof(modelTodoItem));
            }

            var clientTodoItem = new Client.TodoItem(modelTodoItem.Id.ToString(), modelTodoItem.UserId.ToString(), 
                modelTodoItem.Title, modelTodoItem.Text, modelTodoItem.Priority.ToString(), 
                modelTodoItem.State.ToString(), modelTodoItem.CreatedAt, modelTodoItem.LastUpdateAt);

            return clientTodoItem;
        }
    }
}