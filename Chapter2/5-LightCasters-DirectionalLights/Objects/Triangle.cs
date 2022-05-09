using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{


    class Triangle
    {

        private readonly float[] _vertices =
         {
            0.5f, 0.0f, -0.5f,
            0.0f, -0.25f, 0.0f,
            0.0f, 0.0f, -0.5f, 
            0.0f, 0.25f, 0.0f,
        };

        private readonly float[] _tex_coords =
        {
            1.0f, 1.0f,
            1.0f, 0.0f,
            0.0f, 0.0f,
            0.0f, 1.0f
        };


        private readonly float[] _normals =
        {
            1.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 0.0f,
        };

        private readonly int[] _indices =
        {
            0, 1, 2,
            0, 2, 3,
        };



        public float[] Collect()
        {
            List<float> result = new List<float>();
            for (int i = 0; i < _vertices.Length / 3; ++i)
            {
                result.Add(_vertices[i * 3]);
                result.Add(_vertices[i * 3 + 1]);
                result.Add(_vertices[i * 3 + 2]);

                result.Add(_normals[i * 3]);
                result.Add(_normals[i * 3 + 1]);
                result.Add(_normals[i * 3 + 2]);

                result.Add(_tex_coords[i * 2]);
                result.Add(_tex_coords[i * 2 + 1]);
            }


            return result.ToArray();

        }

        public int[] GetIndices()
        {
            return _indices;
        }


    }
}
