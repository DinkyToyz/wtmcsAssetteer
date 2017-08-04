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
        /// Whether to clean HTML and use minified includes or not.
        /// </summary>
        public bool Minify = true;

        /// <summary>
        /// Gets or sets the asset count.
        /// </summary>
        /// <value>
        /// The asset count.
        /// </value>
        internal int AssetCount { get; set; }

        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>
        /// The assets.
        /// </value>
        internal IEnumerable<AssetInfo> Assets { get; set; }

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

        /// <summary>
        /// Writes the specified object to the generated output.
        /// </summary>
        /// <param name="objectToAppend">The object to append.</param>
        public void Write(object objectToAppend)
        {
            if (objectToAppend is string)
            {
                base.Write((string)objectToAppend);
            }
            else
            {
                base.Write(this.ToStringHelper.ToStringWithCulture(objectToAppend));
            }
        }
    }
}