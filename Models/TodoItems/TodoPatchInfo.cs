using System;

namespace Models.TodoItems
{
    public class TodoPatchInfo
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Title { get; set; }
        public string Text { get; set; }
        public TodoPriority? Priority { get; set; }
        public TodoState? State { get; set; }
        
        public TodoPatchInfo(Guid id, Guid userId, string title = null, string text = null, TodoPriority? priority = null, 
            TodoState? state = null)
        {
            Id = id;
            UserId = userId;
            Title = title;
            Text = text;
            Priority = priority;
            State = state;
        }
    }
}