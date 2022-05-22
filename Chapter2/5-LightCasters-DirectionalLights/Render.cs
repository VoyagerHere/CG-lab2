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
        public RenderObjects(float[] _vertices, int[] _indices, Texture diffuse, Texture specular, Shader shader)
        {
            indicesLenght = _indices.Length;
            this.shader = shader;
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Bind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.NamedBufferStorage(
               _vertexBufferObject,
               _vertices.Length * sizeof(float),        // the size needed by this buffer
               _vertices,                           // data to initialize with
               BufferStorageFlags.MapWriteBit);    // at this point we will only write to the buffer

            GL.VertexArrayAttribBinding(_vertexArrayObject, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayObject, 0);

            var positionLocation = shader.GetAttribLocation("aPos​");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var texCoordLocation = shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.VertexArrayVertexBuffer(_vertexArrayObject, 0, _vertexBufferObject, IntPtr.Zero, 8 * sizeof(float));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);

            
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

        public void ShaderAttribute()
        {
            this.Bind();

            var positionLocation = shader.GetAttribLocation("aPos​");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var texCoordLocation = shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

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
