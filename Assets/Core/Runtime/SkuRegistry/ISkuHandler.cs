using System;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface ISkuHandler
    {
        void AddTransaction(IConvertible amount);
        bool IsValidTransaction(IConvertible amount);
        IReadOnlyAsyncReactiveProperty<string> BalanceString { get; }
        string SkuId { get; internal set; }
        IConvertible DefaultBalance { get; }
        IConvertible Balance { get; }
        IConvertible Add(IConvertible a, IConvertible b);
    }
}