using System;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds asset numbers.
    /// </summary>
    internal struct AssetValues
    {
        /// <summary>
        /// The texture multiplier.
        /// </summary>
        private const float textureMultiplier = 1.0f;

        /// <summary>
        /// The triangle multiplier.
        /// </summary>
        private const float trianglesMultiplier = 1000.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetValues"/> struct.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="texture">The texture.</param>
        public AssetValues(Mesh mesh, Texture texture)
        {
            if (mesh == null || !mesh.isReadable || mesh.triangles == null)
            {
                this.InitializedTriangles = false;

                this.TrianglesSize = new Vector3();
                this.TrianglesCount = 0;
                this.TrianglesSurface = 0;
            }
            else
            {
                this.InitializedTriangles = true;

                this.TrianglesSize = mesh.bounds.size;
                this.TrianglesSurface = 2 * mesh.bounds.size.x * mesh.bounds.size.y + 2 * mesh.bounds.size.y * mesh.bounds.size.z + 2 * mesh.bounds.size.z * mesh.bounds.size.x;
                this.TrianglesCount = mesh.triangles.Length / 3;
            }

            if ((UnityEngine.Object)texture == (UnityEngine.Object)null)
            {
                this.InitializedTexture = false;

                this.TextureWidth = 0;
                this.TextureHeight = 0;
                this.TextureArea = 0;
            }
            else
            {
                this.InitializedTexture = true;

                this.TextureWidth = texture.width;
                this.TextureHeight = texture.height;
                this.TextureArea = texture.width * texture.height;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetValues"/> struct.
        /// </summary>
        /// <param name="main">The main.</param>
        /// <param name="lod">The lod.</param>
        public AssetValues(AssetValues main, AssetValues lod)
        {
            if (lod.InitializedTriangles)
            {
                this.InitializedTriangles = true;
                this.TrianglesSize = lod.TrianglesSize;
                this.TrianglesCount = lod.TrianglesCount;
                this.TrianglesSurface = lod.TrianglesSurface;
            }
            else if (main.InitializedTriangles)
            {
                this.InitializedTriangles = true;
                this.TrianglesSize = main.TrianglesSize;
                this.TrianglesCount = main.TrianglesCount;
                this.TrianglesSurface = main.TrianglesSurface;
            }
            else
            {
                this.InitializedTriangles = false;
                this.TrianglesSize = new Vector3();
                this.TrianglesCount = 0;
                this.TrianglesSurface = 0.0f;
            }

            if (lod.InitializedTexture)
            {
                this.InitializedTexture = true;
                this.TextureWidth = lod.TextureWidth;
                this.TextureHeight = lod.TextureHeight;
                this.TextureArea = lod.TextureArea;
            }
            else if (main.InitializedTexture)
            {
                this.InitializedTexture = true;
                this.TextureWidth = main.TextureWidth;
                this.TextureHeight = main.TextureHeight;
                this.TextureArea = main.TextureArea;
            }
            else
            {
                this.InitializedTexture = false;
                this.TextureWidth = 0;
                this.TextureHeight = 0;
                this.TextureArea = 0;
            }
        }

        /// <summary>
        /// Gets the combined weight.
        /// </summary>
        /// <value>
        /// The combined weight.
        /// </value>
        public int CombinedWeight => this.TrianglesWeight + this.TextureWeight;

        /// <summary>
        /// Gets a value indicating whether triangle or texture data for these <see cref="AssetValues"/> are initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool InitializedAny => this.InitializedTriangles || this.InitializedTexture;

        /// <summary>
        /// Gets a value indicating whether triangle and texture data for these <see cref="AssetValues"/> are initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool InitializedBoth => this.InitializedTriangles && this.InitializedTexture;

        /// <summary>
        /// Gets a value indicating whether texture data for these <see cref="AssetValues"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool InitializedTexture { get; private set; }

        /// <summary>
        /// Gets a value indicating whether triangle data for these <see cref="AssetValues"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool InitializedTriangles { get; private set; }

        /// <summary>
        /// Gets the area.
        /// </summary>
        /// <value>
        /// The area.
        /// </value>
        public int TextureArea { get; private set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public string TextureDimensions => (this.TextureArea == 0) ? "-" : this.TextureWidth.ToString() + "x" + this.TextureHeight.ToString();

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int TextureHeight { get; private set; }

        /// <summary>
        /// Gets the texture weight.
        /// </summary>
        /// <value>
        /// The texture weight.
        /// </value>
        public int TextureWeight => (this.InitializedTexture && this.InitializedTriangles) ? CalculateTextureWeight(this.TextureArea, this.TrianglesSurface) : 0;

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int TextureWidth { get; private set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int TrianglesCount { get; private set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public string TrianglesDimensions => this.InitializedTriangles ? this.TrianglesSize.x.ToString("0.#####") + '×' + this.TrianglesSize.y.ToString("0.#####") + '×' + this.TrianglesSize.z.ToString("0.#####") : "";

        /// <summary>
        /// Gets the size of the triangles.
        /// </summary>
        /// <value>
        /// The size of the triangles.
        /// </value>
        public Vector3 TrianglesSize { get; private set; }

        /// <summary>
        /// Gets the triangles box surface.
        /// </summary>
        /// <value>
        /// The triangles box surface.
        /// </value>
        public float TrianglesSurface { get; private set; }

        /// <summary>
        /// Gets the surface.
        /// </summary>
        /// <value>
        /// The surface.
        /// </value>
        public int TrianglesSurfaceRounded => (int)Math.Round(this.TrianglesSurface);

        /// <summary>
        /// Gets the triangle weight.
        /// </summary>
        /// <value>
        /// The triangle weight.
        /// </value>
        public int TrianglesWeight => this.InitializedTriangles ? CalculateTrianglesWeight(this.TrianglesCount, this.TrianglesSurface) : 0;

        /// <summary>
        /// Calculates the combined weight.
        /// </summary>
        /// <param name="trianglesCount">The triangles count.</param>
        /// <param name="textureArea">The texture area.</param>
        /// <param name="trianglesSurface">The triangles surface.</param>
        /// <returns>
        /// The weight.
        /// </returns>
        public static int CalculateCombinedWeight(long trianglesCount, long textureArea, double trianglesSurface)
        {
            return CalculateWeight(trianglesCount, textureArea, trianglesSurface, trianglesMultiplier, textureMultiplier);
        }

        /// <summary>
        /// Calculates the triangles weight.
        /// </summary>
        /// <param name="textureArea">The texture area.</param>
        /// <param name="trianglesSurface">The triangles surface.</param>
        /// <returns>
        /// The weight.
        /// </returns>
        public static int CalculateTextureWeight(long textureArea, double trianglesSurface)
        {
            return CalculateWeight(textureArea, trianglesSurface, textureMultiplier);
        }

        /// <summary>
        /// Calculates the triangles weight.
        /// </summary>
        /// <param name="trianglesCount">The triangles count.</param>
        /// <param name="trianglesSurface">The triangles surface.</param>
        /// <returns>
        /// The weight.
        /// </returns>
        public static int CalculateTrianglesWeight(long trianglesCount, double trianglesSurface)
        {
            return CalculateWeight(trianglesCount, trianglesSurface, trianglesMultiplier);
        }

        /// <summary>
        /// Adds the specified triangles.
        /// </summary>
        /// <param name="assetValues">The asset values.</param>
        public void Add(AssetValues assetValues)
        {
            if (assetValues.InitializedTriangles)
            {
                this.TrianglesCount += assetValues.TrianglesCount;
                this.TrianglesSurface += assetValues.TrianglesSurface;
            }

            if (assetValues.InitializedTexture)
            {
                this.TextureWidth += assetValues.TextureWidth;
                this.TextureHeight += assetValues.TextureHeight;
                this.TextureArea += assetValues.TextureArea;
            }
        }

        /// <summary>
        /// Calculates the weight.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="surface">The surface.</param>
        /// <param name="multiplier1">The first multiplier.</param>
        /// <param name="multiplier2">The second multiplier.</param>
        /// <returns>
        /// The weight.
        /// </returns>
        private static int CalculateWeight(long value1, long value2, double surface, double multiplier1, double multiplier2)
        {
            double surface1 = (surface * multiplier1 < 1.0) ? 1.0 : surface;
            double surface2 = (surface * multiplier2 < 1.0) ? 1.0 : surface;

            return (int)Math.Min((((double)Math.Max(value1, 0) / surface1) * Math.Abs(multiplier1)) + (((double)Math.Max(value2, 0) / surface2) * Math.Abs(multiplier2)), int.MaxValue);
        }

        /// <summary>
        /// Calculates the weight.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="surface">The surface.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>
        /// The weight.
        /// </returns>
        private static int CalculateWeight(long value, double surface, double multiplier)
        {
            if (surface * multiplier < 1.0)
            {
                surface = 1.0;
            }

            return (int)Math.Min((((double)Math.Max(value, 0) / surface) * Math.Abs(multiplier)), int.MaxValue);
        }
    }
}