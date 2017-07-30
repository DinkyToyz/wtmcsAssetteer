namespace WhatThe.Mods.CitiesSkylines.Asseteer.Modder
{
    /// <summary>
    /// Asset handler interface.
    /// </summary>
    internal interface IAsseteer
    {
        /// <summary>
        /// Logs the assets.
        /// </summary>
        void LogAssets();

        /// <summary>
        /// Logs the objects.
        /// </summary>
        void LogObjects();

        /// <summary>
        /// Saves the assets.
        /// </summary>
        void SaveAssets();

        /// <summary>
        /// Saves the objects.
        /// </summary>
        void SaveObjects();
    }
}