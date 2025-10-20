using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Features.GameShop
{
    public static class SkuDefinitionProvider
    {
        private static SkuInfo[] _cachedSkuInfos;

        public static SkuInfo[] GetSkuInfos()
        {
            if (_cachedSkuInfos != null)
            {
                return _cachedSkuInfos;
            }

            var skuInfos = new List<SkuInfo>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (typeof(ISkuDefinition).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        if (Activator.CreateInstance(type) is ISkuDefinition instance)
                        {
                            var skuInfo = new SkuInfo
                                          {
                                              SkuId = instance.SkuId,
                                              DisplayName = instance.DisplayName
                                          };

                            if (!string.IsNullOrEmpty(skuInfo.SkuId) && skuInfos.All(s => s.SkuId != skuInfo.SkuId))
                            {
                                skuInfos.Add(skuInfo);
                            }
                        }
                    }
                }
            }

            _cachedSkuInfos = skuInfos.ToArray();
            return _cachedSkuInfos;
        }

        public struct SkuInfo
        {
            public string SkuId;
            public string DisplayName;
        }
    }
}
