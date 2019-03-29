using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.TodoItems.Repositories
{
    public class MongoTodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<TodoItem> todoItems;

        public MongoTodoRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TodoDb");
            todoItems = database.GetCollection<TodoItem>("TodoItems");
        }

        public Task<TodoItem> CreateAsync(TodoCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var id = Guid.NewGuid();
            var now = DateTime.Now;
            var todoItem = new TodoItem(id, creationInfo.UserId, creationInfo.Title, creationInfo.Text, 
                creationInfo.Priority, now);

            todoItems.InsertOne(todoItem);

            return Task.FromResult(todoItem);
        }
        
        public Task<TodoItem> GetAsync(Guid todoId, Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var todoItem = todoItems.Find(item => item.Id == todoId && item.UserId == userId).FirstOrDefault();
            
            if (todoItem == null)
            {
                throw new TodoNotFoundException($"TodoItem {todoId} not found");
            }

            return Task.FromResult(todoItem);
        }
        
        public Task<IReadOnlyList<TodoItem>> SearchAsync(TodoSearchInfo searchInfo, CancellationToken cancellationToken)
        {
            if (searchInfo == null)
            {
                throw new ArgumentNullException(nameof(searchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var search = todoItems.Find(item => item.UserId == searchInfo.UserId).ToEnumerable();

            if (searchInfo.CreatedFrom != null)
            {
                search = search.Where(item => item.CreatedAt >= searchInfo.CreatedFrom.Value);
            }

            if (searchInfo.CreatedTo != null)
            {
                search = search.Where(item => item.CreatedAt <= searchInfo.CreatedTo.Value);
            }

            if (searchInfo.Priority != null)
            {
                search = search.Where(item => item.Priority == (TodoPriority)searchInfo.Priority);
            }

            if (searchInfo.State != null)
            {
                search = search.Where(item => item.State == (TodoState)searchInfo.State);
            }

            if (searchInfo.Offset != null)
            {
                search = search.Skip(searchInfo.Offset.Value);
            }

            if (searchInfo.Limit != null)
            {
                search = search.Take(searchInfo.Limit.Value);
            }

            if (searchInfo.SortOrder != null || searchInfo.SortBy != null)
            {
                var sortOrder = searchInfo.SortOrder ?? SortOrder.Ascending;
                var sortBy = searchInfo.SortBy ?? TodoSortBy.Creation;
                search = Sort(search, sortOrder, sortBy);
            }

            var todoList = search.ToList();
            return Task.FromResult<IReadOnlyList<TodoItem>>(todoList);
        }

        private IEnumerable<TodoItem> Sort(IEnumerable<TodoItem> enumerable, SortOrder sortOrder, TodoSortBy sortBy)
        {
            // TODO Избавиться от повторений
            switch (sortBy)
            {
                case TodoSortBy.Priority:
                    return sortOrder == SortOrder.Ascending ?
                        enumerable.OrderBy(item => item.Priority) :
                        enumerable.OrderByDescending(item => item.Priority);
                case TodoSortBy.State:
                    return sortOrder == SortOrder.Ascending ?
                        enumerable.OrderBy(item => item.State) :
                        enumerable.OrderByDescending(item => item.State);
                case TodoSortBy.Creation:
                    return sortOrder == SortOrder.Ascending ?
                        enumerable.OrderBy(item => item.CreatedAt) :
                        enumerable.OrderByDescending(item => item.CreatedAt);
                case TodoSortBy.LastUpdate:
                    return sortOrder == SortOrder.Ascending ?
                        enumerable.OrderBy(item => item.LastUpdateAt) :
                        enumerable.OrderByDescending(item => item.LastUpdateAt);
                default:
                    throw new ArgumentException($"Unknown todoItem type of sort \"{sortBy}\".", nameof(sortBy));
            }
        }
        
        public Task<TodoItem> PatchAsync(TodoPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var todoItem = todoItems.Find(item => item.Id == patchInfo.Id && item.UserId == patchInfo.UserId)
                .FirstOrDefault();
            
            if (todoItem == null)
            {
                throw new TodoNotFoundException($"TodoItem {patchInfo.Id} not found");
            }

            var updated = false;

            if (patchInfo.Title != null)
            {
                todoItem.Title = patchInfo.Title;
                updated = true;
            }

            if (patchInfo.Text != null)
            {
                todoItem.Text = patchInfo.Text;
                updated = true;
            }
            
            if (patchInfo.Priority != null)
            {
                todoItem.Priority = (TodoPriority)patchInfo.Priority;
                updated = true;
            }
            
            if (patchInfo.State != null)
        {
                todoItem.State = (TodoState)patchInfo.State;
                updated = true;
            }

            if (updated)
            {
                todoItem.LastUpdateAt = DateTime.UtcNow;
                todoItems.ReplaceOne(item => item.Id == patchInfo.Id, todoItem);
            }

            return Task.FromResult(todoItem);
        }
        
        public Task RemoveAsync(Guid todoId, Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var deleteResult = todoItems.DeleteOne(item => item.Id == todoId && item.UserId == userId);
            
            if (deleteResult.DeletedCount == 0)
            {
                throw new TodoNotFoundException($"TodoItem {todoId} not found");
            }

            return Task.CompletedTask;
        }
    }
}