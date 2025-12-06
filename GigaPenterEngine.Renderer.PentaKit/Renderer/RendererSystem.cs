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
    // A quad (square made of two triangles) that gets used to render sprites on the screen.
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
            // Create a new OpenGL Window
            window = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title });
            window.Load += OnLoad;
            window.FramebufferResize += OnFrameBufferResize;
            this.engineGame = engineGame;
            window.Closing += OnExit;

            // Create a shader object to use for rendering sprites
            shader = new Shader();

            // Creates a vertex array object on the GPU and binds it.
            // a Vertex Array Object stores the actual model data we want to render. Think of it as a crate of legos with no manual.
            // One interesting feature of a VAO is that whatever VBO and EBO we define after we bind it gets linked to it, so if we ever need to render this specific mesh, we only need to bind the VAO.
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            // Create a buffer on the GPU for our Vertex Buffer Object and Element Buffer Object
            // a Vertex Buffer Object stores the actual information on how to handle our data in our Vertex Array Object
            // an Element Buffer Object stores the "elements" of our mesh. For example if you wanna create a square, it's more efficient to reuse two vertices of the first triangle for the second, this stores what vertices the triangles are made of so we don't have to create individual triangles.
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            // Create a memory block for our VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Constants.unitQuad.Length * sizeof(float), Constants.unitQuad, BufferUsageHint.DynamicDraw);

            // Create a memory block for our EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Constants.unitIndices.Length * sizeof(int), Constants.unitIndices, BufferUsageHint.StaticDraw);

            // Define how to read our data in the VAO
            GL.EnableVertexAttribArray(0); // Define where to find the position data of our vertex
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1); // Define where to find the UV data of our vertex
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);
        }

        // If our window gets closed, let the engine know
        private void OnExit(CancelEventArgs args)
        {
            Console.WriteLine("Closing");
            engineGame.RequestExit();
        }

        // If we resize our window, let the GPU know so it can resize it's viewport accordingly (as again, we only deal with coordinates from -1.0 to 1.0, which then get mapped to the viewport by the GPU.)
        private void OnFrameBufferResize(FramebufferResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            Width = e.Width;
            Height = e.Height;
        }

        void OnLoad()
        {
            // Set the color to use as the base of our rendered image
            GL.ClearColor(0f, 0f, 0f, 1f);
            // Enable blending and set the blend function.
            // This lets us use transparent sprites.
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        // Create the model matrix for our basic quad based on the Transform component of our entity.
        Matrix4 SpriteToTransform(Vector3 position, float rotation, Vector2 scale, float aspectRatio)
        {
            // The width of our quad gets calculated based on the aspect ratio of our sprite
            float width = scale.X * aspectRatio;
            Matrix4 trans = Matrix4.CreateTranslation(position);
            Matrix4 rot = Matrix4.CreateRotationZ(rotation);
            Matrix4 scaleM = Matrix4.CreateScale(new Vector3(width, scale.Y, 1f));
            Matrix4 model = trans * rot * scaleM;
            return model;
        }

        public override void Update()
        {
            // Update the Input Handler's current saved state
            instance.currentState = window.KeyboardState;
            window.ProcessEvents(0f);
            // Clear our current buffer
            GL.Clear(ClearBufferMask.ColorBufferBit);
            // Get all our renderable entities
            var sprites = ComponentRegistry.GetComponents<SpriteRenderer>();

            // Check if we have a camera in our scene
            var camera = ComponentRegistry.GetComponents<Camera>().FirstOrDefault();
            Matrix4 view = Matrix4.Identity;
            float worldHeight = 5f;                // 20 units tall
            float aspectRatio = Width / (float)Height;
            float worldWidth = worldHeight * aspectRatio;
            // Create a projection matrix at the center of the game world.
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(-worldWidth / 2, worldWidth / 2, -worldHeight / 2, worldHeight / 2, -1, 1f);
            // If we have a camera, create a projection matrix based on the camera's position and scale
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
                // Bind the VAO that stores our quad so the GPU knows to use it
                GL.BindVertexArray(vao);
                // Bind our sprite shader
                shader.Use();
                // Bind our texture and declare it's slot
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, sprite.textureHandle);
                shader.SetInt("Texture0", 0);

                // Turn our sprite's color into a Vector4 for the shader to use
                Vector4 vec4Color = new Vector4(sprite.color.R / 255f, sprite.color.G / 255f, sprite.color.B / 255f, sprite.color.A / 255f);
                shader.SetVector4("Color", vec4Color);
                // Create a model matrix with our helper method
                Matrix4 model = SpriteToTransform(Helpers.ConvertToTK(transform.Position), transform.Rotation, Helpers.ConvertToTK(transform.Scale), sprite.aspectRatio);

                // Send all our matrices to our shader
                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                // Draw the quad on our screen
                GL.DrawElements(PrimitiveType.Triangles, Constants.unitIndices.Length, DrawElementsType.UnsignedInt, 0);
            }
            // Swap the currently used buffer.
            // OpenGL uses "Double buffering", which means that we have two buffers, one that's shown on the screen, and one that's being worked on.
            // This means we have to swap what buffer we are working on if we wanna show stuff on the screen.
            window.SwapBuffers();
        }
    }
}
