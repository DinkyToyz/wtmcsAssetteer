using System;
using ColossalFramework;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Information retrieval methods.
    /// </summary>
    internal static class InfoHelper
    {
        /// <summary>
        /// Gets the name of the building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The buildings name.</returns>
        public static string GetBuildingName(ushort buildingId)
        {
            try
            {
                BuildingManager manager = Singleton<BuildingManager>.instance;
                string name = manager.GetBuildingName(buildingId, new InstanceID());

                return String.IsNullOrEmpty(name) ? (string)null : name;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the name of the district.
        /// </summary>
        /// <param name="district">The district.</param>
        /// <returns>The name of the district.</returns>
        public static string GetDistrictName(int district)
        {
            DistrictManager districtManager = Singleton<DistrictManager>.instance;

            return districtManager.GetDistrictName(district);
        }
    }
}