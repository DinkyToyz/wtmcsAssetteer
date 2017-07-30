using System.Collections.Generic;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Creates an asset report.
    /// </summary>
    internal class AssetReporter
    {
        /// <summary>
        /// Collects assets of all handled types.
        /// </summary>
        /// <returns>The list of assets.</returns>
        private List<AssetInfo> CollectAssets()
        {
            List<AssetInfo> assets = new List<AssetInfo>(
                BuildingAssetInfo.PrefabCount +
                CitizenAssetInfo.PrefabCount +
                VehicleAssetInfo.PrefabCount +
                PropAssetInfo.PrefabCount +
                TreeAssetInfo.PrefabCount);

            assets.AddRange(BuildingAssetInfo.Collection);
            assets.AddRange(CitizenAssetInfo.Collection);
            assets.AddRange(VehicleAssetInfo.Collection);
            assets.AddRange(PropAssetInfo.Collection);
            assets.AddRange(TreeAssetInfo.Collection);

            return assets;
        }
    }
}