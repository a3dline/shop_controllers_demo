using System.Collections.Generic;

namespace Core
{
    public interface ISkuRegistrationService
    {
        void Register(in SkuRegistry registry);
        IEnumerable<ISkuHandler> SkuHandlers { get; }
    }
}