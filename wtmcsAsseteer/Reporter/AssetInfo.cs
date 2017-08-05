using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Hold info about an asset.
    /// </summary>
    internal abstract class AssetInfo : ObjectInfo
    {
        /// <summary>
        /// The referenced assets.
        /// </summary>
        protected Dictionary<string, Reference> ReferencedAssets = new Dictionary<string, Reference>();

        /// <summary>
        /// The referencing assets.
        /// </summary>
        protected Dictionary<string, Reference> ReferencingAssets = new Dictionary<string, Reference>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfo"/> class.
        /// </summary>
        public AssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public AssetInfo(PrefabInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Asset types.
        /// </summary>
        public enum AssetTypes
        {
            /// <summary>
            /// Unknown type.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// A growable building.
            /// </summary>
            Growable,

            /// <summary>
            /// A non-growable building.
            /// </summary>
            Building,

            /// <summary>
            /// A vehicle.
            /// </summary>
            Vehicle,

            /// <summary>
            /// A prop.
            /// </summary>
            Prop,

            /// <summary>
            /// A tree.
            /// </summary>
            Tree,

            /// <summary>
            /// A citizen.
            /// </summary>
            Citizen,

            /// <summary>
            /// All types.
            /// </summary>
            All
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract AssetTypes AssetType { get; }

        /// <summary>
        /// Gets the dependent count.
        /// </summary>
        /// <value>
        /// The dependent count.
        /// </value>
        public int DependentCount => this.ReferencingAssets.Count;

        /// <summary>
        /// Gets the dependents.
        /// </summary>
        /// <value>
        /// The dependents.
        /// </value>
        public ReadOnlyCollection<Reference> Dependents => this.ReferencingAssets.Values.ToList().AsReadOnly();

        /// <summary>
        /// Gets the lod asset values with fallback to main.
        /// </summary>
        /// <value>
        /// The asset values.
        /// </value>
        public AssetValues FallbackLod { get; private set; }

        /// <summary>
        /// Gets the lod asset values.
        /// </summary>
        /// <value>
        /// The asset values.
        /// </value>
        public AssetValues Lod { get; private set; }

        /// <summary>
        /// Gets the main asset values.
        /// </summary>
        /// <value>
        /// The asset values.
        /// </value>
        public AssetValues Main { get; private set; }

        /// <summary>
        /// Gets the props.
        /// </summary>
        /// <value>
        /// The props.
        /// </value>
        public IEnumerable<Reference> Props => this.ReferencedAssets.Values.Where(r => r.ReferenceType == Reference.ReferenceTypes.Prop);

        /// <summary>
        /// Gets the reference count.
        /// </summary>
        /// <value>
        /// The reference count.
        /// </value>
        public int ReferenceCount => this.ReferencedAssets.Count;

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <value>
        /// The references.
        /// </value>
        public IList<Reference> References => this.ReferencedAssets.Values.ToList().AsReadOnly();

        /// <summary>
        /// Gets the trees.
        /// </summary>
        /// <value>
        /// The trees.
        /// </value>
        public IEnumerable<Reference> Trees => this.ReferencedAssets.Values.Where(r => r.ReferenceType == Reference.ReferenceTypes.Tree);

        /// <summary>
        /// Adds the specified asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Add(AssetInfo asset)
        {
            this.Main.Add(asset.Main);
            this.Lod.Add(asset.Lod);
        }

        /// <summary>
        /// Adds the dependencies.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        public void AddDependencies(IEnumerable<Reference> dependencies)
        {
            foreach (Reference reference in dependencies)
            {
                if (!this.ReferencingAssets.ContainsKey(reference.PrefabName))
                {
                    this.ReferencingAssets[reference.PrefabName] = reference;
                }
            }
        }

        public override void FillInfo(Log.InfoList info)
        {
            info.Add("AssetType", this.AssetType);
            base.FillInfo(info);

            info.Add("Main", this.Main.CombinedWeight, "Triangles", this.Main.TrianglesCount, this.Main.TrianglesDimensions, this.Main.TrianglesSurface, this.Main.TrianglesWeight, "Texture", this.Main.TextureDimensions, this.Main.TextureArea, this.Main.TextureWeight);
            info.Add("Lod", this.Lod.CombinedWeight, "Triangles", this.Lod.TrianglesCount, this.Lod.TrianglesDimensions, this.Lod.TrianglesSurface, this.Lod.TrianglesWeight, "Texture", this.Lod.TextureDimensions, this.Lod.TextureArea, this.Lod.TextureWeight);
            info.Add("Fallback", this.FallbackLod.CombinedWeight, "Triangles", this.FallbackLod.TrianglesCount, this.FallbackLod.TrianglesDimensions, this.FallbackLod.TrianglesSurface, this.FallbackLod.TrianglesWeight, "Texture", this.FallbackLod.TextureDimensions, this.FallbackLod.TextureArea, this.FallbackLod.TextureWeight);

            info.Add("Ref", this.ReferenceCount, this.DependentCount);
        }

        /// <summary>
        /// Checks the lod mesh.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <returns>True if mesh has triangles.</returns>
        protected static bool CheckLodMesh(Mesh mesh)
        {
            if ((UnityEngine.Object)mesh == (UnityEngine.Object)null)
            {
                return false;
            }

            return (mesh.triangles != null && mesh.triangles.Length > 0);
        }

        /// <summary>
        /// Checks the lod mesh data.
        /// </summary>
        /// <param name="meshData">The mesh data.</param>
        /// <returns>True if mesh has triangles.</returns>
        protected static bool CheckLodMeshData(RenderGroup.MeshData meshData)
        {
            if (meshData == null)
            {
                return false;
            }

            return (meshData.m_triangles != null && meshData.m_triangles.Length > 0);
        }

        /// <summary>
        /// Collects assets of the specified prefab type.
        /// </summary>
        /// <typeparam name="TPrefab">The type of the prefab.</typeparam>
        /// <typeparam name="TAsset">The type of the asset.</typeparam>
        /// <returns>A sequence of assets of the specified type.</returns>
        protected static IEnumerable<AssetInfo> CollectPrefabs<TPrefab, TAsset>(Func<PrefabInfo, bool> predicate) where TPrefab : PrefabInfo where TAsset : AssetInfo, new()
        {
            return PrefabHelper.Collect<TPrefab>()
                    .Where(p => predicate(p))
                    .Select(p => { AssetInfo a = new TAsset(); a.Initialize(p); return a; })
                    .Where(a => a.Initialized);
        }

        /// <summary>
        /// Collects assets of the specified prefab type.
        /// </summary>
        /// <typeparam name="TPrefab">The type of the prefab.</typeparam>
        /// <typeparam name="TAsset">The type of the asset.</typeparam>
        /// <returns>A sequence of assets of the specified type.</returns>
        protected static IEnumerable<AssetInfo> CollectPrefabs<TPrefab, TAsset>() where TPrefab : PrefabInfo where TAsset : AssetInfo, new()
        {
            return PrefabHelper.Collect<TPrefab>()
                    .Select(p => { AssetInfo a = new TAsset(); a.Initialize(p); return a; })
                    .Where(a => a.Initialized);
        }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="mesh">The mesh.</param>
        /// <returns>
        /// The mesh.
        /// </returns>
        protected static Mesh GetMeshWithFallback(PrefabInfo prefab, Mesh mesh)
        {
            if ((UnityEngine.Object)mesh != (UnityEngine.Object)null)
            {
                return mesh;
            }

            try
            {
                MeshFilter filter = prefab.GetComponent<MeshFilter>();
                if ((UnityEngine.Object)filter != (UnityEngine.Object)null &&
                    (UnityEngine.Object)filter.sharedMesh != (UnityEngine.Object)null)
                {
                    return filter.sharedMesh;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="mesh">The mesh.</param>
        /// <returns>
        /// The mesh.
        /// </returns>
        protected static Mesh GetMeshWithFallback(GameObject gameObject, Mesh mesh)
        {
            if ((UnityEngine.Object)mesh != (UnityEngine.Object)null)
            {
                return mesh;
            }

            try
            {
                MeshFilter filter = gameObject.GetComponent<MeshFilter>();
                if ((UnityEngine.Object)filter != (UnityEngine.Object)null &&
                    (UnityEngine.Object)filter.sharedMesh != (UnityEngine.Object)null)
                {
                    return filter.sharedMesh;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="material">The material.</param>
        /// <returns>
        /// The texture.
        /// </returns>
        protected static Texture GetTextureWithFallback(PrefabInfo prefab, Material material)
        {
            if ((UnityEngine.Object)material != (UnityEngine.Object)null &&
                (UnityEngine.Object)material.mainTexture != (UnityEngine.Object)null)
            {
                return material.mainTexture;
            }

            if ((UnityEngine.Object)prefab == (UnityEngine.Object)null)
            {
                return null;
            }

            try
            {
                Renderer renderer = prefab.GetComponent<Renderer>();
                if ((UnityEngine.Object)renderer != (UnityEngine.Object)null &&
                    (UnityEngine.Object)renderer.sharedMaterial != (UnityEngine.Object)null &&
                    (UnityEngine.Object)renderer.sharedMaterial.mainTexture != (UnityEngine.Object)null)
                {
                    return renderer.sharedMaterial.mainTexture;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="material">The material.</param>
        /// <returns>
        /// The texture.
        /// </returns>
        protected static Texture GetTextureWithFallback(GameObject gameObject, Material material)
        {
            if ((UnityEngine.Object)material != (UnityEngine.Object)null &&
                (UnityEngine.Object)material.mainTexture != (UnityEngine.Object)null)
            {
                return material.mainTexture;
            }

            if ((UnityEngine.Object)gameObject == (UnityEngine.Object)null)
            {
                return null;
            }

            try
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                if ((UnityEngine.Object)renderer != (UnityEngine.Object)null &&
                    (UnityEngine.Object)renderer.sharedMaterial != (UnityEngine.Object)null &&
                    (UnityEngine.Object)renderer.sharedMaterial.mainTexture != (UnityEngine.Object)null)
                {
                    return renderer.sharedMaterial.mainTexture;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Adds the referenced asset.
        /// </summary>
        /// <typeparam name="T">The prefab type.</typeparam>
        /// <param name="referenceType">Type of the reference.</param>
        /// <param name="prefab">The prefab.</param>
        protected void AddReferencedAsset<T>(Reference.ReferenceTypes referenceType, PrefabInfo prefab) where T : PrefabInfo
        {
            if (!this.ReferencedAssets.ContainsKey(prefab.gameObject.name))
            {
                bool exists = prefab.m_prefabInitialized ||
                              PrefabCollection<T>.FindLoaded(prefab.gameObject.name) != null;

                this.ReferencedAssets[prefab.gameObject.name] = new Reference(referenceType, prefab, exists);
            }
        }

        /// <summary>
        /// Initializes the current instance.
        /// </summary>
        protected override void Clear()
        {
            base.Clear();
            this.ReferencedAssets.Clear();
            this.ReferencingAssets.Clear();
        }

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The lod mesh.</returns>
        protected abstract Mesh GetLodMesh(PrefabInfo prefab);

        /// <summary>
        /// Gets the lod texture.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The lod texture.</returns>
        protected abstract Texture GetLodTexture(PrefabInfo prefab);

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The mesh.</returns>
        protected abstract Mesh GetMainMesh(PrefabInfo prefab);

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>The texture.</returns>
        protected abstract Texture GetMainTexture(PrefabInfo prefab);

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected override bool InitializeData(PrefabInfo prefab)
        {
            base.InitializeData(prefab);

            this.Main = new AssetValues(this.GetMainMesh(prefab), this.GetMainTexture(prefab));
            this.Lod = new AssetValues(this.GetLodMesh(prefab), this.GetLodTexture(prefab));
            this.FallbackLod = new AssetValues(this.Main, this.Lod);

            return this.Main.InitializedAny || this.Lod.InitializedAny;
        }

        /// <summary>
        /// Initializes the current instance for failed initialization.
        /// </summary>
        protected override void InitializeFailed(PrefabInfo prefab)
        {
            this.Main = new AssetValues();
            this.Lod = new AssetValues();
        }
    }
}