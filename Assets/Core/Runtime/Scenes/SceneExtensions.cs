using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public static class SceneExtensions
    {
        public static T GetRootComponent<T>(this Scene scene) where T : Component
        {
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                var component = rootGameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}