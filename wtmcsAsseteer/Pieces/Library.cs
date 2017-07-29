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