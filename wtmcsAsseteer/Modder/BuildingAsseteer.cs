using ColossalFramework;
using System;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Modder
{
    /// <summary>
    /// The building asset handler.
    /// </summary>
    internal class BuildingAsseteer : IAsseteer
    {
        /// <summary>
        /// Logs the assets.
        /// </summary>
        public void LogAssets()
        {
            try
            {
                foreach (BuildingInfo building in PrefabHelper.Collect<BuildingInfo>())
                {
                    Log.InfoList info = this.GetBuildingLogInfo(null, building);

                    if (info != null)
                    {
                        Log.DevDebug(this, "LogAssets", info);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(this, "LogAssets", ex);
            }
        }

        /// <summary>
        /// Logs the objects.
        /// </summary>
        public void LogObjects()
        {
            try
            {
                BuildingManager buildingManager = Singleton<BuildingManager>.instance;
                DistrictManager districtManager = Singleton<DistrictManager>.instance;

                Building[] buildings = buildingManager.m_buildings.m_buffer;

                for (ushort id = 0; id < buildings.Length; id++)
                {
                    if (buildings[id].Info != null)
                    {
                        Log.InfoList info = this.GetBuildingLogInfo(null, buildingManager, districtManager, id, ref buildings[id]);

                        if (info != null)
                        {
                            Log.DevDebug(this, "LogObjects", info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(this, "LogObjects", ex);
            }
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="buildingManager">The building manager.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="identityType">Type of the identity.</param>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="buildingInfo">The building information.</param>
        /// <returns>
        /// A new item.
        /// </returns>
        public Configuration.Item NewItem(BuildingManager buildingManager, Configuration configuration, string identityType, ushort buildingId, BuildingInfo buildingInfo)
        {
            Configuration.Item item = configuration.NewItem();

            item.Identity.Set(identityType, buildingInfo.name);

            item.Values.Set("LowWealthTourists", buildingInfo.m_buildingAI.GetLowWealthTourists());
            item.Values.Set("MediumWealthTourists", buildingInfo.m_buildingAI.GetMediumWealthTourists());
            item.Values.Set("HighWealthTourists", buildingInfo.m_buildingAI.GetHighWealthTourists());

            if (configuration.ItemType == Configuration.ItemTypes.Objects)
            {
                string name = buildingManager.GetBuildingName(buildingId, buildingInfo.m_instanceID);
                if (!string.IsNullOrEmpty(name))
                {
                    item.Values.Set("Building", name);
                }
            }

            return item;
        }

        /// <summary>
        /// Saves the assets.
        /// </summary>
        public void SaveAssets()
        {
            try
            {
                Configuration config = new Configuration(Configuration.ItemTypes.Assets, Configuration.AssetTypes.Buildings, null);

                foreach (BuildingInfo building in PrefabHelper.Collect<BuildingInfo>())
                {
                    config.Add(this.NewItem(null, config, "Asset", 0, building));
                }

                config.Save();
            }
            catch (Exception ex)
            {
                Log.Error(this, "SaveAssets", ex);
            }
        }

        /// <summary>
        /// Saves the objects.
        /// </summary>
        public void SaveObjects()
        {
            try
            {
                Configuration config = new Configuration(Configuration.ItemTypes.Objects, Configuration.AssetTypes.Buildings, null);

                BuildingManager buildingManager = Singleton<BuildingManager>.instance;
                Building[] buildings = buildingManager.m_buildings.m_buffer;

                for (ushort id = 0; id < buildings.Length; id++)
                {
                    if (buildings[id].Info != null && (buildings[id].m_flags & Building.Flags.Created) != Building.Flags.None)
                    {
                        config.Add(this.NewItem(buildingManager, config, "Object", id, buildings[id].Info));
                    }
                }

                config.Save();
            }
            catch (Exception ex)
            {
                Log.Error(this, "SaveObjects", ex);
            }
        }

        /// <summary>
        /// Gets the building log information.
        /// </summary>
        /// <param name="logInfo">The log information.</param>
        /// <param name="buildingManager">The building manager.</param>
        /// <param name="districtManager">The district manager.</param>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="building">The building.</param>
        /// <returns>The building log information.</returns>
        private Log.InfoList GetBuildingLogInfo(Log.InfoList logInfo, BuildingManager buildingManager, DistrictManager districtManager, ushort buildingId, ref Building building)
        {
            if (building.Info == null)
            {
                return logInfo;
            }

            if (logInfo == null)
            {
                logInfo = new Log.InfoList();
            }

            string name;

            name = buildingManager.GetBuildingName(buildingId, building.Info.m_instanceID);
            logInfo.Add("Id", buildingId);
            if (!String.IsNullOrEmpty(name))
            {
                logInfo.Add("Name", name);
            }

            byte districtId = districtManager.GetDistrict(building.m_position);
            name = districtManager.GetDistrictName(districtId);
            if (!String.IsNullOrEmpty(name))
            {
                logInfo.Add("District", districtId, name);
            }

            logInfo.Add("Created", (building.m_flags & Building.Flags.Created) != Building.Flags.None ? "Yes" : "No");

            return this.GetBuildingLogInfo(logInfo, building.Info);
        }

        /// <summary>
        /// Gets the building log information.
        /// </summary>
        /// <param name="logInfo">The log information.</param>
        /// <param name="buildingInfo">The building information.</param>
        /// <returns>The building log information.</returns>
        private Log.InfoList GetBuildingLogInfo(Log.InfoList logInfo, BuildingInfo buildingInfo)
        {
            if (buildingInfo == null)
            {
                return logInfo;
            }

            if (logInfo == null)
            {
                logInfo = new Log.InfoList();
            }

            logInfo.Add("AI", buildingInfo.m_buildingAI.GetType());
            logInfo.Add("InfoName", buildingInfo.name);

            logInfo.Add("LowWealthTourists", buildingInfo.m_buildingAI.GetLowWealthTourists());
            logInfo.Add("MediumWealthTourists", buildingInfo.m_buildingAI.GetMediumWealthTourists());
            logInfo.Add("HighWealthTourists", buildingInfo.m_buildingAI.GetHighWealthTourists());

            return logInfo;
        }
    }
}