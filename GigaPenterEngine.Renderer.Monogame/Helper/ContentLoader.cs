using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.IO;
using StbImageSharp;

namespace GigaPenter.Renderer.Monogame.Helper;

// Records all loaded textures, needed for creating a Texture2D
internal static class TextureRegistry
{
    private static List<Texture> _textures = new List<Texture>();

    public static void AddTexture(Texture texture)
    {
        _textures.Add(texture);
    }

    public static void RenderTextures(GraphicsDevice graphicsDevice)
    {
        // Takes the pixel data we have and turns it into a Texture2D for every texture we have created
        foreach (var texture in _textures)
        {
            Texture2D newTexture = new Texture2D(graphicsDevice, texture.width, texture.height);
            newTexture.SetData(texture.data);
            texture.texture = newTexture;
        }
    }
    public static void RenderTexture(GraphicsDevice graphicsDevice, Texture texture)
    {
        // Takes the pixel data of the input Texture object we have and turns it into a Texture2D
        Texture2D newTexture = new Texture2D(graphicsDevice, texture.width, texture.height);
        newTexture.SetData(texture.data);
        texture.texture = newTexture;
    }
}

// Texture object for saving pixel data that gets used to create a Texture2D
public class Texture
{
    public byte[] data;
    public int width;
    public int height;
    public Texture2D texture;

    public Texture(byte[] _data,  int _width, int _height)
    {
        data =  _data;
        width = _width;
        height = _height;
        TextureRegistry.AddTexture(this);
    }
}

public static class ContentLoader
{
    // Load the pixel data from an image
    public static Texture LoadTexture(string path)
    {
        ImageResult img = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
        
        return new Texture(img.Data, img.Width, img.Height);
    }
}