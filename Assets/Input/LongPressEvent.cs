using UnityEngine;
using UnityEngine.UIElements;

namespace Input
{
    public class LongPressEventStarted : EventBase<LongPressEventStarted>
    {
        public Vector2 LocalPosition { get; private set; }

        protected override void Init()
        {
            base.Init();
            LocalPosition = Vector2.zero;
            bubbles = true;
        }

        public static LongPressEventStarted GetPooled(Vector2 position)
        {
            var e = GetPooled();
            return e;
        }
    }

    public class LongPressEventEnded : EventBase<LongPressEventEnded>
    {
        public Vector2 LocalPosition { get; private set; }

        protected override void Init()
        {
            base.Init();
            LocalPosition = Vector2.zero;
            bubbles = true;
        }

        public static LongPressEventEnded GetPooled(Vector2 position)
        {
            var e = GetPooled();
            e.LocalPosition = position;
            return e;
        }
    }
}