using System;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    public class DAL_NotFound_Exception : Exception
    {
        public DAL_NotFound_Exception (string message)
            : base(message) {}

        public DAL_NotFound_Exception (string message, Exception inner)
            : base(message, inner) {}
    }
}