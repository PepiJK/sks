using System;
using System.Diagnostics.CodeAnalysis;


namespace KochWermann.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceAgentNoResultException : Exception
    {
        public ServiceAgentNoResultException (string message, Exception inner): base(message, inner) {}
    }
}