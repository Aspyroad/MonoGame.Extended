using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended.ViewportAdapters;

namespace MonoGame.Extended.Input.InputListeners
{
    public class TouchGestureEventArgs : EventArgs
    {
        public TouchGestureEventArgs(ViewportAdapter viewportAdapter, TimeSpan time, GestureSample gesture)
        {
            ViewportAdapter = viewportAdapter;
            Gesture = gesture;
            Time = time;
        }

        public ViewportAdapter ViewportAdapter { get; }
        public GestureSample Gesture { get; }
        public TimeSpan Time { get; }

        public override bool Equals(object other)
        {
            var args = other as TouchGestureEventArgs;

            if (args == null)
                return false;

            return ReferenceEquals(this, args) || Gesture.Equals(args.Gesture);
        }

        public override int GetHashCode()
        {
            return Gesture.GetHashCode();
        }
    }
}