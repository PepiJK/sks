using System;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class DALNotFoundException : DALException
    {
        public DALNotFoundException (string message)
            : base(message) {}

        public DALNotFoundException (string message, Exception inner)
            : base(message, inner) {}
    }
}