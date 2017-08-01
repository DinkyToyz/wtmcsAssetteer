using System.Collections.Generic;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.Template
{
    /// <summary>
    /// HTML report template code and values part.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.Template.HtmlReportTemplateBase" />
    partial class HtmlReportTemplate
    {
        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>
        /// The assets.
        /// </value>
        internal IList<AssetInfo> Assets { get; set; }

        /// <summary>
        /// Gets or sets the build information.
        /// </summary>
        /// <value>
        /// The build information.
        /// </value>
        internal string BuildInfo { get; set; }

        /// <summary>
        /// Gets or sets the mean built in asset sizes.
        /// </summary>
        /// <value>
        /// The asset sizes.
        /// </value>
        internal IEnumerable<AssetSizes> MeanBuiltInAssetSizes { get; set; }
    }
}