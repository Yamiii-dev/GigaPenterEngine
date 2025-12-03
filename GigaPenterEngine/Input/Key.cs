namespace GigaPenterEngine.Input;

// Universal KeyCode enum, used for interfacing with an Input Handler (usually the one built into a Renderer)
public enum Key
{
    // Letters
    A, B, C, D, E, F, G,
    H, I, J, K, L, M, N,
    O, P, Q, R, S, T, U,
    V, W, X, Y, Z,

    // Numbers
    Num0, Num1, Num2, Num3, Num4,
    Num5, Num6, Num7, Num8, Num9,

    // Function keys
    F1, F2, F3, F4, F5, F6,
    F7, F8, F9, F10, F11, F12,

    // Control keys
    Escape,
    Enter,
    Space,
    Tab,
    Backspace,
    Delete,
    Insert,
    Home,
    End,
    PageUp,
    PageDown,
    Pause,
    PrintScreen,

    // Arrow keys
    Up,
    Down,
    Left,
    Right,

    // Modifiers
    LeftShift,
    RightShift,
    LeftCtrl,
    RightCtrl,
    LeftAlt,
    RightAlt,
    CapsLock,
    NumLock,
    ScrollLock,

    // Symbols
    Minus,          // -
    Plus,           // +
    Equal,          // =
    LeftBracket,    // [
    RightBracket,   // ]
    Backslash,      // \
    Semicolon,      // ;
    Apostrophe,     // '
    Comma,          // ,
    Period,         // .
    Slash,          // /
    Grave,          // `
}