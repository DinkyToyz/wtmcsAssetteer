using System;
using ICities;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Mod stuff loader.
    /// </summary>
    public class LoadingExtension : LoadingExtensionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingExtension"/> class.
        /// </summary>
        public LoadingExtension()
            : base()
        {
            Log.Debug(this, "Constructed");
        }

        /// <summary>
        /// Called when something is created.
        /// </summary>
        /// <param name="loading">The loading.</param>
        public override void OnCreated(ILoading loading)
        {
            Log.Debug(this, "OnCreated", "Begin");

            try
            {
                foreach (IAsseteer asseteer in Global.Asseteers)
                {
                    asseteer.SaveAssets();
                }
            }
            catch (Exception ex)
            {
                Log.Error(this, "OnCreated", ex);
            }
            finally
            {
                Log.Debug(this, "OnCreated", "Base");
                base.OnCreated(loading);
            }

            Log.Debug(this, "OnCreated", "End");
        }

        /// <summary>
        /// Called when map (etc) is loaded.
        /// </summary>
        /// <param name="mode">The load mode.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            Log.Debug(this, "OnLevelLoaded", "Begin");

            try
            {
                foreach (IAsseteer asseteer in Global.Asseteers)
                {
                    asseteer.SaveObjects();
                }
            }
            catch (Exception ex)
            {
                Log.Error(this, "OnLevelLoaded", ex);
            }
            finally
            {
                Log.Debug(this, "OnLevelLoaded", "Base");
                base.OnLevelLoaded(mode);
            }

            Log.Debug(this, "OnLevelLoaded", "End");
        }
    }
}