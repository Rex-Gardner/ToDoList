using System;

namespace Models.TodoItems
{
    public class TodoNotFoundException : Exception
    {
        public TodoNotFoundException(string message)
            :base(message)
        {
        }
    }
}