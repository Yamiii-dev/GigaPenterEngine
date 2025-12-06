using GigaPenterEngine.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GigaPenterEngine.Renderer.PentaKit.Input
{
    internal static class KeyConverter
    {
        private static readonly Dictionary<Key, Keys> keyMap = new Dictionary<Key, Keys>()
    {
        // Letters
        { Key.A, Keys.A }, { Key.B, Keys.B }, { Key.C, Keys.C }, { Key.D, Keys.D },
        { Key.E, Keys.E }, { Key.F, Keys.F }, { Key.G, Keys.G }, { Key.H, Keys.H },
        { Key.I, Keys.I }, { Key.J, Keys.J }, { Key.K, Keys.K }, { Key.L, Keys.L },
        { Key.M, Keys.M }, { Key.N, Keys.N }, { Key.O, Keys.O }, { Key.P, Keys.P },
        { Key.Q, Keys.Q }, { Key.R, Keys.R }, { Key.S, Keys.S }, { Key.T, Keys.T },
        { Key.U, Keys.U }, { Key.V, Keys.V }, { Key.W, Keys.W }, { Key.X, Keys.X },
        { Key.Y, Keys.Y }, { Key.Z, Keys.Z },

        // Numbers
        { Key.Num0, Keys.D0 }, { Key.Num1, Keys.D1 }, { Key.Num2, Keys.D2 },
        { Key.Num3, Keys.D3 }, { Key.Num4, Keys.D4 }, { Key.Num5, Keys.D5 },
        { Key.Num6, Keys.D6 }, { Key.Num7, Keys.D7 }, { Key.Num8, Keys.D8 },
        { Key.Num9, Keys.D9 },

        // Function keys
        { Key.F1, Keys.F1 }, { Key.F2, Keys.F2 }, { Key.F3, Keys.F3 }, { Key.F4, Keys.F4 },
        { Key.F5, Keys.F5 }, { Key.F6, Keys.F6 }, { Key.F7, Keys.F7 }, { Key.F8, Keys.F8 },
        { Key.F9, Keys.F9 }, { Key.F10, Keys.F10 }, { Key.F11, Keys.F11 }, { Key.F12, Keys.F12 },

        // Control keys
        { Key.Escape, Keys.Escape }, { Key.Enter, Keys.Enter }, { Key.Space, Keys.Space },
        { Key.Tab, Keys.Tab }, { Key.Backspace, Keys.Backspace }, { Key.Delete, Keys.Delete },
        { Key.Insert, Keys.Insert }, { Key.Home, Keys.Home }, { Key.End, Keys.End },
        { Key.PageUp, Keys.PageUp }, { Key.PageDown, Keys.PageDown }, { Key.Pause, Keys.Pause },
        { Key.PrintScreen, Keys.PrintScreen },

        // Arrows
        { Key.Up, Keys.Up }, { Key.Down, Keys.Down }, { Key.Left, Keys.Left }, { Key.Right, Keys.Right },

        // Modifiers
        { Key.LeftShift, Keys.LeftShift }, { Key.RightShift, Keys.RightShift },
        { Key.LeftCtrl, Keys.LeftControl }, { Key.RightCtrl, Keys.RightControl },
        { Key.LeftAlt, Keys.LeftAlt }, { Key.RightAlt, Keys.RightAlt },
        { Key.CapsLock, Keys.CapsLock }, { Key.NumLock, Keys.NumLock }, { Key.ScrollLock, Keys.ScrollLock },

        // Symbols
        { Key.Minus, Keys.Minus }, { Key.Plus, Keys.Equal }, { Key.Equal, Keys.Equal },
        { Key.LeftBracket, Keys.LeftBracket }, { Key.RightBracket, Keys.RightBracket },
        { Key.Backslash, Keys.Backslash }, { Key.Semicolon, Keys.Semicolon },
        { Key.Apostrophe, Keys.Apostrophe }, { Key.Comma, Keys.Comma }, { Key.Period, Keys.Period },
        { Key.Slash, Keys.Slash }, { Key.Grave, Keys.GraveAccent }
    };

        public static Keys ToOpenTKKey(Key key)
        {
            if (keyMap.TryGetValue(key, out var otkKey))
                return otkKey;
            return Keys.Unknown;
        }
    }
    public class InputHandler : IDisposable
    {
        internal KeyboardState currentState;

        public static InputHandler instance;
        private bool disposedValue;

        public InputHandler()
        {
            if (instance == null)
                instance = this;
            else this.Dispose();
        }
        public bool KeyDown(Key key)
        {
            Keys tkKey = KeyConverter.ToOpenTKKey(key);
            return currentState.IsKeyDown(tkKey);
        }

        public bool KeyUp(Key key)
        {
            Keys tkKey = KeyConverter.ToOpenTKKey(key);
            return currentState.IsKeyReleased(tkKey);
        }

        public bool KeyPressed(Key key)
        {
            Keys tkKey = KeyConverter.ToOpenTKKey(key);
            return currentState.IsKeyPressed(tkKey);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
