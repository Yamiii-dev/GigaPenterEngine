using GigaPenterEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture = GigaPenter.Renderer.Monogame.Helper.Texture;

namespace GigaPenter.Renderer.Monogame;


public class RenderableComponent : Component
{
    public Texture Texture;
    public Color Color;

    public RenderableComponent(Texture texture)
    {
        Texture = texture;
        Color = Color.White;
    }
    
    public RenderableComponent(Texture texture, Color color)
    {
        Texture = texture;
        Color = color;
    }
}