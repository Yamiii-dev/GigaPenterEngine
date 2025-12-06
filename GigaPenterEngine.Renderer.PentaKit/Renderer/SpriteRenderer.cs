using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using GigaPenterEngine.Core;
using System.Drawing;

namespace GigaPenterEngine.Renderer.PentaKit
{
    public class SpriteRenderer : Component
    {
        internal int textureHandle;
        internal float aspectRatio = 1f;

        public Color color = Color.White;

        public SpriteRenderer(string spritePath)
        {
            // Create a place in the GPU's memory for our sprite
            textureHandle = GL.GenTexture();
            Use();

            // You have to flip the image vertically for OpenGL
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Load our image using StbImage
            ImageResult image = ImageResult.FromStream(File.OpenRead(spritePath), ColorComponents.RedGreenBlueAlpha);
            // Calculate the aspect ratio of our image
            aspectRatio = image.Width / (float)image.Height;

            // Send the texture data to the GPU
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            // Declare what functions to use for scaling our texture
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            // Declare how to handle coordinates outside the texture's bounds
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // I don't actually think we need to generate a mipmap for a 2D engine, but better safe than sorry.
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        internal void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
        }
    }
}
