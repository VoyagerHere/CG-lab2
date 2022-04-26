using Lab2.Common;
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

        private double _timeLimit = 100;
        private double _time = 0;

        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private int _vertexBufferObject;

        private int _vaoModel;

        private int _vaoLamp;

        private Shader _lampShader;

        private Shader _lightingShader;

        /* Textures */

        private Texture tree_texture;
        private Texture tree_texture_specular;



        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Cylinder cyl = new Cylinder(0f, -10.0f, 7.3f, 6f, 1f);
            Sphere sphere = new Sphere(1.3f, 0.0f, -10.0f, -6.5f);

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            tree_texture = Texture.LoadFromFile("Resources/container2.png");
            tree_texture_specular = Texture.LoadFromFile("Resources/container2.png");


            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            
            {
                _vaoModel = GL.GenVertexArray();
                GL.BindVertexArray(_vaoModel);

                var positionLocation = _lightingShader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

                var normalLocation = _lightingShader.GetAttribLocation("aNormal");
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

                var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            }

            {
                _vaoLamp = GL.GenVertexArray();
                GL.BindVertexArray(_vaoLamp);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

                var positionLocation = _lampShader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            }


            RenderObj.Add(new RenderObjects(cyl.GetAllTogether(), cyl.GetIndices(), tree_texture, tree_texture_specular, _lampShader, 8));
            RenderObj.Add(new RenderObjects(sphere.GetAllTogether(), sphere.GetIndices(), tree_texture, tree_texture_specular, _lampShader, 8));



            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // GL.BindVertexArray(_vaoModel);

            // Textures

            // _diffuseMap.Use(TextureUnit.Texture0);
            // _specularMap.Use(TextureUnit.Texture1);

            _lightingShader.Use();

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

            DrawSceneObjects();

/*            // We want to draw all the cubes at their respective positions
            for (int i = 0; i < _cubePositions.Length; i++)
            {
                // Then we translate said matrix by the cube position
                // The model matrix consists of translations, scaling and/or rotations we'd like to apply to transform all object's vertices it to the global world space
                Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
                // We then calculate the angle and rotate the model around an axis
                float angle = 20.0f * i;
                model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
                // Remember to set the model at last so it can be used by opentk
                _lightingShader.SetMatrix4("model", model);

                // At last we draw all our cubes
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }*/
/*
            GL.BindVertexArray(_vaoModel);

            _lampShader.Use();

            Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);*/

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

        public void DrawCylinder()
        {
            var Object = RenderObj[0];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1;
            Object.ApplyTexture();
            _lampShader.SetMatrix4("model", model);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
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
            _lampShader.SetMatrix4("model", model);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawSceneObjects()
        {
            DrawCylinder();
            DrawSphere();
        }
    }
}