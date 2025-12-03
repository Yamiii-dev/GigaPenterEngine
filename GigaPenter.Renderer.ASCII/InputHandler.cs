using GigaPenterEngine.Input;

namespace GigaPenter.Input.ASCII;

internal static class KeyMapper
{
    public static Key? Map(ConsoleKey consoleKey)
    {
        switch (consoleKey)
        {
            // Letters
            case ConsoleKey.A: return Key.A;
            case ConsoleKey.B: return Key.B;
            case ConsoleKey.C: return Key.C;
            case ConsoleKey.D: return Key.D;
            case ConsoleKey.E: return Key.E;
            case ConsoleKey.F: return Key.F;
            case ConsoleKey.G: return Key.G;
            case ConsoleKey.H: return Key.H;
            case ConsoleKey.I: return Key.I;
            case ConsoleKey.J: return Key.J;
            case ConsoleKey.K: return Key.K;
            case ConsoleKey.L: return Key.L;
            case ConsoleKey.M: return Key.M;
            case ConsoleKey.N: return Key.N;
            case ConsoleKey.O: return Key.O;
            case ConsoleKey.P: return Key.P;
            case ConsoleKey.Q: return Key.Q;
            case ConsoleKey.R: return Key.R;
            case ConsoleKey.S: return Key.S;
            case ConsoleKey.T: return Key.T;
            case ConsoleKey.U: return Key.U;
            case ConsoleKey.V: return Key.V;
            case ConsoleKey.W: return Key.W;
            case ConsoleKey.X: return Key.X;
            case ConsoleKey.Y: return Key.Y;
            case ConsoleKey.Z: return Key.Z;

            // Numbers
            case ConsoleKey.D0: return Key.Num0;
            case ConsoleKey.D1: return Key.Num1;
            case ConsoleKey.D2: return Key.Num2;
            case ConsoleKey.D3: return Key.Num3;
            case ConsoleKey.D4: return Key.Num4;
            case ConsoleKey.D5: return Key.Num5;
            case ConsoleKey.D6: return Key.Num6;
            case ConsoleKey.D7: return Key.Num7;
            case ConsoleKey.D8: return Key.Num8;
            case ConsoleKey.D9: return Key.Num9;

            // Arrow keys
            case ConsoleKey.UpArrow: return Key.Up;
            case ConsoleKey.DownArrow: return Key.Down;
            case ConsoleKey.LeftArrow: return Key.Left;
            case ConsoleKey.RightArrow: return Key.Right;

            // Control keys
            case ConsoleKey.Enter: return Key.Enter;
            case ConsoleKey.Spacebar: return Key.Space;
            case ConsoleKey.Tab: return Key.Tab;
            case ConsoleKey.Backspace: return Key.Backspace;
            case ConsoleKey.Escape: return Key.Escape;
            case ConsoleKey.Delete: return Key.Delete;
            case ConsoleKey.Insert: return Key.Insert;
            case ConsoleKey.Home: return Key.Home;
            case ConsoleKey.End: return Key.End;
            case ConsoleKey.PageUp: return Key.PageUp;
            case ConsoleKey.PageDown: return Key.PageDown;

            // Function keys
            case ConsoleKey.F1: return Key.F1;
            case ConsoleKey.F2: return Key.F2;
            case ConsoleKey.F3: return Key.F3;
            case ConsoleKey.F4: return Key.F4;
            case ConsoleKey.F5: return Key.F5;
            case ConsoleKey.F6: return Key.F6;
            case ConsoleKey.F7: return Key.F7;
            case ConsoleKey.F8: return Key.F8;
            case ConsoleKey.F9: return Key.F9;
            case ConsoleKey.F10: return Key.F10;
            case ConsoleKey.F11: return Key.F11;
            case ConsoleKey.F12: return Key.F12;

            default: return null; // fallback for unmapped keys
        }
    }
}

public static class InputHandler
{
    private static Key? key;
    public static void Update()
    {
        ConsoleKey input = Console.ReadKey(true).Key;
        key = KeyMapper.Map(input);
    }
    
    public static bool KeyDown(Key _key)
    {
        if (key == null) return false;
        if (key == _key) return true;
        return false;
    }

    public static bool KeyUp(Key _key)
    {
        if (key == null) return true;
        if (key == _key) return false;
        return true;
    }
}