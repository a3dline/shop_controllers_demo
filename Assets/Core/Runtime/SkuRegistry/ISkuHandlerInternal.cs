using System;

namespace Core
{
    public interface ISkuHandlerInternal : ISkuHandler
    {
        IConvertible TakeNewBalanceAndClear();
        bool IsDirty { get; }
        void UpdateBalance(IConvertible amount);
        new IConvertible DefaultBalance { get; set; }
    }
}