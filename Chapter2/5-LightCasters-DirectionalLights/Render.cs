﻿using System;
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

        Texture Diffuse, Specular; //диффузная, спекулярная
        Shader shader;
        int shader_type;

        public RenderObjects(float[] _vertices, int[] _indices, Texture diffuse, Texture specular, Shader shader, int stride, int shader_type = 0)
        {
            this.Diffuse = diffuse;
            this.shader = shader;
            this.Specular = specular;
            this.shader_type = shader_type;
            indicesLenght = _indices.Length;

            //Создаётся связь массива вертексов и переменной присваивается дискриптор
            GL.BindVertexArray(_vertexArrayObject);

            //Создаётся связь с буффером
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            //инициализируем буфферное 
            GL.NamedBufferStorage(_vertexBufferObject,
               _vertices.Length * sizeof(float),        // размер, необходимый для этого буффера
               _vertices,                           // данные, которые нужно положить в буферр
               BufferStorageFlags.MapWriteBit);    // на этом этапе мы будем записывать только в буфер

            //настройки vao
            GL.VertexArrayAttribBinding(_vertexArrayObject, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayObject, 0);

            //собираем координты, посылаем туда, задаём инструкции как их читать
            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);

            if (shader_type == 0)
            {
                //собираем координты, посылаем туда, задаём инструкции как их читать
                var normalLocation = shader.GetAttribLocation("aNormal");
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));

                //собираем координты, посылаем туда, задаём инструкции как их читать
                var texCoordLocation = shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 6 * sizeof(float));

                // свяжите массив вершин и буфер и укажите шаг в качестве размера вершины
                GL.VertexArrayVertexBuffer(_vertexArrayObject, 0, _vertexBufferObject, IntPtr.Zero, stride * sizeof(float));


                //Связываем буфер, и посылаем туда данные индексов (для дальнейшего их использования, чтобы соединять точки)
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);//staticDraw

            }
        }

        public void RenderCube()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
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
