using System;
using System.Diagnostics.CodeAnalysis;


namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class BLException : Exception
    {
        public BLException (string message, Exception inner)
            : base(message, inner) {}
    }
}