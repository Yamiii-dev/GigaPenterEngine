namespace GigaPenterEngine.Core;

public class Vector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 operator /(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X / b.X, a.Y / b.Y);
    }

    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.X * b, a.Y * b);
    }

    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(a.X / b, a.Y / b);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

public class Vector3 : Vector2
{
    public float Z { get; set; }
    public Vector3(float x, float y, float z) : base(x, y)
    {
        Z = z;
    }

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3 operator *(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }

    public static Vector3 operator /(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }

    public static Vector3 operator *(Vector3 a, float b)
    {
        return new Vector3(a.X * b, a.Y * b, a.Z * b);
    }

    public static Vector3 operator /(Vector3 a, float b)
    {
        return new Vector3(a.X / b, a.Y / b, a.Z / b);
    }
    
    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}