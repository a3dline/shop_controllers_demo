using Cysharp.Threading.Tasks;

namespace Core
{
    public enum SceneProviderType
    {
        FromBundle,
        BuildIn
    }
    
    public interface ISceneProvider
    {
        ISceneReference GetSceneReference(string sceneName);
    }
}