using ICities;
using System;
using WhatThe.Mods.CitiesSkylines.Asseteer.Modder;

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
            Log.Debug(this, "OnCreated");
            base.OnCreated(loading);
        }

        /// <summary>
        /// Called when map (etc) is loaded.
        /// </summary>
        /// <param name="mode">The load mode.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            try
            {
                Log.Debug(this, "OnLevelLoaded", "Begin");

                try
                {
                    // Save asset report.
                    try
                    {
                        Reporter.AssetReporter.SaveReport();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(this, "OnLevelLoaded", ex, "SaveReport");
                    }

                    // Save assets and objects.
                    try
                    {
                        foreach (IAsseteer asseteer in Global.Asseteers)
                        {
                            // Save assets.
                            try
                            {
                                asseteer.SaveAssets();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(this, "OnLevelLoaded", ex, "SaveAssets", asseteer.GetType().ToString());
                            }

                            // Save objects.
                            try
                            {
                                asseteer.SaveObjects();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(this, "OnLevelLoaded", ex, "SaveObjects", asseteer.GetType().ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(this, "OnLevelLoaded", ex, "SaveAssetsAndObjects");
                    }
                }
                finally
                {
                    Log.Debug(this, "OnLevelLoaded", "Base");
                    base.OnLevelLoaded(mode);
                }

                Log.Debug(this, "OnLevelLoaded", "End");
            }
            finally
            {
                Log.Buffer = false;
            }
        }
    }
}