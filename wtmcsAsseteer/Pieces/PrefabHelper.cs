using System.Collections.Generic;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Prefab utilities.
    /// </summary>
    internal static class PrefabHelper
    {
        /// <summary>
        /// Collects prefabs of the specified prefab type.
        /// </summary>
        /// <typeparam name="TPrefab">The type of the prefab.</typeparam>
        /// <returns>
        /// A sequence of assets of the specified type.
        /// </returns>
        public static IEnumerable<PrefabInfo> Collect<TPrefab>() where TPrefab : PrefabInfo
        {
            int count = PrefabCollection<TPrefab>.PrefabCount();

            for (uint i = 0; i < count; i++)
            {
                PrefabInfo prefab = PrefabCollection<TPrefab>.GetPrefab(i);
                if ((UnityEngine.Object)prefab != (UnityEngine.Object)null)
                {
                    yield return prefab;
                }
            }
        }

        /// <summary>
        /// Gets the prefab count for the prefab type.
        /// </summary>
        /// <typeparam name="TPrefab">The type of the prefab.</typeparam>
        /// <returns>The count of prefabs of the specified type.</returns>
        public static int Count<TPrefab>() where TPrefab : PrefabInfo
        {
            return PrefabCollection<TPrefab>.PrefabCount();
        }
    }
}