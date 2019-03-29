using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;
using Models.TodoItems.Repositories;
using Model = Models.TodoItems;
using Client = ClientModels.TodoItems;
using Converter = ModelsConverters.TodoItems;

namespace API.Controllers
{
    [Route("api/v1/todo")]
    public sealed class TodoController : ControllerBase
    {
        private readonly ITodoRepository repository;
        
        public TodoController(ITodoRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTodoItemAsync([FromBody]Client.TodoCreationInfo creationInfo, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("TodoCreationInfo");
                return BadRequest(error);
            }

            if (!HttpContext.Items.TryGetValue("userId", out var userId) || 
                !Guid.TryParse(userId.ToString(), out var userGuid))
            {
                var error = ServiceErrorResponses.UnAuthorized();
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.TodoCreationInfoConverter.Convert(userGuid, creationInfo);
            var modelTodoItem = await repository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);

            var clientTodoItem = Converter.TodoItemConverter.Convert(modelTodoItem);
            var routeParams = new Dictionary<string, object>
            {
                { "todoId", clientTodoItem.Id }
            };

            return CreatedAtRoute("GetTodoItem", routeParams, clientTodoItem);
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchTodoItemsAsync([FromQuery]Client.TodoSearchInfo searchInfo, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!HttpContext.Items.TryGetValue("userId", out var userId) || 
                !Guid.TryParse(userId.ToString(), out var userGuid))
            {
                var error = ServiceErrorResponses.UnAuthorized();
                return BadRequest(error);
            }
            
            var modelSearchInfo = Converter.TodoSearchInfoConverter.Convert(userGuid, searchInfo ?? new Client.TodoSearchInfo());
            var modelTodoList = await repository.SearchAsync(modelSearchInfo, cancellationToken).ConfigureAwait(false);
            var clientTodoList = modelTodoList.Select(Converter.TodoItemConverter.Convert).ToImmutableList();

            return Ok(clientTodoList);
        }
        
        [HttpGet]
        [Route("{todoId}", Name = "GetTodoItem")]
        public async Task<IActionResult> GetTodoItemAsync([FromRoute]string todoId, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(todoId, out var modelTodoItemId))
            {
                var error = ServiceErrorResponses.TodoNotFound(todoId);
                return NotFound(error);
            }
            
            if (!HttpContext.Items.TryGetValue("userId", out var userId) || 
                !Guid.TryParse(userId.ToString(), out var userGuid))
            {
                var error = ServiceErrorResponses.UnAuthorized();
                return BadRequest(error);
            }

            Model.TodoItem modelTodoItem = null;

            try
            {
                modelTodoItem = await repository.GetAsync(modelTodoItemId, userGuid, cancellationToken).ConfigureAwait(false);
            }
            catch (Model.TodoNotFoundException)
            {
                var error = ServiceErrorResponses.TodoNotFound(todoId);
                return NotFound(error);
            }

            var clientTodoItem = ModelsConverters.TodoItems.TodoItemConverter.Convert(modelTodoItem);

            return Ok(clientTodoItem);
        }
        
        [HttpPatch]
        [Route("{todoId}")]
        public async Task<IActionResult> PatchTodoItemAsync([FromRoute]string todoId, 
            [FromBody]ClientModels.TodoItems.TodoPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
               var error = ServiceErrorResponses.BodyIsMissing("TodoPatchInfo");
               return BadRequest(error);
            }

            if (!Guid.TryParse(todoId, out var todoItemGuid))
            {
                var error = ServiceErrorResponses.TodoNotFound(todoId);
                return NotFound(error);
            }
            
            if (!HttpContext.Items.TryGetValue("userId", out var userId) || 
                !Guid.TryParse(userId.ToString(), out var userGuid))
            {
                var error = ServiceErrorResponses.UnAuthorized();
                return BadRequest(error);
            }

            var modelPathInfo = ModelsConverters.TodoItems.TodoPatchConverter.Convert(todoItemGuid, userGuid, patchInfo);
            Models.TodoItems.TodoItem modelTodoItem = null;

            try
            {
                modelTodoItem = await repository.PatchAsync(modelPathInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (Model.TodoNotFoundException)
            {
                var error = ServiceErrorResponses.TodoNotFound(todoId);
                return NotFound(error);
            }

            var clientTodoItem = ModelsConverters.TodoItems.TodoItemConverter.Convert(modelTodoItem);
            return Ok(clientTodoItem);
        }
        
        [HttpDelete]
        [Route("{todoId}")]
        public async Task<IActionResult> DeleteTodoItemAsync([FromRoute]string todoId, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(todoId, out var todoGuid))
            {
                var error = ServiceErrorResponses.TodoNotFound(todoId);
                return NotFound(error);
            }
            
            if (!HttpContext.Items.TryGetValue("userId", out var userId) || 
                !Guid.TryParse(userId.ToString(), out var userGuid))
            {
                var error = ServiceErrorResponses.UnAuthorized();
                return BadRequest(error);
            }

            try
            {
                await repository.RemoveAsync(todoGuid, userGuid, cancellationToken).ConfigureAwait(false);
            }
            catch (Model.TodoNotFoundException)
            {
                var error = ServiceErrorResponses.TodoNotFound(todoId);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}