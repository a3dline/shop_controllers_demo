using UnityEngine;

namespace Features.GameShop
{
    [CreateAssetMenu(fileName = "ScriptableGameShopData", menuName = "Game/ScriptableGameShopData")]
    public class ScriptableGameShopData : ScriptableObject
    {
        public GameShopData Data;
    }
}