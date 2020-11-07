using System;
using System.Diagnostics.CodeAnalysis;


namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class BL_Exception : Exception
    {
        public BL_Exception (string message, Exception inner)
            : base(message, inner) {}
    }
}