using System;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    public class BL_NotFound_Exception : Exception
    {
        public BL_NotFound_Exception (string message, Exception inner)
            : base(message, inner) {}
    }
}