using System;

namespace Models.TodoItems
{
    public class TodoSearchInfo
    {
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество заметок, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }
        
        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public TodoPriority? Priority { get; set; }
        
        /// <summary>
        /// Состояние задачи
        /// </summary>
        public TodoState? State { get; set; }

        /// <summary>
        /// Минимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Максимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortOrder? SortOrder { get; set; }

        /// <summary>
        /// Аспект заметки, по которому нужно искать
        /// </summary>
        public TodoSortBy? SortBy { get; set; }
    }
}