using UnityEngine;
using UnityEngine.UIElements;

namespace Input
{
    public class DragEvent : EventBase<DragEvent>
    {
        public Vector2 LocalPosition { get; private set; }

        protected override void Init()
        {
            base.Init();
            LocalPosition = Vector2.zero;
            bubbles = true;
        }

        public static DragEvent GetPooled(Vector2 localPosition)
        {
            var e = GetPooled();
            e.LocalPosition = localPosition;
            return e;
        }
    }
}