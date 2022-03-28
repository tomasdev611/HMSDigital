using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Exceptions
{
    class ForbiddenException : Exception
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
