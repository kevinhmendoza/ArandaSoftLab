using System;

namespace Core.Entities.Contracts
{
    public interface ISistema
    {
        string UserName { get; }
        DateTime Now { get; }
        string IpAddress { get; }
        string HostName { get; }
    }
}
