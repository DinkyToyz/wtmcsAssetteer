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
        /// The referenced props.
        /// </summary>
        protected List<Reference> ReferencedProps = new List<Reference>();

        /// <summary>
        /// The referenced trees.
        /// </summary>
        protected List<Reference> ReferencedTrees = new List<Reference>();

        /// <summary>
        /// The referencing assets.
        /// </summary>
        protected List<Reference> ReferencingAssets = new List<Reference>();

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
        /// Gets the dependencies.
        /// </summary>
        /// <value>
        /// The dependencies.
        /// </value>
        public ReadOnlyCollection<Reference> Dependencies => this.ReferencingAssets.AsReadOnly();

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
        /// Gets the missing props.
        /// </summary>
        /// <value>
        /// The missing props.
        /// </value>
        public IEnumerable<Reference> MissingProps => this.ReferencedProps.Where(p => !p.Exists);

        /// <summary>
        /// Gets the missing trees.
        /// </summary>
        /// <value>
        /// The missing trees.
        /// </value>
        public IEnumerable<Reference> MissingTrees => this.ReferencedTrees.Where(t => !t.Exists);

        /// <summary>
        /// Gets the prop count.
        /// </summary>
        /// <value>
        /// The prop count.
        /// </value>
        public int PropCount => (this.ReferencedProps == null) ? 0 : this.ReferencedProps.Count;

        /// <summary>
        /// Gets the props.
        /// </summary>
        /// <value>
        /// The props.
        /// </value>
        public ReadOnlyCollection<Reference> Props => this.ReferencedProps.AsReadOnly();

        /// <summary>
        /// Gets the reference count.
        /// </summary>
        /// <value>
        /// The reference count.
        /// </value>
        public int ReferenceCount => this.ReferencedProps.Count + this.ReferencedTrees.Count;

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <value>
        /// The references.
        /// </value>
        public IEnumerable<Reference> References => this.ReferencedProps.Union(this.ReferencedTrees);

        /// <summary>
        /// Gets the size of the texture.
        /// </summary>
        /// <value>
        /// The size of the texture.
        /// </value>
        public Rectangle TextureSize { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the tree count.
        /// </summary>
        /// <value>
        /// The tree count.
        /// </value>
        public int TreeCount => (this.ReferencedTrees == null) ? 0 : this.ReferencedTrees.Count;

        /// <summary>
        /// Gets the trees.
        /// </summary>
        /// <value>
        /// The trees.
        /// </value>
        public ReadOnlyCollection<Reference> Trees => this.ReferencedTrees.AsReadOnly();

        /// <summary>
        /// Gets the size of the triangles.
        /// </summary>
        /// <value>
        /// The size of the triangles.
        /// </value>
        public Triangles TrianglesSize { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract string Type { get; }

        /// <summary>
        /// Adds the dependencies.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        public void AddDependencies(IEnumerable<Reference> dependencies)
        {
            this.ReferencingAssets.AddRange(dependencies);
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
        protected override void Initialize(PrefabInfo prefab)
        {
            this.ReferencedProps.Clear();
            this.ReferencedTrees.Clear();

            base.Initialize(prefab);

            if (!this.Initialized)
            {
                this.Title = null;

                this.TextureSize = new Rectangle();
                this.LodTextureSize = new Rectangle();
                this.TrianglesSize = new Triangles();
                this.LodTrianglesSize = new Triangles();

                return;
            }

            this.Title = prefab.GetLocalizedTitle();

            this.TextureSize = new Rectangle(this.GetMaterial(prefab), false);
            this.LodTextureSize = new Rectangle(this.GetLodMaterial(prefab), true);
            this.TrianglesSize = new Triangles(this.GetMesh(prefab), false);
            this.LodTrianglesSize = new Triangles(this.GetLodMesh(prefab), true);

            this.Initialized = this.TextureSize.Initialized || this.LodTextureSize.Initialized || this.TrianglesSize.Initialized || this.LodTrianglesSize.Initialized;
        }
    }
}