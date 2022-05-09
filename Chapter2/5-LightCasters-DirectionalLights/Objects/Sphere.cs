using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    class Sphere
    {
        public List<float> Vertecies = new List<float>();
        public List<float> Normals = new List<float>();
        public List<float> TexCoords = new List<float>();

        public List<int> Indices = new List<int>();
        public List<int> lineIndices = new List<int>();

        public float R, X, Y, Z;
        public int sectorCount, stackCount;

        public Sphere(float R, float X, float Y, float Z, int sectorCount = 30, int stackCount = 90)
        {
            this.R = R;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.sectorCount = sectorCount;
            this.stackCount = stackCount;
            this.GetVertecies();
            this.GetIndices();
        }

        public float[] GetNormals()
        {
            return Normals.ToArray();
        }

        public float[] GetVertecies()
        {
            const float PI = 3.1415926f;

            float x, y, z, xy;
            float nx, ny, nz, lengthInv = 1.0f / R;
            float s, t;

            float sectorStep = 2 * PI / sectorCount;
            float stackStep = PI / stackCount;
            float sectorAngle, stackAngle;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = PI / 2 - i * stackStep;
                xy = R * (float)Math.Cos(stackAngle);
                z = R * (float)Math.Sin(stackAngle) + Z;
                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;
                    x = xy * (float)Math.Cos(sectorAngle) + X;
                    y = xy * (float)Math.Sin(sectorAngle) + Y;
                    Vertecies.Add(x);
                    Vertecies.Add(y);
                    Vertecies.Add(z);
                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;
                    Normals.Add(nx);
                    Normals.Add(ny);
                    Normals.Add(nz);
                    s = (float)j / sectorCount;
                    t = (float)i / stackCount;
                    TexCoords.Add(s);
                    TexCoords.Add(t);

                }
            }
            return Vertecies.ToArray();
        }

        public int[] GetIndices()
        {
            int k1, k2;

            for (int i = 0; i < stackCount; ++i)
            {
                k1 = i * (sectorCount + 1);
                k2 = k1 + sectorCount + 1;

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        Indices.Add(k1);
                        Indices.Add(k2);
                        Indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        Indices.Add(k1 + 1);
                        Indices.Add(k2);
                        Indices.Add(k2 + 1);
                    }
                    lineIndices.Add(k1);
                    lineIndices.Add(k2);
                    if (i != 0)
                    {
                        lineIndices.Add(k1);
                        lineIndices.Add(k1 + 1);
                    }
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

        public void SetCoords(float coordX, float coordY, float coordZ)
        {
            this.X = coordX;
            this.Y = coordY;
            this.Z = coordZ;

            GetVertecies();
        }


    }
}
