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
        /// The assets.
        /// </summary>
        protected List<AssetInfo> Assets = null;

        /// <summary>
        /// The cleaner.
        /// </summary>
        private HtmlCleaner cleaner = new HtmlCleaner();

        /// <summary>
        /// The HTML template.
        /// </summary>
        private HtmlReportTemplate HtmlTemplate = null;

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
        /// Saves the report.
        /// </summary>
        public static void SaveReport()
        {
            AssetReporter reporter = new AssetReporter();

            reporter.Initialize();
            reporter.Save();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.HtmlTemplate = new HtmlReportTemplate();

            this.CollectAssets();
            this.AggregateAssets();

            this.HtmlTemplate.BuildInfo = Library.Build;
            this.HtmlTemplate.Assets = this.Assets.OrderBy(a => a.Type).ThenBy(a => a.BuiltIn).ThenByDescending(a => a.TrianglesSize.Weight).ToList();
            this.HtmlTemplate.MeanBuiltInAssetSizes = this.BuiltInAssetSizes.Values.OrderBy(c => c.Type == "All").ThenBy(c => c.Type);
        }

        /// <summary>
        /// Saves report from this instance.
        /// </summary>
        public void Save()
        {
            string html = this.HtmlTemplate.TransformText();
            html = cleaner.Clean(html);

            FileSystem.WriteFile(".report.html", html);
        }

        /// <summary>
        /// The built in asset counts.
        /// </summary>
        private Dictionary<string, AssetSizes> BuiltInAssetSizes = null;

        /// <summary>
        /// Aggregates the assets.
        /// </summary>
        private void AggregateAssets()
        {
            if (this.BuiltInAssetSizes != null)
            {
                return;
            }

            AssetSizes allCounts = new AssetSizes("All");
            this.BuiltInAssetSizes = new Dictionary<string, AssetSizes>();

            string curType = "";
            AssetSizes curCounts = null;

            foreach (AssetInfo asset in this.Assets)
            {
                    if (asset.Type != curType)
                    {
                        curType = asset.Type;
                        if (!this.BuiltInAssetSizes.TryGetValue(curType, out curCounts))
                        {
                            curCounts = new AssetSizes(curType);
                            this.BuiltInAssetSizes[curType] = curCounts;
                        }
                    }

                    curCounts.Add(asset);
                    allCounts.Add(asset);
            }

            this.BuiltInAssetSizes["All"] = allCounts;
            foreach (AssetSizes counts in this.BuiltInAssetSizes.Values)
            {
                counts.CalculateMeans();
            }
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
                foreach (Reference reference in asset.Props)
                {
                    List<Reference> references;
                    if (!Dependencies.TryGetValue(reference.PrefabName, out references))
                    {
                        references = new List<Reference>();
                        Dependencies[reference.PrefabName] = references;
                    }

                    references.Add(new Reference(reference.ReferenceType, asset.PrefabName, true));
                }

                foreach (Reference reference in asset.Trees)
                {
                    List<Reference> references;
                    if (!Dependencies.TryGetValue(reference.PrefabName, out references))
                    {
                        references = new List<Reference>();
                        Dependencies[reference.PrefabName] = references;
                    }

                    references.Add(new Reference(reference.ReferenceType, asset.PrefabName, true));
                }
            }

            foreach (AssetInfo asset in this.Assets)
            {
                List<Reference> references;
                if (Dependencies.TryGetValue(asset.PrefabName, out references))
                {
                    asset.AddDependencies(references);
                }
            }
        }
    }
}