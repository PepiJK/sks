using System;

namespace KochWermann.SKS.Package.BusinessLogic.Entities
{
    public class BL_Exeption : Exception
    {
        public BL_Exeption (string message, Exception inner)
            : base(message, inner) {}
    }
}