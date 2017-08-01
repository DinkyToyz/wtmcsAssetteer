using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds size of rectangular thing.
    /// </summary>
    internal struct Rectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="area">The area.</param>
        /// <param name="isLod">if set to <c>true</c> [is lod].</param>
        public Rectangle(int width, int height, int area, bool isLod)
        {
            this.Initialized = true;

            this.Width = width;
            this.Height = height;
            this.Area = area;
            this.Lod = isLod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="isLod">if set to <c>true</c> [is lod].</param>
        public Rectangle(int width, int height, bool isLod)
        {
            this.Initialized = true;

            this.Width = width;
            this.Height = height;
            this.Area = width * height;
            this.Lod = isLod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="material">The material.</param>
        /// <param name="isLod">if set to <c>true</c> this is for lod.</param>
        public Rectangle(Material material, bool isLod)
        {
            this.Lod = isLod;

            if (material == null || material.mainTexture == null)
            {
                this.Initialized = false;

                this.Width = 0;
                this.Height = 0;
                this.Area = 0;
            }
            else
            {
                this.Initialized = true;

                this.Width = material.mainTexture.width;
                this.Height = material.mainTexture.height;
                this.Area = material.mainTexture.width * material.mainTexture.height;
            }
        }

        /// <summary>
        /// Gets the area.
        /// </summary>
        /// <value>
        /// The area.
        /// </value>
        public int Area { get; private set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public string Dimensions => (this.Area == 0) ? "-" : this.Width.ToString() + "x" + this.Height.ToString();

        /// <summary>
        /// Gets a value indicating whether this <see cref="Triangles"/> is heavy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if heavy; otherwise, <c>false</c>.
        /// </value>
        public bool Heavy => this.Area > 1024 * 1024;

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Rectangle"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is a lod.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a lod; otherwise, <c>false</c>.
        /// </value>
        public bool Lod { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [very heavy].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [very heavy]; otherwise, <c>false</c>.
        /// </value>
        public bool VeryHeavy => this.Area > 2048 * 2048;

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; private set; }
    }
}