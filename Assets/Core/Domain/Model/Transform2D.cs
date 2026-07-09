using UnityEngine;

namespace Core.Domain
{
    public class Transform2D
    {
        public Vector2 Position = new();
        public Quaternion Rotation = new();
        public Vector2 Scale = new();

        public static Transform2D Default => new()
        { Position = Vector2.zero, Rotation = Quaternion.identity, Scale = Vector2.one };

        public static Transform2D operator +(Transform2D t, Transform2D t2)
        {
            var transform = Default;
            transform.Position = t.Position + t2.Position;
            transform.Scale = t.Scale + t2.Scale;
            transform.Rotation = t.Rotation * t2.Rotation;
            return transform;
        }
    }
}
