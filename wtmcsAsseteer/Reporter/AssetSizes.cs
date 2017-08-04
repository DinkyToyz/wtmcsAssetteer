using System;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Asset data counter.
    /// </summary>
    internal class AssetSizes
    {
        /// <summary>
        /// The source type for which to count.
        /// </summary>
        private ObjectInfo.SourceTypes countForSourceType = ObjectInfo.SourceTypes.BuiltIn;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSizes"/> class.
        /// </summary>
        /// <param name="assetType">The type.</param>
        public AssetSizes(AssetInfo.AssetTypes assetType)
        {
            this.AssetType = assetType;

            this.TotalAssetCount = 0;
            this.CustomAssetCount = 0;
            this.BuiltInAssetCount = 0;
            this.WorkshopAssetCount = 0;

            this.Main = new Values();
            this.Lod = new Values();
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public AssetInfo.AssetTypes AssetType { get; private set; }

        /// <summary>
        /// The built in asset count.
        /// </summary>
        public long BuiltInAssetCount { get; private set; }

        /// <summary>
        /// The custom asset count.
        /// </summary>
        public long CustomAssetCount { get; private set; }

        /// <summary>
        /// The lod asset values.
        /// </summary>
        public Values Lod { get; private set; }

        /// <summary>
        /// The main asset values.
        /// </summary>
        public Values Main { get; private set; }

        /// <summary>
        /// Gets the asset count.
        /// </summary>
        /// <value>
        /// The asset count.
        /// </value>
        public long TotalAssetCount { get; private set; }

        /// <summary>
        /// The workshop asset count
        /// </summary>
        public long WorkshopAssetCount { get; private set; }

        /// <summary>
        /// Adds the specified asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Add(AssetInfo asset)
        {
            this.TotalAssetCount++;

            if (asset.BuiltIn)
            {
                this.BuiltInAssetCount++;
            }

            if (asset.Custom)
            {
                this.CustomAssetCount++;
            }

            if (asset.Workshop)
            {
                this.WorkshopAssetCount++;
            }

            if (asset.SourceType == this.countForSourceType || this.countForSourceType == ObjectInfo.SourceTypes.Unknown)
            {
                if (asset.Main.InitializedAny)
                {
                    this.Main.Add(asset.Main);
                }

                if (asset.Lod.InitializedAny)
                {
                    this.Lod.Add(asset.Lod);
                }
            }
        }

        /// <summary>
        /// Fills the information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void FillInfo(Log.InfoList info)
        {
            info.Add("AssetType", this.AssetType);
            info.Add("Counts", this.TotalAssetCount, this.BuiltInAssetCount, this.CustomAssetCount, this.WorkshopAssetCount);
            this.Main.FillInfo("Main", info);
            this.Lod.FillInfo("Lod", info);
        }

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
            Log.InfoList info = new Log.InfoList(prefix);
            this.FillInfo(info);
            return info.ToString();
        }

        /// <summary>
        /// Asset value counters.
        /// </summary>
        public class Values
        {
            /// <summary>
            /// The textures area value.
            /// </summary>
            private long texturesAreaValue = 0;

            /// <summary>
            /// The textures height value
            /// </summary>
            private long texturesHeightValue = 0;

            /// <summary>
            /// The textures width value.
            /// </summary>
            private long texturesWidthValue = 0;

            /// <summary>
            /// The triangles count value.
            /// </summary>
            private long trianglesCountValue = 0;

            /// <summary>
            /// The triangles surface value.
            /// </summary>
            private double trianglesSurfaceValue = 0.0;

            /// <summary>
            /// Gets the combined weight.
            /// </summary>
            /// <value>
            /// The combined weight.
            /// </value>
            public long CombinedWeight => (this.TexturesAssetCount == 0) ? this.TrianglesWeight :
                                          (this.TrianglesAssetCount == 0) ? this.TexturesWeight :
                                          AssetValues.CalculateCombinedWeight(this.trianglesCountValue, this.texturesAreaValue, this.trianglesSurfaceValue);

            /// <summary>
            /// Gets a value indicating whether triangle or texture data for these <see cref="Values"/> are initialized.
            /// </summary>
            /// <value>
            ///   <c>true</c> if initialized; otherwise, <c>false</c>.
            /// </value>
            public bool InitializedAny => this.TrianglesAssetCount > 0 || this.TexturesAssetCount > 0;

            /// <summary>
            /// Gets a value indicating whether triangle an texture data for these <see cref="Values"/> are initialized.
            /// </summary>
            /// <value>
            ///   <c>true</c> if initialized; otherwise, <c>false</c>.
            /// </value>
            public bool InitializedBoth => this.TrianglesAssetCount > 0 && this.TexturesAssetCount > 0;

            /// <summary>
            /// Gets a value indicating whether texture data for these <see cref="Values"/> are initialized.
            /// </summary>
            /// <value>
            ///   <c>true</c> if initialized; otherwise, <c>false</c>.
            /// </value>
            public bool InitializedTextures => this.TexturesAssetCount > 0;

            /// <summary>
            /// Gets a value indicating whether triangle data for these <see cref="Values"/> are initialized.
            /// </summary>
            /// <value>
            ///   <c>true</c> if initialized; otherwise, <c>false</c>.
            /// </value>
            public bool InitializedTriangles => this.TrianglesAssetCount > 0;

            /// <summary>
            /// Gets the texture area.
            /// </summary>
            /// <value>
            /// The texture area.
            /// </value>
            public long TexturesArea => (this.TexturesAssetCount == 0) ? 0 : (long)(Math.Round((double)this.texturesAreaValue / (double)this.TexturesAssetCount));

            /// <summary>
            /// Gets the texture asset count.
            /// </summary>
            /// <value>
            /// The texture asset count.
            /// </value>
            public long TexturesAssetCount { get; private set; }

            /// <summary>
            /// Gets the textures dimensions.
            /// </summary>
            /// <value>
            /// The textures dimensions.
            /// </value>
            public string TexturesDimensions => (this.TexturesArea > 0) ? this.TexturesWidth.ToString() + '×' + this.TexturesHeight.ToString() : "";

            /// <summary>
            /// Gets the height of the texture.
            /// </summary>
            /// <value>
            /// The height of the texture.
            /// </value>
            public long TexturesHeight => (this.TexturesAssetCount == 0) ? 0 : (long)(Math.Round((double)this.texturesHeightValue / (double)this.TexturesAssetCount));

            /// <summary>
            /// Gets the texture weight.
            /// </summary>
            /// <value>
            /// The texture weight.
            /// </value>
            public long TexturesWeight => (this.TexturesAssetCount == 0) ? 0 : AssetValues.CalculateTextureWeight(this.texturesAreaValue, this.trianglesSurfaceValue);

            /// <summary>
            /// Gets the width of the texture.
            /// </summary>
            /// <value>
            /// The width of the texture.
            /// </value>
            public long TexturesWidth => (this.TexturesAssetCount == 0) ? 0 : (long)(Math.Round((double)this.texturesWidthValue / (double)this.TexturesAssetCount));

            /// <summary>
            /// Gets the total asset count.
            /// </summary>
            /// <value>
            /// The total asset count.
            /// </value>
            public long TotalAssetCount { get; private set; }

            /// <summary>
            /// Gets the triangle asset count.
            /// </summary>
            /// <value>
            /// The triangle asset count.
            /// </value>
            public long TrianglesAssetCount { get; private set; }

            /// <summary>
            /// Gets the triangle count.
            /// </summary>
            /// <value>
            /// The triangle count.
            /// </value>
            public long TrianglesCount => (this.TrianglesAssetCount == 0) ? 0 : (long)(Math.Round((double)this.trianglesCountValue / (double)this.TrianglesAssetCount));

            /// <summary>
            /// Gets the triangle surface.
            /// </summary>
            /// <value>
            /// The triangle surface.
            /// </value>
            public double TrianglesSurface => (this.TrianglesAssetCount == 0) ? 0 : (long)(Math.Round(this.trianglesSurfaceValue / (double)this.TrianglesAssetCount));

            /// <summary>
            /// Gets the triangles surface rounded.
            /// </summary>
            /// <value>
            /// The triangles surface rounded.
            /// </value>
            public long TrianglesSurfaceRounded => (int)Math.Round(this.TrianglesSurface);

            /// <summary>
            /// Gets the triangle weight.
            /// </summary>
            /// <value>
            /// The triangle weight.
            /// </value>
            public long TrianglesWeight => (this.TrianglesAssetCount == 0) ? 0 : AssetValues.CalculateTrianglesWeight(this.trianglesCountValue, this.trianglesSurfaceValue);

            /// <summary>
            /// Adds the specified triangles size.
            /// </summary>
            /// <param name="values">The values.</param>
            public void Add(AssetValues values)
            {
                this.TotalAssetCount++;

                if (values.InitializedTriangles && values.TrianglesSurface > 0.0)
                {
                    this.TrianglesAssetCount++;
                    this.trianglesCountValue += values.TrianglesCount;
                    this.trianglesSurfaceValue += values.TrianglesSurface;
                }

                if (values.InitializedTexture && values.TextureArea > 0)
                {
                    this.TexturesAssetCount++;
                    this.texturesWidthValue += values.TextureWidth;
                    this.texturesHeightValue += values.TextureHeight;
                    this.texturesAreaValue += values.TextureArea;
                }
            }

            /// <summary>
            /// Fills the information.
            /// </summary>
            /// <param name="prefix">The prefix.</param>
            /// <param name="info">The information.</param>
            public void FillInfo(string prefix, Log.InfoList info)
            {
                info.Add(prefix, this.TotalAssetCount, this.CombinedWeight,
                         "Triangles", this.TrianglesAssetCount, this.TrianglesCount, this.TrianglesSurface, this.TrianglesWeight, this.trianglesCountValue, this.trianglesSurfaceValue,
                         "Textures", this.TexturesAssetCount, this.TexturesDimensions, this.TexturesArea, this.TexturesWeight, this.texturesWidthValue, this.texturesHeightValue, this.texturesAreaValue);
            }
        }
    }
}