using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a citizen asset.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class CitizenAssetInfo : AssetInfo
    {
        /// <summary>
        /// The renderer.
        /// </summary>
        private SkinnedMeshRenderer renderer = null;

        /// <summary>
        /// The render-initialized flag.
        /// </summary>
        private bool renderInitialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitizenAssetInfo"/> class.
        /// </summary>
        public CitizenAssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CitizenAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public CitizenAssetInfo(CitizenInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<CitizenInfo, CitizenAssetInfo>();

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => PrefabHelper.Count<CitizenInfo>();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AssetTypes AssetType => AssetInfo.AssetTypes.Citizen;

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            return GetMeshWithFallback(((CitizenInfo)prefab).m_lodObject, ((CitizenInfo)prefab).m_lodMesh);
        }

        /// <summary>
        /// Gets the lod texture.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod texture.
        /// </returns>
        protected override Texture GetLodTexture(PrefabInfo prefab)
        {
            return GetTextureWithFallback(((CitizenInfo)prefab).m_lodObject, ((CitizenInfo)prefab).m_lodMaterial);
        }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The mesh.
        /// </returns>
        protected override Mesh GetMainMesh(PrefabInfo prefab)
        {
            try
            {
                if (!this.InitializeRenderer((CitizenInfo)prefab))
                {
                    return null;
                }

                Mesh mesh = new Mesh();
                renderer.BakeMesh(mesh);

                return mesh;
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
        /// <returns>
        /// The texture.
        /// </returns>
        protected override Texture GetMainTexture(PrefabInfo prefab)
        {
            try
            {
                if (!this.InitializeRenderer((CitizenInfo)prefab))
                {
                    return null;
                }

                if ((UnityEngine.Object)renderer.sharedMaterial == (UnityEngine.Object)null)
                {
                    return null;
                }

                Material material = new Material(renderer.sharedMaterial);

                if ((UnityEngine.Object)material != (UnityEngine.Object)null &&
                    (UnityEngine.Object)material.mainTexture != (UnityEngine.Object)null)
                {
                    return material.mainTexture;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the renderer.
        /// </summary>
        /// <param name="citizenInfo">The citizen information.</param>
        protected bool InitializeRenderer(CitizenInfo citizenInfo)
        {
            if (this.renderInitialized)
            {
                return (UnityEngine.Object)this.renderer != (UnityEngine.Object)null;
            }

            this.renderInitialized = true;

            try
            {
                this.renderer = citizenInfo.m_skinRenderer;
                if ((UnityEngine.Object)this.renderer != (UnityEngine.Object)null)
                {
                    return true;
                }

                this.renderer = citizenInfo.gameObject.GetComponent<SkinnedMeshRenderer>();
                if ((UnityEngine.Object)this.renderer != (UnityEngine.Object)null)
                {
                    return true;
                }

                for (int i = 0; i < citizenInfo.gameObject.transform.childCount; ++i)
                {
                    this.renderer = citizenInfo.gameObject.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>();
                    if ((UnityEngine.Object)this.renderer != (UnityEngine.Object)null)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                this.renderer = null;
                return false;
            }
        }
    }
}