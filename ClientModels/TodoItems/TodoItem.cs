using System;

namespace ClientModels.TodoItems
{
    public class TodoItem
    {
        public TodoItem(string id, string userId, string title, string text, string priority, 
            string state, DateTime createdAt, DateTime lastUpdateAt)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Priority = priority ?? throw new ArgumentNullException(nameof(priority));
            State = state ?? throw new ArgumentNullException(nameof(state));
            CreatedAt = createdAt;
            LastUpdateAt = lastUpdateAt;
        }

        public string Id { get; }

        public string UserId { get; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Priority { get; set; }

        public string State { get; set; }

        public DateTime CreatedAt { get; }

        public DateTime LastUpdateAt { get; set; }
    }
}
