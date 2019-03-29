using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Auth;
using API.Errors;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Models.Users.Repositories;
using ModelsConverters.Users;
using Client = ClientModels.Users;
using Model = Models.Users;

namespace API.Controllers
{
    [Route("api/v1/user")]
    public sealed class UserController : Controller
    {
        private readonly IUserRepository repository;

        public UserController(IUserRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUserAsync([FromBody] Client.UserCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("UserCreationInfo");
                return BadRequest(error);
            }

            var hashPassword = PasswordEncoder.Encode(creationInfo.Password);
            var modelCreationInfo = new UserCreationInfo(creationInfo.Login, hashPassword);
            User modelUser = null;
            
            try
            {
                modelUser = await repository.CreateAsync(modelCreationInfo, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (UserDuplicationException)
            {
                var error = ServiceErrorResponses.UserDuplication(modelCreationInfo.Login);
                return BadRequest(error);
            }
            
            var clientUser = UserConverter.Convert(modelUser);

            return Ok(clientUser);
        }

        [HttpGet]
        [Route("id/{userId}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(userId, out var modelUserId))
            {
                var error = ServiceErrorResponses.BodyIsMissing(userId);
                return BadRequest(error);
            }

            User modelUser = null;

            try
            {
                modelUser = await repository.GetAsync(modelUserId, cancellationToken).ConfigureAwait(false);
            }
            catch (UserNotFoundException)
            {
                var error = ServiceErrorResponses.UserNotFound(userId);
                return NotFound(error);
            }

            var clientUser = UserConverter.Convert(modelUser);

            return Ok(clientUser);
        }

        [HttpGet]
        [Route("username/{userLogin}")]
        public async Task<IActionResult> GetUserByLoginAsync([FromRoute] string userLogin,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userLogin == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing(userLogin);
                return BadRequest(error);
            }

            User modelUser = null;

            try
            {
                modelUser = await repository.GetAsync(userLogin, cancellationToken).ConfigureAwait(false);
            }
            catch (UserNotFoundException)
            {
                var error = ServiceErrorResponses.UserNotFound(userLogin);
                return NotFound(error);
            }

            var clientUser = UserConverter.Convert(modelUser);
            return Ok(clientUser);
        }
    }
}