using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Exceptions
{
    public class PermissionRequiredException : Exception
    {
        public PermissionRequiredException() : base()
        {
        }

        public PermissionRequiredException(string message) : base(message)
        {
        }

        public PermissionRequiredException(string message, Exception inner) : base(message, inner)
        { }
    }
}
