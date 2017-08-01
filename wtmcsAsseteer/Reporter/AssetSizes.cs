namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Asset data counter.
    /// </summary>
    internal class AssetSizes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSizes"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AssetSizes(string type)
        {
            this.Type = type;

            this.TotalAssetCount = 0;
            this.CustomAssetCount = 0;
            this.BuiltInAssetCount = 0;
            this.WorkshopAssetCount = 0;

            this.TriangleAssetCount = 0;
            this.TrianglesCount = 0;
            this.TrianglesVolume = 0;
            this.TrianglesWeight = 0;

            this.LodTriangleAssetCount = 0;
            this.LodTrianglesCount = 0;
            this.LodTrianglesVolume = 0;
            this.LodTrianglesWeight = 0;

            this.TextureAssetCount = 0;
            this.TexturesWidth = 0;
            this.TexturesHeight = 0;
            this.TexturesArea = 0;

            this.LodTextureAssetCount = 0;
            this.LodTexturesWidth = 0;
            this.LodTexturesHeight = 0;
            this.LodTexturesArea = 0;
        }

        /// <summary>
        /// Gets the asset count.
        /// </summary>
        /// <value>
        /// The asset count.
        /// </value>
        public long TotalAssetCount { get; private set; }

        /// <summary>
        /// The custom asset count.
        /// </summary>
        public long CustomAssetCount { get; private set; }

        /// <summary>
        /// The built in asset count.
        /// </summary>
        public long BuiltInAssetCount { get; private set; }

        /// <summary>
        /// The workshop asset count
        /// </summary>
        public long WorkshopAssetCount { get; private set; }

        /// <summary>
        /// Gets the lod texture area.
        /// </summary>
        /// <value>
        /// The lod texture area.
        /// </value>
        public long LodTexturesArea { get; private set; }

        /// <summary>
        /// Gets the lod texture asset count.
        /// </summary>
        /// <value>
        /// The lod texture asset count.
        /// </value>
        public long LodTextureAssetCount { get; private set; }

        /// <summary>
        /// Gets the height of the lod texture.
        /// </summary>
        /// <value>
        /// The height of the lod texture.
        /// </value>
        public long LodTexturesHeight { get; private set; }

        /// <summary>
        /// Gets the width of the lod texture.
        /// </summary>
        /// <value>
        /// The width of the lod texture.
        /// </value>
        public long LodTexturesWidth { get; private set; }

        /// <summary>
        /// Gets the lod triangle asset count.
        /// </summary>
        /// <value>
        /// The lod triangle asset count.
        /// </value>
        public long LodTriangleAssetCount { get; private set; }

        /// <summary>
        /// Gets the lod triangle count.
        /// </summary>
        /// <value>
        /// The lod triangle count.
        /// </value>
        public long LodTrianglesCount { get; private set; }

        /// <summary>
        /// Gets the lod triangle weight.
        /// </summary>
        /// <value>
        /// The lod triangle weight.
        /// </value>
        public long LodTrianglesWeight { get; private set; }

        /// <summary>
        /// Gets the lod triangle volume.
        /// </summary>
        /// <value>
        /// The lod triangle volume.
        /// </value>
        public long LodTrianglesVolume { get; private set; }

        /// <summary>
        /// Gets the texture area.
        /// </summary>
        /// <value>
        /// The texture area.
        /// </value>
        public long TexturesArea { get; private set; }

        /// <summary>
        /// Gets the texture asset count.
        /// </summary>
        /// <value>
        /// The texture asset count.
        /// </value>
        public long TextureAssetCount { get; private set; }

        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        /// <value>
        /// The height of the texture.
        /// </value>
        public long TexturesHeight { get; private set; }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        /// <value>
        /// The width of the texture.
        /// </value>
        public long TexturesWidth { get; private set; }

        /// <summary>
        /// Gets the triangle asset count.
        /// </summary>
        /// <value>
        /// The triangle asset count.
        /// </value>
        public long TriangleAssetCount { get; private set; }

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        /// <value>
        /// The triangle count.
        /// </value>
        public long TrianglesCount { get; private set; }

        /// <summary>
        /// Gets the triangle weight.
        /// </summary>
        /// <value>
        /// The triangle weight.
        /// </value>
        public long TrianglesWeight { get; private set; }

        /// <summary>
        /// Gets the triangle volume.
        /// </summary>
        /// <value>
        /// The triangle volume.
        /// </value>
        public long TrianglesVolume { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the textures dimensions.
        /// </summary>
        /// <value>
        /// The textures dimensions.
        /// </value>
        public string TexturesDimensions => (this.TexturesArea > 0) ? this.TexturesWidth.ToString() + 'x' + this.TexturesHeight.ToString() : "-";

        /// <summary>
        /// Gets the lod textures dimensions.
        /// </summary>
        /// <value>
        /// The lod textures dimensions.
        /// </value>
        public string LodTexturesDimensions => (this.LodTexturesArea > 0) ? this.LodTexturesWidth.ToString() + 'x' + this.LodTexturesHeight.ToString() : "-";

        /// <summary>
        /// The source type for which to count.
        /// </summary>
        private ObjectInfo.SourceTypes countForSourceType = ObjectInfo.SourceTypes.BuiltIn;

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
                if (asset.TrianglesSize.Volume > 0)
                {
                    this.Add(asset.TrianglesSize);
                }

                if (asset.LodTrianglesSize.Volume > 0)
                {
                    this.AddLod(asset.LodTrianglesSize);
                }

                if (asset.TextureSize.Area > 0)
                {
                    this.Add(asset.TextureSize);
                }

                if (asset.LodTextureSize.Area > 0)
                {
                    this.AddLod(asset.LodTextureSize);
                }
            }
        }

        /// <summary>
        /// Calculates the means.
        /// </summary>
        public void CalculateMeans()
        {
            if (this.TriangleAssetCount > 0)
            {
                this.TrianglesCount /= this.TriangleAssetCount;
                this.TrianglesVolume /= this.TriangleAssetCount;
                this.TrianglesWeight /= this.TriangleAssetCount;
            }

            if (this.LodTriangleAssetCount > 0)
            {
                this.LodTrianglesCount /= this.LodTriangleAssetCount;
                this.LodTrianglesVolume /= this.LodTriangleAssetCount;
                this.LodTrianglesWeight /= this.LodTriangleAssetCount;
            }

            if (this.TextureAssetCount > 0)
            {
                this.TexturesWidth /= this.TextureAssetCount;
                this.TexturesHeight /= this.TextureAssetCount;
                this.TexturesArea /= this.TextureAssetCount;
            }

            if (this.LodTextureAssetCount > 0)
            {
                this.LodTexturesWidth /= this.LodTextureAssetCount;
                this.LodTexturesHeight /= this.LodTextureAssetCount;
                this.LodTexturesArea /= this.LodTextureAssetCount;
            }
        }

        /// <summary>
        /// Adds the specified triangles size.
        /// </summary>
        /// <param name="trianglesSize">Size of the triangles.</param>
        private void Add(Triangles trianglesSize)
        {
            this.TriangleAssetCount++;
            this.TrianglesCount += trianglesSize.Count;
            this.TrianglesVolume += trianglesSize.Volume;
            this.TrianglesWeight += trianglesSize.Weight;
        }

        /// <summary>
        /// Adds the specified texture size.
        /// </summary>
        /// <param name="textureSize">Size of the texture.</param>
        private void Add(Rectangle textureSize)
        {
            this.TextureAssetCount++;
            this.TexturesWidth += textureSize.Width;
            this.TexturesHeight += textureSize.Height;
            this.TexturesArea += textureSize.Area;
        }

        /// <summary>
        /// Adds the specified lod triangles size.
        /// </summary>
        /// <param name="trianglesSize">Size of the triangles.</param>
        private void AddLod(Triangles trianglesSize)
        {
            this.LodTriangleAssetCount++;
            this.LodTrianglesCount += trianglesSize.Count;
            this.LodTrianglesVolume += trianglesSize.Volume;
            this.LodTrianglesWeight += trianglesSize.Weight;
        }

        /// <summary>
        /// Adds the specified lod texture size.
        /// </summary>
        /// <param name="textureSize">Size of the texture.</param>
        private void AddLod(Rectangle textureSize)
        {
            this.LodTextureAssetCount++;
            this.LodTexturesWidth += textureSize.Width;
            this.LodTexturesHeight += textureSize.Height;
            this.LodTexturesArea += textureSize.Area;
        }
    }
}