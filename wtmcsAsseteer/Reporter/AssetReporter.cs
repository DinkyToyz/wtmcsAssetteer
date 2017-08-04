using System;
using System.Collections.Generic;
using System.Linq;
using WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.Template;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Creates an asset report.
    /// </summary>
    internal class AssetReporter
    {
        /// <summary>
        /// Whether the data report has been created.
        /// </summary>
        public static bool DataReportCreated = false;

        /// <summary>
        /// Whether the HTML report has been created.
        /// </summary>
        public static bool HtmlReportCreated = false;

        /// <summary>
        /// The assets.
        /// </summary>
        protected List<AssetInfo> Assets = null;

        /// <summary>
        /// The built in asset counts.
        /// </summary>
        private Dictionary<AssetInfo.AssetTypes, AssetSizes> BuiltInAssetSizes = null;

        /// <summary>
        /// The cleaner.
        /// </summary>
        private HtmlCleaner cleaner = new HtmlCleaner();

        /// <summary>
        /// Gets the built in assets.
        /// </summary>
        /// <value>
        /// The built in assets.
        /// </value>
        protected IEnumerable<AssetInfo> BuiltInAssets => this.Assets.Where(a => a.BuiltIn);

        /// <summary>
        /// Gets the custom assets.
        /// </summary>
        /// <value>
        /// The custom assets.
        /// </value>
        protected IEnumerable<AssetInfo> CustomAssets => this.Assets.Where(a => a.Custom);

        /// <summary>
        /// Gets the workshop assets.
        /// </summary>
        /// <value>
        /// The workshop assets.
        /// </value>
        protected IEnumerable<AssetInfo> WorkshopAssets => this.Assets.Where(a => a.Workshop);

        /// <summary>
        /// Gets the category counts.
        /// </summary>
        /// <value>
        /// The category counts.
        /// </value>
        private IEnumerable<CategoryCount> CategoryCounts => this.Assets.GroupBy(a => new { a.RawCategory, a.Category }, (c, al) => new CategoryCount(c.RawCategory, c.Category, al.Count()));

        /// <summary>
        /// Gets the ordered assets.
        /// </summary>
        /// <value>
        /// The ordered assets.
        /// </value>
        private IEnumerable<AssetInfo> OrderedAssets => this.Assets.OrderBy(a => a.AssetType).ThenBy(a => a.BuiltIn).ThenByDescending(a => a.Main.TrianglesWeight);

        /// <summary>
        /// Gets the ordered built in asset sizes.
        /// </summary>
        /// <value>
        /// The ordered built in asset sizes.
        /// </value>
        private IEnumerable<AssetSizes> OrderedBuiltInAssetSizes => this.BuiltInAssetSizes.Values.OrderBy(s => s.AssetType == AssetInfo.AssetTypes.All).ThenBy(s => s.AssetType);

        /// <summary>
        /// Gets the ordered category counts.
        /// </summary>
        /// <value>
        /// The ordered category counts.
        /// </value>
        private IEnumerable<CategoryCount> OrderedCategoryCounts => this.CategoryCounts.OrderBy(c => c.Count).ThenBy(c => c.Category.ToLowerInvariant());

        /// <summary>
        /// Opens the data report.
        /// </summary>
        public static void OpenDataReport()
        {
            FileSystem.OpenFile(".report.txt");
        }

        /// <summary>
        /// Opens the HTML report.
        /// </summary>
        public static void OpenHtmlReport()
        {
            FileSystem.OpenFile(".report.html");
        }

        /// <summary>
        /// Saves the reports.
        /// </summary>
        public static void SaveReports(bool force = false, bool htmlReport = true, bool dataReport = true)
        {
            htmlReport = htmlReport && (force || !HtmlReportCreated);
            dataReport = dataReport && (force || !DataReportCreated);

            if (htmlReport || dataReport)
            {
                AssetReporter reporter = new AssetReporter();

                if (htmlReport)
                {
                    reporter.SaveHtmlReport();
                }

                if (dataReport)
                {
                    reporter.SaveDataReport();
                }
            }
        }

        /// <summary>
        /// Logs asset sizes.
        /// </summary>
        public void DebugLog()
        {
            if (Log.LogALot && this.Assets != null)
            {
                foreach (CategoryCount count in this.OrderedCategoryCounts)
                {
                    Log.DevDebug(this, "Category", count);
                }

                if (this.BuiltInAssetSizes != null)
                {
                    foreach (AssetSizes sizes in this.BuiltInAssetSizes.Values.OrderBy(s => s.AssetType == AssetInfo.AssetTypes.All).ThenBy(s => s.AssetType))
                    {
                        Log.DevDebug(this, "Sizes", sizes);
                    }
                }

                foreach (AssetInfo asset in this.Assets.OrderBy(a => a.AssetType).ThenBy(a => a.BuiltIn).ThenByDescending(a => a.Main.TrianglesWeight))
                {
                    Log.DevDebug(this, "Asset", asset);
                }
            }
        }

        /// <summary>
        /// Saves the data report.
        /// </summary>
        public void SaveDataReport()
        {
            if (!this.Initialize())
            {
                return;
            }

            Log.Debug(this, "SaveDateReport", "Begin");

            try
            {
                List<string> report = new List<string>(this.Assets.Count + 100);

                report.AddRange(this.OrderedCategoryCounts.Select(c => c.ToString("Category")));

                if (this.BuiltInAssetSizes != null)
                {
                    report.AddRange(this.OrderedBuiltInAssetSizes.Select(s => s.ToString("Sizes")));
                }

                report.AddRange(this.OrderedAssets.Select(a => a.ToString("Asset")));

                if (report.Count > 0)
                {
                    report.Add("");

                    Log.DevDebug(this, "SaveDateReport", "Write");
                    FileSystem.WriteFile(".report.txt", String.Join(Environment.NewLine, report.ToArray()));
                    Log.DevDebug(this, "SaveDateReport", "Saved");

                    DataReportCreated = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(this, "SaveDataReport", ex);
            }

            Log.Debug(this, "SaveDateReport", "End");
        }

        /// <summary>
        /// Saves report from this instance.
        /// </summary>
        public void SaveHtmlReport()
        {
            if (!this.Initialize())
            {
                return;
            }

            Log.Debug(this, "SaveHtmlReport", "Begin");

            try
            {
                HtmlReportTemplate template = new HtmlReportTemplate();

                template.BuildInfo = Library.Build;
                template.AssetCount = this.Assets.Count;
                template.Assets = this.OrderedAssets;
                template.MeanBuiltInAssetSizes = this.OrderedBuiltInAssetSizes;

                Log.DevDebug(this, "SaveHtmlReport", "Transform");
                string html = template.TransformText();

                Log.DevDebug(this, "SaveHtmlReport", "Clean");
                if (template.Minify)
                {
                    html = cleaner.Clean(html);
                }

                Log.DevDebug(this, "SaveHtmlReport", "Write");
                FileSystem.WriteFile(".report.html", html);
                Log.DevDebug(this, "SaveHtmlReport", "Saved");

                HtmlReportCreated = true;
            }
            catch (Exception ex)
            {
                Log.Error(this, "SaveHtmlReport", ex);
            }

            Log.Debug(this, "SaveHtmlReport", "End");
        }

        /// <summary>
        /// Aggregates the assets.
        /// </summary>
        private void AggregateAssets()
        {
            if (this.BuiltInAssetSizes != null)
            {
                return;
            }

            Log.Debug(this, "AggregateAssets", "Begin");

            AssetSizes allCounts = new AssetSizes(AssetInfo.AssetTypes.All);
            this.BuiltInAssetSizes = new Dictionary<AssetInfo.AssetTypes, AssetSizes>();

            AssetInfo.AssetTypes curType = AssetInfo.AssetTypes.Unknown;
            AssetSizes curCounts = null;

            foreach (AssetInfo asset in this.Assets)
            {
                if (asset.AssetType != curType)
                {
                    curType = asset.AssetType;
                    if (!this.BuiltInAssetSizes.TryGetValue(curType, out curCounts))
                    {
                        curCounts = new AssetSizes(curType);
                        this.BuiltInAssetSizes[curType] = curCounts;
                    }
                }

                curCounts.Add(asset);
                allCounts.Add(asset);
            }

            this.BuiltInAssetSizes[AssetInfo.AssetTypes.All] = allCounts;

            Log.Debug(this, "AggregateAssets", "End");
        }

        /// <summary>
        /// Collects assets of all handled types.
        /// </summary>
        /// <returns>The list of assets.</returns>
        private void CollectAssets()
        {
            if (this.Assets != null)
            {
                return;
            }

            Log.DevDebug(this, "CollectAssets", "Begin");

            this.Assets = new List<AssetInfo>(
                BuildingAssetInfo.PrefabCount +
                CitizenAssetInfo.PrefabCount +
                VehicleAssetInfo.PrefabCount +
                PropAssetInfo.PrefabCount +
                TreeAssetInfo.PrefabCount);

            Assets.AddRange(TreeAssetInfo.Collection);
            Assets.AddRange(PropAssetInfo.Collection);
            Assets.AddRange(CitizenAssetInfo.Collection);
            Assets.AddRange(VehicleAssetInfo.Collection);
            Assets.AddRange(BuildingAssetInfo.Collection);

            Dictionary<string, List<Reference>> Dependencies = new Dictionary<string, List<Reference>>();

            foreach (AssetInfo asset in this.Assets)
            {
                foreach (Reference reference in asset.References.Where(r => !String.IsNullOrEmpty(r.PrefabName)))
                {
                    List<Reference> references;
                    if (!Dependencies.TryGetValue(reference.PrefabName, out references))
                    {
                        references = new List<Reference>();
                        Dependencies[reference.PrefabName] = references;
                    }

                    references.Add(new Reference(reference.ReferenceType, asset, true));
                }
            }

            foreach (AssetInfo asset in this.Assets.Where(a => !String.IsNullOrEmpty(a.PrefabName)))
            {
                List<Reference> references;
                if (Dependencies.TryGetValue(asset.PrefabName, out references))
                {
                    asset.AddDependencies(references);
                }
            }

            Log.DevDebug(this, "CollectAssets", "End");
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private bool Initialize()
        {
            if (this.Assets != null && this.BuiltInAssetSizes != null)
            {
                return true;
            }

            if (!Global.LevelLoaded)
            {
                return false;
            }

            Log.Debug(this, "Initialize", "Begin");

            this.CollectAssets();
            this.AggregateAssets();

            Log.Debug(this, "Initialize", "End");
            return true;
        }

        /// <summary>
        /// Holds a category count.
        /// </summary>
        private struct CategoryCount
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CategoryCount" /> struct.
            /// </summary>
            /// <param name="rawCategory">The raw category.</param>
            /// <param name="category">The category.</param>
            /// <param name="count">The count.</param>
            public CategoryCount(string rawCategory, string category, int count)
            {
                this.RawCategory = rawCategory;
                this.Category = category;
                this.Count = count;
            }

            /// <summary>
            /// Gets or sets the category.
            /// </summary>
            /// <value>
            /// The category.
            /// </value>
            public string Category { get; private set; }

            /// <summary>
            /// Gets or sets the count.
            /// </summary>
            /// <value>
            /// The count.
            /// </value>
            public int Count { get; private set; }

            /// <summary>
            /// Gets or sets the raw category.
            /// </summary>
            /// <value>
            /// The raw category.
            /// </value>
            public string RawCategory { get; private set; }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return this.ToString(null);
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <param name="prefix">The prefix.</param>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public string ToString(string prefix)
            {
                Log.InfoList info = new Log.InfoList();
                info.Add(prefix, this.Category, this.Count, this.RawCategory);
                return info.ToString();
            }
        }
    }
}