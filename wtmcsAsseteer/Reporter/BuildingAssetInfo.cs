using System.Collections.Generic;
using UnityEngine;

namespace WhatThe.Mods.CitiesSkylines.Asseteer.Reporter
{
    /// <summary>
    /// Holds information about a building asset.
    /// </summary>
    /// <seealso cref="WhatThe.Mods.CitiesSkylines.Asseteer.Reporter.AssetInfo" />
    internal class BuildingAssetInfo : AssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingAssetInfo"/> class.
        /// </summary>
        public BuildingAssetInfo() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingAssetInfo"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        public BuildingAssetInfo(PrefabInfo prefab) : base(prefab)
        { }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public static IEnumerable<AssetInfo> Collection => CollectPrefabs<BuildingInfo, BuildingAssetInfo>();

        /// <summary>
        /// Gets the prefab count.
        /// </summary>
        /// <value>
        /// The prefab count.
        /// </value>
        public static int PrefabCount => PrefabHelper.Count<BuildingInfo>();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override string Type => "Building";

        /// <summary>
        /// Gets the lod material.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod material.
        /// </returns>
        protected override Material GetLodMaterial(PrefabInfo prefab)
        {
            return ((BuildingInfo)prefab).m_lodMaterial;
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
            return ((BuildingInfo)prefab).m_lodMesh;
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
            return ((BuildingInfo)prefab).m_material;
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
            return ((BuildingInfo)prefab).m_mesh;
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected override void Initialize(PrefabInfo prefab)
        {
            base.Initialize(prefab);

            if (!this.Initialized)
            {
                return;
            }

            BuildingInfo building = (BuildingInfo)prefab;

            if (building.m_props != null)
            {
                for (int i = 0; i < building.m_props.Length; i++)
                {
                    if ((UnityEngine.Object)building.m_props[i].m_prop != (UnityEngine.Object)null)
                    {
                        bool propIs = building.m_props[i].m_prop.m_prefabInitialized ||
                                      PrefabCollection<PropInfo>.FindLoaded(building.m_props[i].m_prop.gameObject.name) != null;

                        this.ReferencedProps.Add(new Reference(Reference.ReferenceTypes.Prop, building.m_props[i].m_prop.gameObject.name, propIs));
                    }

                    if ((UnityEngine.Object)building.m_props[i].m_tree != (UnityEngine.Object)null)
                    {
                        bool treeIs = building.m_props[i].m_tree.m_prefabInitialized ||
                                      PrefabCollection<TreeInfo>.FindLoaded(building.m_props[i].m_tree.gameObject.name) != null;

                        this.ReferencedTrees.Add(new Reference(Reference.ReferenceTypes.Tree, building.m_props[i].m_tree.gameObject.name, treeIs));
                    }
                }
            }
        }
    }
}