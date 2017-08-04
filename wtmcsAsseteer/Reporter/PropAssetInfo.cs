using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a citizen asset.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class PropAssetInfo : AssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropAssetInfo"/> class.
        /// </summary>
        public PropAssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public PropAssetInfo(PropInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<PropInfo, PropAssetInfo>(p => IncludePrefab(p));

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => PrefabHelper.Count<PropInfo>();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AssetTypes AssetType => AssetInfo.AssetTypes.Prop;

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            return GetMeshWithFallback(((PropInfo)prefab).m_lodObject, ((PropInfo)prefab).m_lodMesh);
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
            return GetTextureWithFallback(((PropInfo)prefab).m_lodObject, ((PropInfo)prefab).m_lodMaterial);
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
            return GetMeshWithFallback(prefab, ((PropInfo)prefab).m_mesh);
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
            return GetTextureWithFallback(prefab, ((PropInfo)prefab).m_material);
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected override bool InitializeData(PrefabInfo prefab)
        {
            bool success = base.InitializeData(prefab);

            if (((PropInfo)prefab).m_variations != null)
            {
                for (int i = 0; i < ((PropInfo)prefab).m_variations.Length; i++)
                {
                    if ((UnityEngine.Object)((PropInfo)prefab).m_variations[i].m_prop != (UnityEngine.Object)null)
                    {
                        this.AddReferencedAsset<PropInfo>(Reference.ReferenceTypes.Variation, ((PropInfo)prefab).m_variations[i].m_prop);
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Check if the prefab should be included.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>True to include.</returns>
        private static bool IncludePrefab(PrefabInfo prefab)
        {
            return ((PropInfo)prefab).m_hasRenderer && !((PropInfo)prefab).m_isMarker;
        }
    }
}