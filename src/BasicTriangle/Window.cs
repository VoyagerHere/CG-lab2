using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.IO;

namespace Lab2
{
    public class Window : GameWindow
    {
        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title) { }


        float[] vertices = {
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        // VBO Descriptor
        // store a large number of vertices in the GPU's memory
        int VertexBufferObject;

        // Element Buffer Object,
        // which is a type of buffer that lets us re-use vertices to create multiple primitives out of them
        int ElementBufferObject;
        // Stores our vertex attribute configuration and which VBO to use
        int VertexArrayObject;
        Shader shader;


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            // Window color after clean 
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Initialization code
            VertexBufferObject = GL.GenBuffer();
            
            shader = new Shader
             (
                "../../Shaders/shader.vert",
                "../../Shaders/shader.frag"
             );

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Copy user-defined data into the currently-bound buffer
            GL.BufferData(
                BufferTarget.ArrayBuffer /* buffer*/,
                vertices.Length * sizeof(float) /*size*/,
                vertices, /*data to buffer*/
                BufferUsageHint.StaticDraw
             /* StaticDraw: the data will most likely not change at all or very rarely.
                DynamicDraw: the data is likely to change a lot.
                StreamDraw: the data will change every time it is drawn.
             */
             );

            //  Tell OpenGL how it should interpret the vertex data
            GL.VertexAttribPointer
            (
                0, // position = 0
                3, // vec3
                VertexAttribPointerType.Float,
                false, // Normalize
                3 * sizeof(float), // Space between consecutive vertex attributes
                0 //  offset of where the position data begins in the buffer.
            );
            GL.EnableVertexAttribArray(0);

            // ..:: Initialization code (done once (unless your object frequently changes)) :: ..
            // 1. bind Vertex Array Object
            GL.BindVertexArray(VertexArrayObject);
            // 2. copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            // 3. then set our vertex attributes pointers
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // Bind the VAO
            GL.BindVertexArray(VertexArrayObject);
            // Use/Bind the program
            shader.Use();
            // This draws the triangle.
            GL.DrawElements
            (
                PrimitiveType.Triangles, // PrimitiveType
                indices.Length, // The amount of vertices to draw
                DrawElementsType.UnsignedInt, // The type of the EBO's elements
                0 // The offset of what we want to draw
            );

            // Double-buffering
            // One area is displayed, while the other is being rendered to
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}











