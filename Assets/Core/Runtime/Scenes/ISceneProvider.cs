namespace Core
{
    public interface ISceneProvider
    {
        public enum Type
        {
            FromBundle,
            BuildIn
        }

        ISceneReference GetSceneReference(string sceneName);
    }
}