using ColossalFramework;
using System;
using System.Text.RegularExpressions;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Information retrieval methods.
    /// </summary>
    public static class InfoHelper
    {
        /// <summary>
        /// The clean category patterns.
        /// </summary>
        private static Regex[] CleanCategoryPatterns = null;

        /// <summary>
        /// Cleans the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static string CleanCategory(string category)
        {
            if (CleanCategoryPatterns == null)
            {
                CleanCategoryPatterns = new Regex[]
                    {
                        // Remove from end, regardless if start.
                        new Regex(@"(.+)(?:Modderpack|Expansion\d+|Others|Default|Category\d+|Large|Services|Department)$", RegexOptions.Compiled),

                        // Remove from start, regardless if end.
                        new Regex(@"^(?:Beautification|Props(?:Common)?|PublicTransport|Monument)(.+)", RegexOptions.Compiled),

                        // Remove from start, depending on end.
                        new Regex(@"^.*?(Forestry|Oil|Ore|Tourist|Farming)$", RegexOptions.Compiled),

                        // Remove from end, depending on start.
                        new Regex(@"^(FireDepartment|Billboards|Landscaping|Industrial|Parks|Residential(?!(?:High|Low))).*$", RegexOptions.Compiled),

                        // Remove from start, depending on end.
                        new Regex(@"^.*?(Heating|Maintenance|Transport|Leisure|Tourist)$", RegexOptions.Compiled)
                    };
            }

            foreach (Regex pattern in CleanCategoryPatterns)
            {
                category = pattern.Replace(category, "$1");
            }

            return category;
        }

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