using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    class Cylinder
    {

        public List<float> Vertecies = new List<float>();
        public List<float> Normals = new List<float>();
        public List<float> TexCoords = new List<float>();

        public List<int> Indices = new List<int>();
        public List<int> lineIndices = new List<int>();

        public float baseRadius, height;
        public int sectorCount, stackCount;

        private int baseCenterIndex;
        private int topCenterIndex;

        float x, y, z;
        public Cylinder(float x, float y, float z, float baseRadius, float height, int sectorCount = 18, int stackCount = 36)
        {
            this.baseRadius = baseRadius;
            this.height = height;
            this.sectorCount = sectorCount;
            this.stackCount = stackCount;
            this.x = x;
            this.y = y;
            this.z = z;
            this.buildVerticesSmooth();
        }
        public List<float> getUnitCircleVertices()
        {
            const float PI = 3.1415926f;
            float sectorStep = 2 * PI / sectorCount;
            float sectorAngle;

            List<float> unitCircleVertices = new List<float>();
            for (int i = 0; i <= sectorCount; ++i)
            {
                sectorAngle = i * sectorStep;
                unitCircleVertices.Add((float)Math.Cos(sectorAngle));
                unitCircleVertices.Add((float)Math.Sin(sectorAngle));
                unitCircleVertices.Add(0);
            }
            return unitCircleVertices;
        }
        public void buildVerticesSmooth()
        {
            List<float> unitVertices = getUnitCircleVertices();
            for (int i = 0; i < 2; ++i)
            {
                float h = -height / 2.0f + i * height;
                float t = 1.0f - i;

                for (int j = 0, k = 0; j <= sectorCount; ++j, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k + 1];
                    float uz = unitVertices[k + 2];
                    Vertecies.Add(ux * baseRadius + x);
                    Vertecies.Add(uy * baseRadius + y);
                    Vertecies.Add(h + z);
                    Normals.Add(ux);
                    Normals.Add(uy);
                    Normals.Add(uz);
                    TexCoords.Add((float)j / sectorCount);
                    TexCoords.Add(t);
                }
            }
            baseCenterIndex = (int)Vertecies.Count / 3;
            topCenterIndex = baseCenterIndex + sectorCount + 1;
            for (int i = 0; i < 2; ++i)
            {
                float h = -height / 2.0f + i * height;
                float nz = -1 + i * 2;
                Vertecies.Add(0 + x); Vertecies.Add(0 + y); Vertecies.Add(h + z);
                Normals.Add(0); Normals.Add(0); Normals.Add(nz);
                TexCoords.Add(0.5f); TexCoords.Add(0.5f);

                for (int j = 0, k = 0; j < sectorCount; ++j, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k + 1];
                    Vertecies.Add(ux * baseRadius + x);
                    Vertecies.Add(uy * baseRadius + y);
                    Vertecies.Add(h + z);
                    Normals.Add(0);
                    Normals.Add(0);
                    Normals.Add(nz);
                    TexCoords.Add(-ux * 0.5f + 0.5f);
                    TexCoords.Add(-uy * 0.5f + 0.5f);
                }
            }
            return;
        }
        public float[] GetNormals()
        {
            return Normals.ToArray();
        }

        public float[] GetVertecies()
        {
            return Vertecies.ToArray();
        }
        public int[] GetIndices()
        {
            int k1 = 0;
            int k2 = sectorCount + 1;

            for (int i = 0; i < sectorCount; ++i, ++k1, ++k2)
            {
                Indices.Add(k1);
                Indices.Add(k1 + 1);
                Indices.Add(k2);
                Indices.Add(k2);
                Indices.Add(k1 + 1);
                Indices.Add(k2 + 1);
            }
            for (int i = 0, k = baseCenterIndex + 1; i < sectorCount; ++i, ++k)
            {
                if (i < sectorCount - 1)
                {
                    Indices.Add(baseCenterIndex);
                    Indices.Add(k + 1);
                    Indices.Add(k);
                }
                else
                {
                    Indices.Add(baseCenterIndex);
                    Indices.Add(baseCenterIndex + 1);
                    Indices.Add(k);
                }
            }
            for (int i = 0, k = topCenterIndex + 1; i < sectorCount; ++i, ++k)
            {
                if (i < sectorCount - 1)
                {
                    Indices.Add(topCenterIndex);
                    Indices.Add(k);
                    Indices.Add(k + 1);
                }
                else
                {
                    Indices.Add(topCenterIndex);
                    Indices.Add(k);
                    Indices.Add(topCenterIndex + 1);
                }
            }


            return Indices.ToArray();
        }

        public float[] Collect()
        {
            List<float> result = new List<float>();

            for (int i = 0; i < Vertecies.Count / 3; ++i)
            {
                result.Add(Vertecies[i * 3]);
                result.Add(Vertecies[i * 3 + 1]);
                result.Add(Vertecies[i * 3 + 2]);

                result.Add(Normals[i * 3]);
                result.Add(Normals[i * 3 + 1]);
                result.Add(Normals[i * 3 + 2]);

                result.Add(TexCoords[i * 2]);
                result.Add(TexCoords[i * 2 + 1]);
            }


            return result.ToArray();


        }
    }


}


