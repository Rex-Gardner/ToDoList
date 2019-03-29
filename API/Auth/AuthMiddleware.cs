using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.Errors;

namespace API.Auth
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        private IAuthenticator authenticator;

        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authInfo = context.Request.Headers.GetCommaSeparatedValues("Authorization");
            string userName = null;
            string password = null;
 
            if (authInfo.Length == 0)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ServiceErrorCodes.Unauthorized);
            }
            
            var loginInfo = authInfo[0].Split(" ");
 
            if (loginInfo.Length != 2)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ServiceErrorCodes.Unauthorized);
            }
            
            var base64String = loginInfo[1];
            var decodedByteData = Convert.FromBase64String(base64String);
            var decodedInfo = System.Text.Encoding.UTF8.GetString(decodedByteData).Split(":");
            userName = decodedInfo[0];
            password = decodedInfo[1];
            
            //TODO мне не нравится эта штука
            authenticator = (IAuthenticator)context.RequestServices.GetService(typeof(IAuthenticator));
            
            try
            {
                var user = await authenticator.AuthenticateAsync(userName, password, CancellationToken.None);
                context.Items.Add("userId", user.Id);
            }
            catch (AuthenticationException)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ServiceErrorCodes.InvalidCredentials);
            }

            await next.Invoke(context);
        }
    }
}