﻿using Lab2.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;

namespace Lab2
{
    public class Window : GameWindow
    {

        private List<RenderObjects> RenderObj = new List<RenderObjects>();

        private readonly Vector3 _lightPos = new Vector3(0.0f, 10.0f, 4.0f);

        private int _vertexBufferObject;

        private int _vaoLamp;


        private Shader _lampShader;

        private Shader _lightingShader;
        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        int RotateTriangle = 0;


        private Texture _texture_tree;
        private Texture _texture_red;
        private Texture _texture_green;
        private Texture _texture_yellow;
        private Texture _texture_blue;
        private Texture _texture_specular_tree;
        private Texture _texture_specular_red;
        private Texture _texture_specular_green;
        private Texture _texture_specular_yellow;
        private Texture _texture_specular_blue;


           


        private readonly float[] _vertices =
        {
            // Positions          Normals              Texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();


            Cylinder cl1 = new Cylinder(0f, -10.0f, -1f, 0.45f, 13.8f); 
            Sphere sp1 = new Sphere(0.2f, 0.0f, 7.0f, -7.3f); 
            Cylinder cl2 = new Cylinder(0.0f, 7.0f, -9.0f, 0.1f, 3f); // Horizontal
            Cylinder cl3 = new Cylinder(0.0f, 7.0f, -7.4f, 0.8f, 0.3f);  // Disk
            Triangle tr = new Triangle();


            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);



            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");


            _texture_tree = Texture.LoadFromFile("Resources/tree.jpg");
            _texture_red = Texture.LoadFromFile("Resources/red.jpg");
            _texture_green = Texture.LoadFromFile("Resources/green.jpg");
            _texture_yellow = Texture.LoadFromFile("Resources/yellow.jpg");
            _texture_blue = Texture.LoadFromFile("Resources/blue.jpg");
            _texture_specular_tree = Texture.LoadFromFile("Resources/tree_specular.jpg");
            _texture_specular_red = Texture.LoadFromFile("Resources/red_specular.jpg");
            _texture_specular_green = Texture.LoadFromFile("Resources/green_specular.jpg");
            _texture_specular_yellow = Texture.LoadFromFile("Resources/yellow_specular.jpg");
            _texture_specular_blue = Texture.LoadFromFile("Resources/blue_specular.jpg");

            // Light Cube
            {
                _vaoLamp = GL.GenVertexArray();
                GL.BindVertexArray(_vaoLamp);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

                var positionLocation = _lampShader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            }


            SetShader(_lightingShader);

            RenderObj.Add(new RenderObjects(cl1.Collect(), cl1.GetIndices(), _texture_tree, _texture_specular_tree, _lightingShader));
            RenderObj.Add(new RenderObjects(cl2.Collect(), cl1.GetIndices(), _texture_tree, _texture_specular_tree, _lightingShader));
            RenderObj.Add(new RenderObjects(sp1.Collect(), sp1.GetIndices(), _texture_red, _texture_specular_red, _lightingShader));
            RenderObj.Add(new RenderObjects(cl3.Collect(), cl3.GetIndices(), _texture_yellow, _texture_specular_yellow, _lightingShader));

            // Triangles
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_blue, _texture_specular_blue, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_red, _texture_specular_red, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_yellow, _texture_specular_yellow, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_green, _texture_specular_green, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_blue, _texture_specular_blue, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_red, _texture_specular_red, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_yellow, _texture_specular_yellow, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_green, _texture_specular_green, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_red, _texture_specular_red, _lightingShader));
            RenderObj.Add(new RenderObjects(tr.Collect(), tr.GetIndices(), _texture_blue, _texture_specular_blue, _lightingShader));

            CursorGrabbed = true;

        }

        private void SetShader(Shader _lightingShader)
        {
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _lightingShader.SetVector3("viewPos", _camera.Position);

            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);

            // Directional light needs a direction, in this example we just use (-0.2, -1.0, -0.3f) as the lights direction
            _lightingShader.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
            _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            _lightingShader.Use();
            SetShader(_lightingShader);

            // Rotate Speed
            RotateTriangle += 10;
            RotateTriangle %= 360;

            DrawSceneObjects(RotateTriangle);

            GL.BindVertexArray(_vaoLamp);
            _lampShader.Use();

            Matrix4 lampMatrix = Matrix4.CreateScale(2.2f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        public void DrawCylinder1(int i)
        {
            var Object = RenderObj[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1;
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", model);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawCylinder2(int i)
        {
            var Object = RenderObj[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(0));
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1;
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", model);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }


        public void DrawSphere(int i)
        {
            var Object = RenderObj[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, 0, 0);
            var model = Matrix4.Identity;
            Object.Bind();
            // model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", model);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawTriange(int i, int r)
        {
            var Object = RenderObj[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX3 = Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(r));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0.0f, 7f, -8.5f);

            Matrix4 ScaleMatrix = Matrix4.CreateScale(4f);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1  * RotationMatrixX3 * ScaleMatrix * RotationMatrixX2;
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", model);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawSceneObjects(int rotate_tr)
        {
            DrawCylinder1(0);
            DrawCylinder2(1);
            DrawCylinder2(3);
            DrawTriange(4, 0 + rotate_tr);
            DrawTriange(5, 40 + rotate_tr);
            DrawTriange(6, 80 + rotate_tr);
            DrawTriange(7, 120 + rotate_tr);
            DrawTriange(8, 160 + rotate_tr);
            DrawTriange(9, 180 + rotate_tr);
            DrawTriange(10, 220 + rotate_tr);
            DrawTriange(11, 260 + rotate_tr);
            DrawTriange(12, 300 + rotate_tr);
            DrawTriange(13, 340 + rotate_tr);
            DrawSphere(2);


        }
    }
}