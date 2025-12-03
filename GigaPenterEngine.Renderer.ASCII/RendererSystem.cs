using System.Numerics;
using GigaPenterEngine.BaseComponents;
using GigaPenterEngine.Core;
using Vector2 = GigaPenterEngine.Core.Vector2;

namespace GigaPenterEngine.Renderer.ASCII;

// Renderer akin to something like ncurses, made purely for early testing, jeb na to brasi
public class RendererSystem : GameSystem
{
    public int width;
    public int height;
    
    public RendererSystem(int _width, int _height)
    {
        width = _width;
        height = _height;
    }

    public override void Start()
    {
        buffer = new Symbol[height,width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                buffer[y, x] = new Symbol();
            }
        }
        Console.CursorVisible = false;
        Console.TreatControlCAsInput = true;
        try
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
        catch
        {
            
        }
        Console.Clear();
    }

    private class Symbol
    {
        public char symbol;
        public ConsoleColor color;

        public Symbol(char _symbol = ' ', ConsoleColor _color = ConsoleColor.White)
        {
            symbol = _symbol;
            color = _color;
        }
    }
    
    private Symbol[,] buffer;

    public void Draw(char symbol, ConsoleColor color, int x, int y)
    {
        if(buffer[y, x].symbol == symbol && buffer[y, x].color == color)
            return;
        buffer[y, x].symbol = symbol;
        buffer[y, x].color = color;
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(symbol);
    }
    
    public override void Update()
    {
        List<RenderableComponent> components = ComponentRegistry.GetComponents<RenderableComponent>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool occupied = false;
                foreach (RenderableComponent component in components)
                {
                    Vector2 position = component.Parent.GetComponent<Transform>().Position;
                    if ((int)position.X == x && (int)position.Y == y)
                    {
                        Draw(component.Symbol, component.Color, x, y);
                        occupied = true;
                        break;
                    }
                }

                if (!occupied)
                {
                    Draw(' ', ConsoleColor.Black,  x, y);
                }
            }
        }
    }
}