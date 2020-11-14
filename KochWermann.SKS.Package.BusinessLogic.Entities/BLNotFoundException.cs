using System;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class BLNotFoundException : BLException
    {
        public BLNotFoundException (string message, Exception inner)
            : base(message, inner) {}
    }
}