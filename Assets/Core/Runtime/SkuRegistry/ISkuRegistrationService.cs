using System.Collections.Generic;

namespace Core
{
    public interface ISkuRegistrationService
    {
        void Register(ISkuDefinition definition);
        IEnumerable<ISkuHandler> SkuHandlers { get; }
        ISkuHandler GetSkuHandler(string skuId);
        string GetSkuDisplayName(string skuId);
    }
}