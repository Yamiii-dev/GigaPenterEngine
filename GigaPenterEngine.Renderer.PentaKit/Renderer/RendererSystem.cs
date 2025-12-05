using GigaPenterEngine.BaseComponents;
using GigaPenterEngine.Core;
using GigaPenterEngine.Renderer.PentaKit.Input;
using GigaPenterEngine.Renderer.PentaKit.Internal;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using Vector2 = OpenTK.Mathematics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace GigaPenterEngine.Renderer.PentaKit
{
    internal static class Constants
    {
        public static float[] unitQuad = new float[]
        {
            -0.5f, -0.5f, 0f, 0f, // bottom-left
            0.5f, -0.5f, 1f, 0f, // bottom-right
            0.5f,  0.5f, 1f, 1f, // top-right
            -0.5f,  0.5f, 0f, 1f  // top-left
        };
        public static int[] unitIndices = new int[] {
            0,1,2,2,3,0
        };
    }
    public class RendererSystem : GameSystem
    {
        int Width = 0;
        int Height = 0;

        GameWindow window;

        Shader shader;

        int vao;

        Game engineGame;

        InputHandler instance = new InputHandler();

        public RendererSystem(int width, int height, string title, Game engineGame)
        {
            Width = width;
            Height = height;
            window = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title });
            //window.Run();
            window.Load += OnLoad;
            window.FramebufferResize += OnFrameBufferResize;
            this.engineGame = engineGame;
            window.Closing += OnExit;

            shader = new Shader();
            // Set up buffers
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Constants.unitQuad.Length * sizeof(float), Constants.unitQuad, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Constants.unitIndices.Length * sizeof(int), Constants.unitIndices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1); 
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);
        }

        private void OnExit(CancelEventArgs args)
        {
            Console.WriteLine("Closing");
            engineGame.RequestExit();
        }

        private void OnFrameBufferResize(FramebufferResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            Width = e.Width;
            Height = e.Height;
        }

        void OnLoad()
        {
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        Matrix4 SpriteToTransform(Vector3 position, Vector2 scale, float aspectRatio)
        {
            float width = scale.X * aspectRatio;
            Matrix4 trans = Matrix4.CreateTranslation(position);
            Matrix4 scaleM = Matrix4.CreateScale(new Vector3(width, scale.Y, 1f));
            Matrix4 model = trans * scaleM;
            return model;
        }

        public override void Update()
        {
            instance.currentState = window.KeyboardState;
            window.ProcessEvents(0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            var sprites = ComponentRegistry.GetComponents<SpriteRenderer>();

            var camera = ComponentRegistry.GetComponents<Camera>().FirstOrDefault();
            Matrix4 view = Matrix4.Identity;
            float worldHeight = 5f;                // 20 units tall
            float aspectRatio = Width / (float)Height;
            float worldWidth = worldHeight * aspectRatio;
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(-worldWidth / 2, worldWidth / 2, -worldHeight / 2, worldHeight / 2, -1, 1f);
            if (camera != null)
            {
                var cameraTransform = camera.Parent.GetComponent<Transform>();
                view = Matrix4.CreateTranslation(new Vector3(-cameraTransform.Position.X, -cameraTransform.Position.Y, 0f));
                var cameraWidth = worldWidth * cameraTransform.Scale.X;
                var cameraHeight = worldHeight * cameraTransform.Scale.Y;
                projection = Matrix4.CreateOrthographicOffCenter(-cameraWidth / 2, cameraWidth / 2, -cameraHeight / 2, cameraHeight / 2, -1, 1f);
            }
            foreach (var sprite in sprites)
            {
                var transform = sprite.Parent.GetComponent<Transform>();
                if (transform == null)
                    continue;
                GL.BindVertexArray(vao);
                shader.Use();
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, sprite.textureHandle);
                shader.SetInt("Texture0", 0);


                Vector4 vec4Color = new Vector4(sprite.color.R / 255f, sprite.color.G / 255f, sprite.color.B / 255f, sprite.color.A / 255f);
                shader.SetVector4("Color", vec4Color);
                Matrix4 model = SpriteToTransform(Helpers.ConvertToTK(transform.Position), Helpers.ConvertToTK(transform.Scale), sprite.aspectRatio);

                //Console.WriteLine(Vector3.TransformPosition(new Vector3(0f, 0f, 0f), model));

                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.DrawElements(PrimitiveType.Triangles, Constants.unitIndices.Length, DrawElementsType.UnsignedInt, 0);
            }
            window.SwapBuffers();
        }
    }
}
