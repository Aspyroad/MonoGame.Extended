using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended.ViewportAdapters;

namespace MonoGame.Extended.Input.InputListeners
{
    public delegate void TapEventHandler(object source, TouchGestureEventArgs e);
    public delegate void DragCompleteEventHandler(object source, TouchGestureEventArgs e);
    public delegate void FlickEventHandler(object source, TouchGestureEventArgs e);
    public delegate void FreeDragEventHandler(object source, TouchGestureEventArgs e);
    public delegate void HoldEventHandler(object source, TouchGestureEventArgs e);
    public delegate void HorizonatlDragEventHandler(object source, TouchGestureEventArgs e);
    public delegate void PinchEventHandler(object source, TouchGestureEventArgs e);
    public delegate void PinchCompleteEventHandler(object source, TouchGestureEventArgs e);
    public delegate void VerticalDragEventHandler(object source, TouchGestureEventArgs e);

    /// <summary>
    ///     Handles touchj gesture detection from Monogame.
    /// </summary>
    /// <remarks>
    ///     An event based system for tracking input gesture recognition from Monogame.
    ///     Notes : Monogame uses a queing system for gesture recognition.
    ///     It is important to remember Monogame is doing the gesture recognition, not the OS.
    ///     You must rely upon update calls to check for gestures, this can change the behaviour 
    ///     of longer/slower drag gestures for example, and this must be handled differently by your
    ///     game code/gesture recognition routines. When we subscribe to a gesture event, it also enables
    ///     monogames recognition of the gesture, as all gestures are disabled by default.
    ///     Unsubscribing to the gesture event, disables the gestures recognition in monogame.
    ///     This keeps gesture recognition overhead down. 
    ///     
    ///     Due to nature of the listener, even when game is not in focus, listener will continue to be updated.
    ///     To avoid that, manual pause of Update() method is required whenever game loses focus.
    ///     To avoid having to do it manually, register listener to <see cref="InputListenerComponent" />
    /// </remarks>

    public class TouchGestureListener : InputListener
    {
        object lockMe = new object();
        event TapEventHandler _tapEventHandler;
        event DragCompleteEventHandler _dragCompleteEventHandler;
        event FlickEventHandler _flickEventHandler;
        event FreeDragEventHandler _freeDragEventHandler;
        event HoldEventHandler _holdEventHandler;
        event HorizonatlDragEventHandler _horizonatlDragEventHandler;
        event PinchEventHandler _pinchEventHandler;
        event PinchCompleteEventHandler _pinchCompleteEventHandler;
        event VerticalDragEventHandler _verticalDragEventHandler;

        public TouchGestureListener() : this(new TouchGestureListenerSettings())
        {
        }

        public TouchGestureListener(ViewportAdapter viewportAdapter) : this(new TouchGestureListenerSettings())
        {
            ViewportAdapter = viewportAdapter;
        }

        public TouchGestureListener(TouchGestureListenerSettings settings)
        {
            ViewportAdapter = settings.ViewportAdapter;
        }

        public ViewportAdapter ViewportAdapter { get; set; }

        public event TapEventHandler Tap
        {
            add
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.Tap; 
                    _tapEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~ GestureType.Tap;
                    _tapEventHandler -= value;
                }
            }
        }

        public event DragCompleteEventHandler DragComplete
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.DragComplete;
                    _dragCompleteEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.DragComplete;
                    _dragCompleteEventHandler -= value;
                }
            }
        }

        public event FlickEventHandler Flick
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.Flick;
                    _flickEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.Flick;
                    _flickEventHandler -= value;
                }
            }
        }

        public event FreeDragEventHandler FreeDrag
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.FreeDrag;
                    _freeDragEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.FreeDrag;
                    _freeDragEventHandler = value;
                }
            }
        }

        public event HoldEventHandler Hold
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.Hold;
                    _holdEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.Hold;
                    _holdEventHandler -= value;
                }
            }
        }

        public event  HorizonatlDragEventHandler HorizontalDrag
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.HorizontalDrag;
                    _horizonatlDragEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.HorizontalDrag;
                    _horizonatlDragEventHandler -= value;
                }
            }
        }

        public event PinchEventHandler Pinch
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.Pinch;
                    _pinchEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.Pinch;
                    _pinchEventHandler -= value;
                }
            }
        }

        public event PinchCompleteEventHandler PinchComplete
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.PinchComplete;
                    _pinchCompleteEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.PinchComplete;
                    _pinchCompleteEventHandler -= value;
                }
            }
        }

        public event VerticalDragEventHandler VerticalDrag
        {
            add
            {
                lock (lockMe)
                {
                    // Set the gesture enabled in TouchPanel.
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures | GestureType.VerticalDrag;
                    _verticalDragEventHandler += value;
                }
            }
            remove
            {
                lock (lockMe)
                {
                    TouchPanel.EnabledGestures = TouchPanel.EnabledGestures & ~GestureType.VerticalDrag;
                    _verticalDragEventHandler -= value;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Loop over all gestures in the Que. 
            // There may be more than one per frame due to the OS dropping frames etc
            while (TouchPanel.IsGestureAvailable)
            {
                
                var gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.Tap:
                        _tapEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.DragComplete:
                        _dragCompleteEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.Flick:
                        _flickEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.FreeDrag:
                        _freeDragEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.Hold:
                        _holdEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.HorizontalDrag:
                        _horizonatlDragEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.Pinch:
                        _pinchEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.PinchComplete:
                        _freeDragEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;
                    case GestureType.VerticalDrag:
                        _freeDragEventHandler?.Invoke(this, new TouchGestureEventArgs(ViewportAdapter, gameTime.TotalGameTime, gesture));
                        break;

                }
            }
        }
    }
}