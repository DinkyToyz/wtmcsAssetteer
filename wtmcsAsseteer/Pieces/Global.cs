using System.Collections.Generic;
using WhatThe.Mods.CitiesSkylines.Asseteer.Modder;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Global objects.
    /// </summary>
    internal static class Global
    {
        /// <summary>
        /// Whether to use the aseteers.
        /// </summary>
        public static readonly bool UseAseteers = false;

        /// <summary>
        /// The game has been started.
        /// </summary>
        public static bool GameStarted = false;

        /// <summary>
        /// A level is loaded.
        /// </summary>
        public static bool LevelLoaded = false;

        /// <summary>
        /// The mod enabled
        /// </summary>
        public static bool ModEnabled = false;

        /// <summary>
        /// The settings
        /// </summary>
        public static SerializableSettings Settings = SerializableSettings.Load();

        /// <summary>
        /// The asset handlers.
        /// </summary>
        private static List<IAsseteer> asseteers;

        /// <summary>
        /// Gets the asset handlers.
        /// </summary>
        /// <value>
        /// The asset handlers.
        /// </value>
        public static IEnumerable<IAsseteer> Asseteers
        {
            get
            {
                if (asseteers == null)
                {
                    asseteers = new List<IAsseteer>();
                    asseteers.Add(new BuildingAsseteer());
                }

                return asseteers;
            }
        }

        /// <summary>
        /// Frees the asset handlers.
        /// </summary>
        public static void FreeAsseteers()
        {
            if (asseteers != null)
            {
                asseteers.Clear();
                asseteers = null;
            }
        }
    }
}