using System;
using System.Net;
using Models.Errors;

namespace API.Errors
{
    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse TodoNotFound(string todoId)
        {
            if (todoId == null)
            {
                throw new ArgumentNullException(nameof(todoId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"TodoItem \"{todoId}\" not found.",
                    Target = "todoItem"
                }
            };

            return error;
        }

        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse UserDuplication(string userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotAcceptable,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotAcceptable,
                    Message = $"User \"{userInfo}\" already exists.",
                    Target = "user"
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse UserNotFound(string userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"User \"{userInfo}\" not found.",
                    Target = "user"
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse UnAuthorized()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.Forbidden,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Unauthorized,
                    Message = "User is not authorized.",
                    Target = "todoItem"
                }
            };

            return error;
        }
    }
}