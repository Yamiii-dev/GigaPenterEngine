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
            textureHandle = GL.GenTexture();
            Use();

            StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult image = ImageResult.FromStream(File.OpenRead(spritePath), ColorComponents.RedGreenBlueAlpha);
            aspectRatio = image.Width / (float)image.Height;
            Console.WriteLine($"Image size: {image.Width}x{image.Height}");
            Console.WriteLine(aspectRatio);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        internal void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
        }
    }
}
