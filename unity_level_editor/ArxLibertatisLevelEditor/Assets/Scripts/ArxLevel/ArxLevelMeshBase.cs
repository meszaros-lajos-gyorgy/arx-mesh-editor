﻿using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    public abstract class ArxLevelMeshBase
    {
        public readonly ArxLevel level;

        protected string[] texArxPaths;
        protected Dictionary<int, int> tcToIndex;

        public ArxLevelMeshBase(ArxLevel level)
        {
            this.level = level;
        }

        int lightIndex = 0;
        protected void CreateMeshBegin()
        {
            lightIndex = 0;
        }

        protected void LoadTextures()
        {
            texArxPaths = new string[level.FTS.textureContainers.Length];
            tcToIndex = new Dictionary<int, int>();
            for (int i = 0; i < level.FTS.textureContainers.Length; i++)
            {
                texArxPaths[i] = ArxIOHelper.GetString(level.FTS.textureContainers[i].fic);
                tcToIndex[level.FTS.textureContainers[i].tc] = i;
            }
        }

        protected void AddPoly(SubMeshData data, FTS_IO.FTS_IO_EERIEPOLY poly)
        {
            int firstVert = data.verts.Count;
            if (poly.type.HasFlag(PolyType.QUAD))
            { //QUAD
                for (int i = 0; i < 4; i++)
                {
                    var vert = poly.vertices[i];
                    data.verts.Add(new Vector3(vert.posX, vert.posY, vert.posZ));
                    data.uvs.Add(new Vector2(vert.texU, 1 - vert.texV));
                    data.norms.Add(poly.normals[i].ToVector3());
                    data.colors.Add(ArxIOHelper.FromBGRA(level.LLF.lightColors[lightIndex++]));
                }

                data.indices.Add(firstVert);
                data.indices.Add(firstVert + 1);
                data.indices.Add(firstVert + 2);
                data.indices.Add(firstVert + 2);
                data.indices.Add(firstVert + 1);
                data.indices.Add(firstVert + 3);
            }
            else
            { //TRIANGLE
                for (int i = 0; i < 3; i++)
                {
                    var vert = poly.vertices[i];
                    data.verts.Add(new Vector3(vert.posX, vert.posY, vert.posZ));
                    data.uvs.Add(new Vector2(vert.texU, 1 - vert.texV));
                    data.norms.Add(poly.normals[i].ToVector3());
                    data.colors.Add(ArxIOHelper.FromBGRA(level.LLF.lightColors[lightIndex++]));
                }

                data.indices.Add(firstVert);
                data.indices.Add(firstVert + 1);
                data.indices.Add(firstVert + 2);
            }
        }

        protected Mesh SubMeshToMesh(SubMeshData data)
        {
            Mesh m = new Mesh();

            m.vertices = data.verts.ToArray();
            m.uv = data.uvs.ToArray();
            m.normals = data.norms.ToArray();
            m.colors = data.colors.ToArray();
            m.triangles = data.indices.ToArray();

            return m;
        }
    }
}
