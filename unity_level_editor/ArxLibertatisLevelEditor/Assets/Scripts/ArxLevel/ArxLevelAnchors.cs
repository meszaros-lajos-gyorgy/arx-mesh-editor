﻿using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    public class ArxLevelAnchors
    {
        private ArxLevel level;

        public ArxLevelAnchors(ArxLevel level)
        {
            this.level = level;
        }

        public GameObject CreateAnchors()
        {
            GameObject anchorsObject = new GameObject();

            List<Vector3> vertices = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();

            List<GameObject> anchors = new List<GameObject>();
            foreach (var anchor in level.FTS.anchors)
            {
                GameObject anchorObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Anchor"));
                anchorObject.transform.position = anchor.data.pos.ToVector3();
                anchors.Add(anchorObject);

                for(int i =0; i< anchor.linkedAnchors.Length; i++)
                {
                    int start = indices.Count;

                    var other = level.FTS.anchors[anchor.linkedAnchors[i]];
                    vertices.Add(anchor.data.pos.ToVector3());
                    colors.Add(Color.black);
                    vertices.Add(other.data.pos.ToVector3());
                    colors.Add(Color.white);
                    indices.Add(start);
                    indices.Add(start+1);
                }
            }

            var m = new Mesh();
            m.SetVertices(vertices);
            m.SetColors(colors);
            m.subMeshCount = 1;
            if(indices.Count > ushort.MaxValue)
            {
                m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            m.SetIndices(indices, MeshTopology.Lines, 0);

            var mf = anchorsObject.AddComponent<MeshFilter>();
            mf.sharedMesh = m;
            var mr = anchorsObject.AddComponent<MeshRenderer>();
            mr.sharedMaterial = Material.Instantiate(Resources.Load<Material>("Materials/AnchorConnection"));

            foreach (var cellIndex in LevelCellIndex.GetCellIndices(level.FTS.sceneHeader.sizex, level.FTS.sceneHeader.sizez))
            {
                var cell = level.FTS.cells[cellIndex.index];
                if (cell.anchors.Length > 0)
                {
                    GameObject cellObject = new GameObject("Cell " + cellIndex.x + "," + cellIndex.z);
                    cellObject.transform.SetParent(anchorsObject.transform);
                    foreach (var anchorIndex in cell.anchors)
                    {
                        anchors[anchorIndex].transform.SetParent(cellObject.transform);
                    }
                }
            }

            return anchorsObject;
        }
    }
}
