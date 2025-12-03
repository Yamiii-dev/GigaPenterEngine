using System.Drawing;
using GigaPenterEngine.Core;

namespace GigaPenterEngine.Renderer.ASCII;

public class RenderableComponent : Component
{
    public char Symbol { get; set; }
    public ConsoleColor Color { get; set; }
    public RenderableComponent(char symbol, ConsoleColor color = ConsoleColor.White) : base()
    {
        Color = color;
        Symbol = symbol;
    }
}