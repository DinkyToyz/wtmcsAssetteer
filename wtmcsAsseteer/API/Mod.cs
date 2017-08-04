using ColossalFramework.UI;
using ICities;
using System;
using WhatThe.Mods.CitiesSkylines.Asseteer.Reporter;

namespace WhatThe.Mods.CitiesSkylines.Asseteer
{
    /// <summary>
    /// Mod interface.
    /// </summary>
    public class Mod : IUserMod
    {
        /// <summary>
        /// The data report button.
        /// </summary>
        private UIComponent DataReportButton = null;

        /// <summary>
        /// The HTML report button.
        /// </summary>
        private UIComponent HtmlReportButton = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mod"/> class.
        /// </summary>
        public Mod()
        {
            Log.NoOp();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Mod"/> class.
        /// </summary>
        ~Mod()
        {
            Log.FlushBuffer();
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get
            {
                return Library.Description;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return Library.Title;
            }
        }

        /// <summary>
        /// Called when mod is disabled.
        /// </summary>
        public void OnDisabled()
        {
            Log.Debug(this, "OnDisabled");
            Log.FlushBuffer();
        }

        /// <summary>
        /// Called when mod is enabled.
        /// </summary>
        public void OnEnabled()
        {
            Log.Debug(this, "OnEnabled");
            Log.FlushBuffer();
        }

        /// <summary>
        /// Called when initializing mod settings UI.
        /// </summary>
        /// <param name="helper">The helper.</param>
        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                this.InitializeHelper(helper);

                helper.AddCheckbox(
                    "Create HTML report automatically",
                    Global.Settings.CreateHtmlReportOnLevelLoaded,
                    value =>
                    {
                        try
                        {
                            if (Global.Settings.CreateHtmlReportOnLevelLoaded != value)
                            {
                                Global.Settings.CreateHtmlReportOnLevelLoaded = value;
                                Global.Settings.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(this, "OnSettingsUI", ex, "CreateHtmlReportOnLevelLoaded", value);
                        }
                    });

                helper.AddCheckbox(
                    "Create text file report automatically",
                    Global.Settings.CreateDataReportOnLevelLoaded,
                    value =>
                    {
                        try
                        {
                            if (Global.Settings.CreateDataReportOnLevelLoaded != value)
                            {
                                Global.Settings.CreateDataReportOnLevelLoaded = value;
                                Global.Settings.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(this, "OnSettingsUI", ex, "CreateDataReportOnLevelLoaded", value);
                        }
                    });

                this.HtmlReportButton = (UIComponent)helper.AddButton(
                    FileSystem.CanOpenFile ? "HTML report" : "Save HTML report",
                        () =>
                        {
                            try
                            {
                                if (FileSystem.CanOpenFile)
                                {
                                    AssetReporter.SaveReports(false, true, false);
                                    AssetReporter.OpenHtmlReport();
                                }
                                else
                                {
                                    AssetReporter.SaveReports(true, true, false);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(this, "OnSettingsUI", ex, "HtmlReportButton");
                            }
                        });

                this.DataReportButton = (UIComponent)helper.AddButton(
                    FileSystem.CanOpenFile ? "Text report" : "Save text report",
                        () =>
                        {
                            try
                            {
                                if (FileSystem.CanOpenFile)
                                {
                                    AssetReporter.SaveReports(false, false, true);
                                    AssetReporter.OpenDataReport();
                                }
                                else
                                {
                                    AssetReporter.SaveReports(true, false, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(this, "OnSettingsUI", ex, "DataReportButton");
                            }
                        });
            }
            catch (Exception ex)
            {
                Log.Error(this, "OnSettingsUI", ex);
            }
        }

        /// <summary>
        /// Initializes the helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        private void InitializeHelper(UIHelperBase helper)
        {
            if (!(helper is UIHelper))
            {
                return;
            }

            try
            {
                UIComponent panel = (UIComponent)((UIHelper)helper).self;

                Log.DevDebug(this, "InitializeHelper", helper, panel);
                panel.eventVisibilityChanged += OnHelperPanelVisibilityChange;
            }
            catch (Exception ex)
            {
                Log.Error(this, "InitializeHelper", ex);
            }
        }

        /// <summary>
        /// Called when helper panel visibility changes.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="value">if set to <c>true</c> component is made visible.</param>
        private void OnHelperPanelVisibilityChange(UIComponent component, bool value)
        {
            if (!value)
            {
                return;
            }

            try
            {
                this.RefreshUsability();
            }
            catch (Exception ex)
            {
                Log.Error(this, "OnHelperPanelVisibilityChange", ex);
            }
        }

        /// <summary>
        /// Refreshes the usability for controls.
        /// </summary>
        private void RefreshUsability()
        {
            try
            {
                this.SetUsability(this.HtmlReportButton, Global.LevelLoaded || (FileSystem.CanOpenFile && AssetReporter.HtmlReportCreated));
                this.SetUsability(this.DataReportButton, Global.LevelLoaded || (FileSystem.CanOpenFile && AssetReporter.DataReportCreated));
            }
            catch (Exception ex)
            {
                Log.Error(this, "RefreshUsability", ex);
            }
        }

        /// <summary>
        /// Sets the components usability.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="usability">if set to <c>true</c> component will be usable.</param>
        private void SetUsability(UIComponent component, bool usability)
        {
            if (component != null)
            {
                if (usability)
                {
                    component.Enable();
                    component.Show();
                }
                else
                {
                    component.Hide();
                    component.Disable();
                }
            }
        }
    }
}