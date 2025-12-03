using GigaPenterEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = GigaPenterEngine.Renderer.Monogame.Helper.Texture;

namespace GigaPenterEngine.Renderer.Monogame;

using Helper_Texture = Helper.Texture;

public class RenderableComponent : Component
{
    public Helper_Texture Texture;
    public Color Color;

    public RenderableComponent(Helper_Texture texture)
    {
        Texture = texture;
        Color = Color.White;
    }
    
    public RenderableComponent(Helper_Texture texture, Color color)
    {
        Texture = texture;
        Color = color;
    }
}