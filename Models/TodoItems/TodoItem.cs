using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models.TodoItems
{
    public class TodoItem
    {
        private const string Unnamed = "Без названия";

        public TodoItem(Guid id, Guid userId, string title, string text, TodoPriority priority, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Title = title ?? Unnamed;
            Text = text ?? string.Empty;
            Priority = priority;
            State = TodoState.NotStarted;
            CreatedAt = createdAt;
            LastUpdateAt = createdAt;
        }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; private set; }

        [BsonElement("Owner")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; private set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Text")]
        public string Text { get; set; }

        [BsonElement("Priority")]
        [BsonRepresentation(BsonType.String)]
        public TodoPriority Priority { get; set; }

        [BsonElement("State")]
        [BsonRepresentation(BsonType.String)]
        public TodoState State { get; set; }

        [BsonElement("CreatedAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; private set; }

        [BsonElement("LastUpdateAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdateAt { get; set; }
    }
}
