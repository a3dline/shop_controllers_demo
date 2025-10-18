using System;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface ISkuHandler
    {
        void AddTransaction(IConvertible amount);
        bool IsValidTransaction(IConvertible amount);
        IReadOnlyAsyncReactiveProperty<string> BalanceStringProperty { get; }
        string Name { get; }
    }
}