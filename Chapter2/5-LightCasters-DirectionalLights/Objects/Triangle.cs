using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{


    class Triangle
    {
        public float r, x, y, z;

        public Triangle(float x, float y, float z, float r)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
            this.GetVertecies();

        }

        public float[] GetVertecies()
        {
            float[] vertices = {
                x - 0.5f,  y - 0.5f, 0.0f, //Bottom-left vertex
                x + 0.5f,  y - 0.5f, 0.0f, //Bottom-right vertex
                 0.0f,  y + 0.5f, 0.0f  //Top vertex
            };
            return vertices;
        }

       /* public int[] GetIndices()
        {

        }*/

    }
}
