using Core;
using UnityEngine;

namespace Features.BalanceBar
{
    public record BalanceBarItemContext
    {
        public Transform Parent;
        public BalanceBarItemView Prefab;
        public ISkuHandler SkuHandler;
    }
}