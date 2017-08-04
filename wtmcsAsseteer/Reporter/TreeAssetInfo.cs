using ColossalFramework;
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
        public TreeAssetInfo(TreeInfo prefab) : base(prefab)
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
        public static int PrefabCount => PrefabHelper.Count<TreeInfo>();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override AssetTypes AssetType => AssetInfo.AssetTypes.Tree;

        /// <summary>
        /// Gets the lod mesh.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns>
        /// The lod mesh.
        /// </returns>
        protected override Mesh GetLodMesh(PrefabInfo prefab)
        {
            if (CheckLodMesh(((TreeInfo)prefab).m_lodMesh16))
            {
                return ((TreeInfo)prefab).m_lodMesh16;
            }
            else if (CheckLodMesh(((TreeInfo)prefab).m_lodMesh8))
            {
                return ((TreeInfo)prefab).m_lodMesh8;
            }
            else if (CheckLodMesh(((TreeInfo)prefab).m_lodMesh4))
            {
                return ((TreeInfo)prefab).m_lodMesh4;
            }
            else if (CheckLodMesh(((TreeInfo)prefab).m_lodMesh1))
            {
                return ((TreeInfo)prefab).m_lodMesh1;
            }

            Mesh mesh = new Mesh();
            if (CheckLodMeshData(((TreeInfo)prefab).m_lodMeshData16))
            {
                ((TreeInfo)prefab).m_lodMeshData16.PopulateMesh(mesh);
                return mesh;
            }
            else if (CheckLodMeshData(((TreeInfo)prefab).m_lodMeshData8))
            {
                ((TreeInfo)prefab).m_lodMeshData8.PopulateMesh(mesh);
                return mesh;
            }
            else if (CheckLodMeshData(((TreeInfo)prefab).m_lodMeshData4))
            {
                ((TreeInfo)prefab).m_lodMeshData4.PopulateMesh(mesh);
                return mesh;
            }
            else if (CheckLodMeshData(((TreeInfo)prefab).m_lodMeshData1))
            {
                ((TreeInfo)prefab).m_lodMeshData1.PopulateMesh(mesh);
                return mesh;
            }

            try
            {
                RenderGroup.MeshData meshData = GenerateLodMeshData((TreeInfo)prefab);

                mesh = new Mesh();
                meshData.PopulateMesh(mesh);
                return mesh;
            }
            catch
            {
                return null;
            }
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
            if ((UnityEngine.Object)((TreeInfo)prefab).m_lodMaterial != (UnityEngine.Object)null &&
                (UnityEngine.Object)((TreeInfo)prefab).m_lodMaterial.mainTexture != (UnityEngine.Object)null)
            {
                return ((TreeInfo)prefab).m_lodMaterial.mainTexture;
            }

            try
            {
                return Singleton<TreeManager>.instance.m_renderDiffuseTexture;
            }
            catch
            {
                return null;
            }
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
            // todo: Built-in mesh?
            Mesh mesh = GetMeshWithFallback(prefab, ((TreeInfo)prefab).m_mesh);

            if ((UnityEngine.Object)mesh != (UnityEngine.Object)null)
            {
                return mesh;
            }

            if ((UnityEngine.Object)prefab != (UnityEngine.Object)null &&
                (UnityEngine.Object)((TreeInfo)prefab).m_generatedInfo != null &&
                (UnityEngine.Object)((TreeInfo)prefab).m_generatedInfo.m_treeInfo != (UnityEngine.Object)null &&
                (UnityEngine.Object)((TreeInfo)prefab).m_generatedInfo.m_treeInfo.m_mesh != (UnityEngine.Object)null)
            {
                return ((TreeInfo)prefab).m_generatedInfo.m_treeInfo.m_mesh;
            }

            return null;
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
            return GetTextureWithFallback(prefab, ((TreeInfo)prefab).m_material);
        }

        /// <summary>
        /// Initializes the current instance with values from specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        protected override bool InitializeData(PrefabInfo prefab)
        {
            bool success = base.InitializeData(prefab);

            if (((TreeInfo)prefab).m_variations != null)
            {
                for (int i = 0; i < ((TreeInfo)prefab).m_variations.Length; i++)
                {
                    if (((TreeInfo)prefab).m_variations[i] != null &&
                        (UnityEngine.Object)((TreeInfo)prefab).m_variations[i].m_tree != (UnityEngine.Object)null)
                    {
                        this.AddReferencedAsset<PropInfo>(Reference.ReferenceTypes.Variation, ((TreeInfo)prefab).m_variations[i].m_tree);
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Generates the lod mesh data.
        /// </summary>
        /// <param name="treeInfo">The tree information.</param>
        /// <returns>The mesh data.</returns>
        private static RenderGroup.MeshData GenerateLodMeshData(TreeInfo treeInfo)
        {
            // From game code at 2017-08-02.
            RenderGroup.VertexArrays vertexArrays = (RenderGroup.VertexArrays)0;
            int vertexCount = 0;
            int triangleCount = 0;
            int objectCount = 0;
            TreeInstance.CalculateGroupData(ref vertexCount, ref triangleCount, ref objectCount, ref vertexArrays);
            RenderGroup.MeshData data1 = new RenderGroup.MeshData(vertexArrays, vertexCount, triangleCount);
            RenderGroup.MeshData data2 = new RenderGroup.MeshData(vertexArrays, vertexCount * 4, triangleCount * 4);
            RenderGroup.MeshData data3 = new RenderGroup.MeshData(vertexArrays, vertexCount * 8, triangleCount * 8);
            RenderGroup.MeshData data4 = new RenderGroup.MeshData(vertexArrays, vertexCount * 16, triangleCount * 16);
            Vector3 zero1 = Vector3.zero;
            Vector3 zero2 = Vector3.zero;
            float maxRenderDistance = 0.0f;
            float maxInstanceDistance = 0.0f;
            //int vertexIndex1 = 0;
            //int triangleIndex1 = 0;
            //for (int index = 0; index < 1; ++index)
            //    TreeInstance.PopulateGroupData(treeInfo, new Vector3(0.0f, 0.0f, (float)index), 1f, 1f, ref vertexIndex1, ref triangleIndex1, Vector3.zero, data1, ref zero1, ref zero2, ref maxRenderDistance, ref maxInstanceDistance);
            //int vertexIndex2 = 0;
            //int triangleIndex2 = 0;
            //for (int index = 0; index < 4; ++index)
            //    TreeInstance.PopulateGroupData(treeInfo, new Vector3(0.0f, 0.0f, (float)index), 1f, 1f, ref vertexIndex2, ref triangleIndex2, Vector3.zero, data2, ref zero1, ref zero2, ref maxRenderDistance, ref maxInstanceDistance);
            //int vertexIndex3 = 0;
            //int triangleIndex3 = 0;
            //for (int index = 0; index < 8; ++index)
            //    TreeInstance.PopulateGroupData(treeInfo, new Vector3(0.0f, 0.0f, (float)index), 1f, 1f, ref vertexIndex3, ref triangleIndex3, Vector3.zero, data3, ref zero1, ref zero2, ref maxRenderDistance, ref maxInstanceDistance);
            int vertexIndex4 = 0;
            int triangleIndex4 = 0;
            for (int index = 0; index < 16; ++index)
                TreeInstance.PopulateGroupData(treeInfo, new Vector3(0.0f, 0.0f, (float)index), 1f, 1f, ref vertexIndex4, ref triangleIndex4, Vector3.zero, data4, ref zero1, ref zero2, ref maxRenderDistance, ref maxInstanceDistance);

            return data4;
        }
    }
}