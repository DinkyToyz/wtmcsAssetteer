using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Hold info about an asset.
    /// </summary>
    internal abstract class AssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfo"/> class.
        /// </summary>
        public AssetInfo()
        {
            this.Initialized = this.Initialize(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public AssetInfo(PrefabInfo prefab)
        {
            this.Initialized = this.Initialize(prefab);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="AssetInfo"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets the size of the lod texture.
        /// </summary>
        /// <value>
        /// The size of the lod texture.
        /// </value>
        public Rectangle LodTextureSize { get; private set; }

        /// <summary>
        /// Gets the size of the lod triangles.
        /// </summary>
        /// <value>
        /// The size of the lod triangles.
        /// </value>
        public Triangles LodTrianglesSize { get; private set; }

        /// <summary>
        /// Gets the name of the prefab.
        /// </summary>
        /// <value>
        /// The name of the prefab.
        /// </value>
        public string PrefabName { get; private set; }

        /// <summary>
        /// Gets the steam identifier.
        /// </summary>
        /// <value>
        /// The steam identifier.
        /// </value>
        public int SteamId { get; private set; }

        /// <summary>
        /// Gets the size of the texture.
        /// </summary>
        /// <value>
        /// The size of the texture.
        /// </value>
        public Rectangle TextureSize { get; private set; }

        /// <summary>
        /// Gets the size of the triangles.
        /// </summary>
        /// <value>
        /// The size of the triangles.
        /// </value>
        public Triangles TrianglesSize { get; private set; }

        /// <summary>
        /// Collects assets of the specified prefab type.
        /// </summary>
        /// <typeparam name="TPrefab">The type of the prefab.</typeparam>
        /// <typeparam name="TAsset">The type of the asset.</typeparam>
        /// <returns>A sequence of assets of the specified type.</returns>
        protected static IEnumerable<AssetInfo> CollectPrefabs<TPrefab, TAsset>() where TPrefab : PrefabInfo where TAsset : AssetInfo, new()
        {
            int count = PrefabCollection<TPrefab>.PrefabCount();

            for (uint i = 0; i < count; i++)
            {
                PrefabInfo prefab = PrefabCollection<BuildingInfo>.GetPrefab(i);
                if (prefab != null)
                {
                    AssetInfo assetInfo = new TAsset();
                    if (assetInfo.Initialize(prefab))
                    {
                        yield return assetInfo;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the prefab count for the prefab type.
        /// </summary>
        /// <typeparam name="TPrefab">The type of the prefab.</typeparam>
        /// <returns>The count of prefabs of the specified type.</returns>
        protected static int GetPrefabCount<TPrefab>() where TPrefab : PrefabInfo
        {
            return PrefabCollection<TPrefab>.PrefabCount();
        }

        /// <summary>
        /// Gets the lod material.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The lod material.</returns>
        protected abstract Material GetLodMaterial(PrefabInfo prefab);

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The lod mesh.</returns>
        protected abstract Mesh GetLodMesh(PrefabInfo prefab);

        /// <summary>
        /// Gets the material.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The material.</returns>
        protected abstract Material GetMaterial(PrefabInfo prefab);

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The mesh.</returns>
        protected abstract Mesh GetMesh(PrefabInfo prefab);

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected virtual bool Initialize(PrefabInfo prefab)
        {
            if (prefab == null)
            {
                this.PrefabName = null;
                this.SteamId = 0;

                this.TextureSize = new Rectangle();
                this.LodTextureSize = new Rectangle();
                this.TrianglesSize = new Triangles();
                this.LodTrianglesSize = new Triangles();

                return false;
            }
            else
            {
                if (prefab.name == null)
                {
                    this.PrefabName = prefab.GetType().ToString();
                    this.SteamId = 0;
                }
                else
                {
                    this.PrefabName = prefab.name;

                    int p = prefab.name.IndexOf('.');
                    int id;
                    this.SteamId = (p > 0 && Int32.TryParse(prefab.name.Substring(0, p), out id)) ? id : 0;
                }

                this.TextureSize = new Rectangle(this.GetMaterial(prefab));
                this.LodTextureSize = new Rectangle(this.GetLodMaterial(prefab));
                this.TrianglesSize = new Triangles(this.GetMesh(prefab));
                this.LodTrianglesSize = new Triangles(this.GetLodMesh(prefab));

                return this.TextureSize.Initialized || this.LodTextureSize.Initialized || this.TrianglesSize.Initialized || this.LodTrianglesSize.Initialized;
            }
        }

        /// <summary>
        /// Holds size of rectangular thing.
        /// </summary>
        public struct Rectangle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Rectangle"/> struct.
            /// </summary>
            /// <param name="material">The material.</param>
            public Rectangle(Material material)
            {
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
            /// Gets the width.
            /// </summary>
            /// <value>
            /// The width.
            /// </value>
            public int Width { get; private set; }

            /// <summary>
            /// Gets the dimensions.
            /// </summary>
            /// <value>
            /// The dimensions.
            /// </value>
            public string Dimensions => (this.Area == 0) ? "-" : this.Width.ToString() + "x" + this.Height.ToString();
        }

        /// <summary>
        /// Holds info about triangles.
        /// </summary>
        public struct Triangles
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Triangles"/> struct.
            /// </summary>
            /// <param name="mesh">The mesh.</param>
            public Triangles(Mesh mesh)
            {
                if (mesh == null || !mesh.isReadable || mesh.triangles == null)
                {
                    this.Initialized = false;

                    this.Count = 0;
                    this.Weight = float.NaN;
                }
                else
                {
                    this.Initialized = true;

                    this.Count = mesh.triangles.Length / 3;

                    if (this.Count == 0)
                    {
                        this.Weight = 0.0f;
                    }
                    else
                    {
                        float size = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;

                        if (size <= 0.0)
                        {
                            this.Weight = this.Count;
                        }
                        else
                        {
                            this.Weight = (float)this.Count / size;
                        }
                    }
                }
            }

            /// <summary>
            /// Gets the count.
            /// </summary>
            /// <value>
            /// The count.
            /// </value>
            public int Count { get; private set; }

            /// <summary>
            /// Gets a value indicating whether this <see cref="Triangles"/> is initialized.
            /// </summary>
            /// <value>
            ///   <c>true</c> if initialized; otherwise, <c>false</c>.
            /// </value>
            public bool Initialized { get; private set; }

            /// <summary>
            /// Gets the weight.
            /// </summary>
            /// <value>
            /// The weight.
            /// </value>
            public float Weight { get; private set; }
        }
    }
}