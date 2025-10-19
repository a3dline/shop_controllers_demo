using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IRepository
    {
        UniTask UpsetAsync(string key, string value, CancellationToken token);
        UniTask<string> GetAsync(string key, CancellationToken token);    
    }
}