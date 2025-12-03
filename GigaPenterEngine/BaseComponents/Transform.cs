using GigaPenterEngine.Core;

namespace GigaPenterEngine.BaseComponents;

public class Transform : Component
{
    public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
    public Vector2 Scale { get; set; } = new Vector2(1, 1);
    public float Rotation { get; set; } = 0f;
}