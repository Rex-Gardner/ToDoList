using System;

namespace Models.TodoItems
{
    public class TodoCreationInfo
    {
        public Guid UserId { get; }
        public string Title { get; }
        public string Text { get; }
        public TodoPriority Priority { get; }

        public TodoCreationInfo(Guid userId, string title, string text, TodoPriority? priority)
        {
            UserId = userId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Priority = priority ?? TodoPriority.Major;
        }
    }
}