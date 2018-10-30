using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended.ViewportAdapters;

namespace MonoGame.Extended.Input.InputListeners
{
    public class TouchListener : InputListener
    {
        TouchLocation startLocation;
        TouchLocation prevMoveLocation;

        public TouchListener() : this(new TouchListenerSettings())
        {
            Initiate();
        }

        public TouchListener(ViewportAdapter viewportAdapter) : this(new TouchListenerSettings())
        {
            Initiate();
            ViewportAdapter = viewportAdapter;
        }

        public TouchListener(TouchListenerSettings settings)
        {
            Initiate();
            ViewportAdapter = settings.ViewportAdapter;
        }

        public ViewportAdapter ViewportAdapter { get; set; }

        public event EventHandler<TouchEventArgs> TouchStarted;
        public event EventHandler<TouchEventArgs> TouchEnded;
        public event EventHandler<TouchEventArgs> TouchMoved;
        public event EventHandler<TouchEventArgs> TouchCancelled;

        private void Initiate()
        {
            startLocation = new TouchLocation();
            prevMoveLocation = new TouchLocation();
        }

        public override void Update(GameTime gameTime)
        {
            var touchCollection = TouchPanel.GetState();

            foreach (var touchLocation in touchCollection)
            {
                switch (touchLocation.State)
                {
                    case TouchLocationState.Pressed:
                        {
                            startLocation = touchLocation;
                            TouchStarted?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation, startLocation));
                            break;
                        }
                    case TouchLocationState.Moved:
                        {
                            // AspyRoad mod - 01-06-2018
                            // This is used to help minimise calls to TouchMoved as many TouchMoved events are the same position.
                            // This is an artifect of the MonoGames update calls, and the touch que. I found it made touch smoothe with less
                            // calls using the same position, as they wernt really moves
                            if (!touchLocation.Position.Equals(prevMoveLocation.Position))
                            {
                                TouchMoved?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation, startLocation));
                            }
                            this.prevMoveLocation = touchLocation;

                            break;
                        }
                    case TouchLocationState.Released:
                        {
                            TouchEnded?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation, startLocation));
                            break;
                        }
                    case TouchLocationState.Invalid:
                        {
                            TouchCancelled?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation, startLocation));
                            break;
                        }
                }
            }
        }
    }
}