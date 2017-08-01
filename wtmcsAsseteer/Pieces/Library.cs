using System.Reflection;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Mod info.
    /// </summary>
    internal static class Library
    {
        /// <summary>
        /// The description.
        /// </summary>
        public const string Description = "Does stuff to Cities: Skylines assets.";

        /// <summary>
        /// The name.
        /// </summary>
        public const string Name = "wtmcsAsseteer";

        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = "Asseteer (WtM)";

        /// <summary>
        /// Gets the build.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public static string Build
        {
            get
            {
                try
                {
                    AssemblyName name = Assembly.GetExecutingAssembly().GetName();
                    return name.Name + " " + name.Version.ToString() + " (" + AssemblyInfo.PreBuildStamps.DateTime.ToString("yyyy-MM-dd HH:mm") + ")";
                }
                catch
                {
                    try
                    {
                        return Name + " (" + AssemblyInfo.PreBuildStamps.DateTime.ToString("yyyy-MM-dd HH:mm") + ")";
                    }
                    catch
                    {
                        return Name;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is a debug build.
        /// </summary>
        /// <value>
        /// <c>true</c> if this is a debug build; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDebugBuild
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}