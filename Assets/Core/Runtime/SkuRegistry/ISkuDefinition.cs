using System;

namespace Core
{
    public interface ISkuDefinition
    {
        string SkuId { get; }
        string DisplayName { get; }
        Type SkuValueType { get; }
        ISkuHandler Handler { get; }
    }
}