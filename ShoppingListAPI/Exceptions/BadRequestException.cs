using System;

namespace ShoppingListAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
            
        }
    }
}
