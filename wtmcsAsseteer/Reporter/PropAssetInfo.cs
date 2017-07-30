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
        public PropAssetInfo(PrefabInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<PropInfo, PropAssetInfo>();

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => GetPrefabCount<PropInfo>();

        /// <summary>
        /// Gets the lod material.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod material.
        /// </returns>
        protected override Material GetLodMaterial(PrefabInfo prefab)
        {
            return ((PropInfo)prefab).m_lodMaterial;
        }

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            return ((PropInfo)prefab).m_lodMesh;
        }

        /// <summary>
        /// Gets the material.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The material.
        /// </returns>
        protected override Material GetMaterial(PrefabInfo prefab)
        {
            return ((PropInfo)prefab).m_material;
        }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The mesh.
        /// </returns>
        protected override Mesh GetMesh(PrefabInfo prefab)
        {
            return ((PropInfo)prefab).m_mesh;
        }
    }
}