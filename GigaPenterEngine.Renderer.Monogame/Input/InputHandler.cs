using GigaPenterEngine.Input;
using GigaPenterEngine.Renderer.Monogame.Helper;
using Microsoft.Xna.Framework.Input;

namespace GigaPenterEngine.Renderer.Monogame.Input;

public static class InputHandler
{
    internal static KeyboardState keyboard;
    internal static MouseState mouse;
    internal static GamePadState gamePad;
    
    internal static KeyboardState lastKeyboardState;
    internal static MouseState lastMouseState;
    internal static GamePadState lastGamePadState;
    
    // Is true at the first frame of a key press
    public static bool KeyDown(Key key)
    {
        return keyboard.IsKeyDown(Helpers.KeyMapper.ToMonoKey(key)) && lastKeyboardState.IsKeyUp(Helpers.KeyMapper.ToMonoKey(key));
    }

    // Is true after a key gets lifted
    public static bool KeyUp(Key key)
    {
        return keyboard.IsKeyUp(Helpers.KeyMapper.ToMonoKey(key));
    }

    // Is true if a key is down
    public static bool KeyPressed(Key key)
    {
        return keyboard.IsKeyDown(Helpers.KeyMapper.ToMonoKey(key));
    }
}