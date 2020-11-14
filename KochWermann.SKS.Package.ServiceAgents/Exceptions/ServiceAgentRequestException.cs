using System;
using System.Diagnostics.CodeAnalysis;


namespace KochWermann.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceAgentRequestException : Exception
    {
        public ServiceAgentRequestException (string message): base(message) {}
    }
}