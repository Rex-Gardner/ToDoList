using System;

namespace API.Auth
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException()
            : base("Invalid credentials.")
        {
        }
    }
}