using System;

namespace Core
{
    public interface ISkuDefinition
    {
        string SkuId { get; }
        string DisplayName { get; }
        ISkuHandler Handler { get; }
    }
}