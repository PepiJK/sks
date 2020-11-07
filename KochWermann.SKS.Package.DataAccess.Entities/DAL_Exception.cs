using System;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class DAL_Exception : Exception
    {
        public DAL_Exception (string message)
            : base(message) {}

        public DAL_Exception (string message, Exception inner)
            : base(message, inner) {}
    }
}