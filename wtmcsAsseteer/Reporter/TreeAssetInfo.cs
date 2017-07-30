using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a tree asset.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class TreeAssetInfo : AssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeAssetInfo"/> class.
        /// </summary>
        public TreeAssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public TreeAssetInfo(PrefabInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<TreeInfo, TreeAssetInfo>();

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => GetPrefabCount<TreeInfo>();

        /// <summary>
        /// Gets the lod material.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod material.
        /// </returns>
        protected override Material GetLodMaterial(PrefabInfo prefab)
        {
            return ((TreeInfo)prefab).m_lodMaterial;
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
            return ((TreeInfo)prefab).m_lodMesh16;
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
            return ((TreeInfo)prefab).m_material;
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
            return ((TreeInfo)prefab).m_mesh;
        }
    }
}