using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lab2;
using Lab2.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lab2
{
    class RenderObjects
    {
        public readonly int _vertexArrayObject = GL.GenVertexArray();
        public readonly int _elementBufferObject = GL.GenBuffer();
        public readonly int _vertexBufferObject = GL.GenBuffer();
        public readonly int indicesLenght;
        Texture Diffuse, Specular;
        Shader shader;
        int shader_type;
        public RenderObjects(float[] _vertices, int[] _indices, Texture diffuse, Texture specular, Shader shader, int stride, int shader_type = 0)
        {
            this.Diffuse = diffuse;
            this.shader = shader;
            this.Specular = specular;
            this.shader_type = shader_type;
            indicesLenght = _indices.Length;

            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.NamedBufferStorage(_vertexBufferObject,
               _vertices.Length * sizeof(float),        
               _vertices,                           
               BufferStorageFlags.MapWriteBit
            );    

            GL.VertexArrayAttribBinding(_vertexArrayObject, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayObject, 0);

            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 6 * sizeof(float));

            GL.VertexArrayVertexBuffer(_vertexArrayObject, 0, _vertexBufferObject, IntPtr.Zero, stride * sizeof(float));


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);//staticDraw
        }

        public void RenderCube()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        public void RenderTriangle()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

        public void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, indicesLenght, DrawElementsType.UnsignedInt, 0);
        }

        public void ApplyTexture()
        {
            Diffuse.Use(TextureUnit.Texture0);
            Specular.Use(TextureUnit.Texture1);
            shader.Use();
        }

        public void Bind()
        {
            GL.BindVertexArray(_vertexArrayObject);
        }

    }
}
