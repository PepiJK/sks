using System;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    public class BL_Exception : Exception
    {
        public BL_Exception (string message, Exception inner)
            : base(message, inner) {}
    }
}