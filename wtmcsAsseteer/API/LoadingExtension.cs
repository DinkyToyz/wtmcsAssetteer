using ICities;
using System;
using WhatThe.Mods.CitiesSkylines.Asseteer.Modder;
using WhatThe.Mods.CitiesSkylines.Asseteer.Reporter;

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

                Global.LevelLoaded = true;

                try
                {
                    // Save asset report.
                    try
                    {
                        AssetReporter.SaveReports(true, Global.Settings.CreateHtmlReportOnLevelLoaded, Global.Settings.CreateDataReportOnLevelLoaded);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(this, "OnLevelLoaded", ex);
                    }

                    // Save assets and objects.
                    if (Global.UseAseteers)
                    {
                        try
                        {
                            foreach (IAsseteer asseteer in Global.Asseteers)
                            {
                                Log.Debug(this, "OnLevelLoaded", "Save", asseteer.GetType().ToString());

                                // Save assets.
                                try
                                {
                                    Log.Debug(this, "OnLevelLoaded", asseteer.GetType().ToString());
                                    asseteer.SaveAssets();
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(this, "OnLevelLoaded", ex, asseteer.GetType().ToString());
                                }

                                // Save objects.
                                try
                                {
                                    Log.Debug(this, "OnLevelLoaded", asseteer.GetType().ToString());
                                    asseteer.SaveObjects();
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(this, "OnLevelLoaded", ex, asseteer.GetType().ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(this, "OnLevelLoaded", ex);
                        }
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

        /// <summary>
        /// Called when level is unloading.
        /// </summary>
        public override void OnLevelUnloading()
        {
            Log.Debug(this, "OnLevelUnloading");

            Global.LevelLoaded = false;

            base.OnLevelUnloading();
        }

        /// <summary>
        /// Called when released.
        /// </summary>
        public override void OnReleased()
        {
            Log.Debug(this, "OnReleased");

            Global.LevelLoaded = false;

            base.OnReleased();
        }
    }
}