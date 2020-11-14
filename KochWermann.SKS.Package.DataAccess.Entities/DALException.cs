using System;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class DALException : Exception
    {
        public DALException (string message)
            : base(message) {}

        public DALException (string message, Exception inner)
            : base(message, inner) {}
    }
}