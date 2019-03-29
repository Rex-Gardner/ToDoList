using System;
using Model = Models.TodoItems;
using Client = ClientModels.TodoItems;

namespace ModelsConverters.TodoItems
{
    public static class TodoCreationInfoConverter
    {
        public static Model.TodoCreationInfo Convert(Guid userId, Client.TodoCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            Model.TodoPriority? priority = null;

            if (Enum.TryParse(clientCreationInfo.Priority, true, out Model.TodoPriority tmpPriority))
            {
                priority = tmpPriority;
            }

            var modelCreationInfo = new Model.TodoCreationInfo(userId, clientCreationInfo.Title, 
                clientCreationInfo.Text, priority);

            return modelCreationInfo;
        }
    }
}
