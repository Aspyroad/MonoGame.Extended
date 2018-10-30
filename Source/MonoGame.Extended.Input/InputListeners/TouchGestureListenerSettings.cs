using MonoGame.Extended.ViewportAdapters;

namespace MonoGame.Extended.Input.InputListeners
{
    public class TouchGestureListenerSettings : InputListenerSettings<TouchGestureListener>
    {
        public TouchGestureListenerSettings()
        {
            
        }

        public ViewportAdapter ViewportAdapter { get; set; }

        public override TouchGestureListener CreateListener()
        {
            return new TouchGestureListener(this);
        }
    }
}