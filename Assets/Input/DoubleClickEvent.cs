using System.Numerics;
using UnityEngine.UIElements;

namespace Input
{
    public class DoubleClickEvent : EventBase<DoubleClickEvent>
    {
        public static DoubleClickEvent GetPooled(Vector2 position)
        {
            var e = GetPooled();
            return e;
        }
    }
}