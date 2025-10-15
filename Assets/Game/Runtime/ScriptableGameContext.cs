using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameContext", menuName = "Game/GameContext", order = 0)]
    public class ScriptableGameContext : ScriptableObject
    {
        public GameContext Context;
    }
}