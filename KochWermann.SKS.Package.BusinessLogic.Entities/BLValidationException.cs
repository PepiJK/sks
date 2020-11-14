using System;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class BLValidationException : BLException
    {
        public BLValidationException (string message, Exception inner)
            : base(message, inner) {}
    }
}