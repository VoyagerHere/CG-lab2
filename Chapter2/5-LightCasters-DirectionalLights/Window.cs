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

        private readonly Vector3 _lightPos = new Vector3(1.2f, 5f, 4f);


        private Shader _lampShader;

        private Shader _lightingShader;

        private Texture tree_texture;
        private Texture tree_texture_specular;

        private double _timeLimit = 100;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        // StopWatch _timer;

        // double timeValue = _timer.Elapsed.TotalSeconds;


        


        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Cylinder cyl = new Cylinder(0.0f, 0.0f, 0.0f, 0.1f, 10f);
            Sphere sphere = new Sphere(2.0f, -0.0f, -10.0f, -6.5f);
            Triangle tr1 = new Triangle(2.0f, 2.0f, 2.0f, 2.0f);
            Cube cb = new Cube();

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            tree_texture = Texture.LoadFromFile("Resources/container2.png");
            tree_texture_specular = Texture.LoadFromFile("Resources/container2_specular.png");


            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");



            RenderObj.Add(new RenderObjects(cyl.GetAllTogether(), cyl.GetIndices(), tree_texture, tree_texture_specular, _lightingShader, 8));
            RenderObj.Add(new RenderObjects(sphere.GetAllTogether(), sphere.GetIndices(), tree_texture, tree_texture_specular, _lightingShader, 8));
            RenderObj.Add(new RenderObjects(cb.GetAllTogether(), cb.GetIndices(), tree_texture, tree_texture_specular, _lampShader, 8));


            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);




            _lightingShader.Use();

            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _lightingShader.SetVector3("viewPos", _camera.Position);

            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 1000.0f);

            // Directional light needs a direction, in this example we just use (-0.2, -1.0, -0.3f) as the lights direction
            _lightingShader.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
            _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));



            DrawSceneObjects();

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


        public void DrawSceneObjects()
        {
            DrawLightCube();
            DrawCylinder();
            DrawSphere();
        }

        public void DrawCylinder()
        {
            var Object = RenderObj[0];
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

        public void DrawSphere()
        {
            var Object = RenderObj[1];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)_timeLimit / 4, 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", model);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawTriange(int i)
        {
            var Object = RenderObj[1];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)_timeLimit / 4, 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", model);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawLightCube()
        {
            var Object = RenderObj[2];
            var model = Matrix4.Identity;
            Object.Bind();
            Matrix4 lampMatrix = Matrix4.CreateScale(2f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);
            Object.ApplyTexture();
            _lightingShader.SetMatrix4("model", lampMatrix);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.RenderCube();
        }

        
    }
}