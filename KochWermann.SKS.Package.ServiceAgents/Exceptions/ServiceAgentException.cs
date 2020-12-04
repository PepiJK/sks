using System;
using System.Diagnostics.CodeAnalysis;


namespace KochWermann.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceAgentException : Exception
    {
        public ServiceAgentException (string message, Exception inner): base(message, inner) {}
    }
}