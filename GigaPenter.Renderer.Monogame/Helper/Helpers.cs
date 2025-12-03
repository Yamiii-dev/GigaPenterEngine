using System;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using GigaPenterEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GigaPenter.Renderer.Monogame.Helper;

public static class Helpers
{
    // operators for turning our Custom vectors into MonoGame vectors
    public static Vector2 ToMonoGame(this GigaPenterEngine.Core.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }

    public static Vector3 ToMonoGame(this GigaPenterEngine.Core.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
    // Functions for turning our Key Codes into MonoGame KeyCode
    public static class KeyMapper
    {
        // Define the conversions
        private static readonly Dictionary<Key, Keys> keyToMono = new()
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
            { Key.Tab, Keys.Tab }, { Key.Backspace, Keys.Back }, { Key.Delete, Keys.Delete },
            { Key.Insert, Keys.Insert }, { Key.Home, Keys.Home }, { Key.End, Keys.End },
            { Key.PageUp, Keys.PageUp }, { Key.PageDown, Keys.PageDown },
            { Key.Pause, Keys.Pause }, { Key.PrintScreen, Keys.PrintScreen },
    
            // Arrow keys
            { Key.Up, Keys.Up }, { Key.Down, Keys.Down }, { Key.Left, Keys.Left }, { Key.Right, Keys.Right },
    
            // Modifiers
            { Key.LeftShift, Keys.LeftShift }, { Key.RightShift, Keys.RightShift },
            { Key.LeftCtrl, Keys.LeftControl }, { Key.RightCtrl, Keys.RightControl },
            { Key.LeftAlt, Keys.LeftAlt }, { Key.RightAlt, Keys.RightAlt },
            { Key.CapsLock, Keys.CapsLock }, { Key.NumLock, Keys.NumLock }, { Key.ScrollLock, Keys.Scroll },
    
            // Symbols
            { Key.Minus, Keys.OemMinus }, { Key.Plus, Keys.OemPlus }, { Key.Equal, Keys.OemPlus },
            { Key.LeftBracket, Keys.OemOpenBrackets }, { Key.RightBracket, Keys.OemCloseBrackets },
            { Key.Backslash, Keys.OemBackslash }, { Key.Semicolon, Keys.OemSemicolon },
            { Key.Apostrophe, Keys.OemQuotes }, { Key.Comma, Keys.OemComma },
            { Key.Period, Keys.OemPeriod }, { Key.Slash, Keys.OemQuestion }, { Key.Grave, Keys.OemTilde }
        };

        public static Keys ToMonoKey(Key key)
        {
            if (keyToMono.TryGetValue(key, out var monoKey))
                return monoKey;

            throw new ArgumentException($"No mapping defined for {key}");
        }

        public static Key FromMonoKey(Keys key)
        {
            foreach (var pair in keyToMono)
            {
                if (pair.Value == key) return pair.Key;
            }
    
            throw new ArgumentException($"No mapping defined for {key}");
        }
}

}