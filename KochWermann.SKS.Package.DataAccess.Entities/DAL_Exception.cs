using System;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    public class DAL_Exception : Exception
    {
        public DAL_Exception (string message)
            : base(message) {}

        public DAL_Exception (string message, Exception inner)
            : base(message, inner) {}
    }
}