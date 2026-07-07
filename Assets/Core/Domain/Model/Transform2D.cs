using System.Numerics;

namespace Core.Domain
{
    public class Transform2D
    {
        public Vector2 Position = new();
        public Quaternion Rotation = new();
        public Vector2 Scale = new();

        public static Transform2D Default => new()
        { Position = Vector2.Zero, Rotation = Quaternion.Identity, Scale = Vector2.One };
    }
}
