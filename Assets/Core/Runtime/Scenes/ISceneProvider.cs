namespace Core
{
    public interface ISceneProvider
    {
        ISceneReference GetSceneReference(string sceneName);
    }
}