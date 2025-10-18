using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IRepository
    {
        UniTask UpsetAsync(string key, string value);
        UniTask<string> GetAsync(string key);    
    }
}