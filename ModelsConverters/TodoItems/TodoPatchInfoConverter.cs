using System;
using Model = Models.TodoItems;
using Client = ClientModels.TodoItems;

namespace ModelsConverters.TodoItems
{
    public static class TodoPatchConverter
    {
        public static Model.TodoPatchInfo Convert(Guid todoId, Guid userId, Client.TodoPatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            Model.TodoPriority? priority = null;

            if (Enum.TryParse(clientPatchInfo.Priority, true, out Model.TodoPriority tmpPriority))
            {
                priority = tmpPriority;
            }
            
            Model.TodoState? state = null;
            
            if (Enum.TryParse(clientPatchInfo.State, true, out Model.TodoState tmpState))
            {
                state = tmpState;
            }
            
            var modelPatchInfo = new Model.TodoPatchInfo(todoId, userId)
            {
                Text = clientPatchInfo.Text,
                Title = clientPatchInfo.Title,
                Priority = priority,
                State = state
            };

            return modelPatchInfo;
        }
    }
}