using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;

namespace MonoGame.Extended.NuclexGui.Input
{
    public class DefaultInputCapturer : IInputCapturer, IDisposable
    {
        private bool _disposedValue; // To detect redundant calls
        private readonly GamePadListener _gamePadListener;

        /// <summary>Current receiver of input events</summary>
        /// <remarks>
        ///     Always valid. If no input receiver is assigned, this field will be set
        ///     to a dummy receiver.
        /// </remarks>
        private IInputReceiver _inputReceiver;

        /// <summary>Input service the capturer is currently subscribed to</summary>
        private IGuiInputService _inputService;

        private readonly KeyboardListener _keyboardListener;
        private readonly MouseListener _mouseListener;
        private readonly TouchListener _touchListener;
        private readonly TouchGestureListener _touchGestureListener;

        /// <summary>Initializes a new input capturer, taking the input service from a service provider</summary>
        /// <param name="serviceProvider">Service provider the input capturer will take the input service from</param>
        public DefaultInputCapturer(IServiceProvider serviceProvider) : this(GetInputService(serviceProvider))
        {
        }

        /// <summary>Initializes a new input capturer using the specified input service</summary>
        /// <param name="inputService">Input service the capturer will subscribe to</param>
        public DefaultInputCapturer(IGuiInputService inputService)
        {
            _inputService = inputService;
            _inputReceiver = new DummyInputReceiver();

            _keyboardListener = inputService.KeyboardListener;
            _mouseListener = inputService.MouseListener;
            _gamePadListener = inputService.GamePadListener;
            _touchListener = inputService.TouchListener;
            _touchGestureListener = inputService.TouchGestureListener;

            SubscribeInputDevices();
        }

        // ~MainInputCapturer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        /// <summary>Input receiver any captured input will be sent to</summary>
        public IInputReceiver InputReceiver
        {
            get
            {
                if (ReferenceEquals(_inputReceiver, DummyInputReceiver.Default))
                    return null;
                return _inputReceiver;
            }
            set
            {
                if (value == null)
                    _inputReceiver = DummyInputReceiver.Default;
                else
                    _inputReceiver = value;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_inputService != null)
                    {
                        UnsubscribeInputDevices();
                        _inputService = null;
                    }
                }

                _disposedValue = true;
            }
        }

        private void SubscribeInputDevices()
        {
            _keyboardListener.KeyPressed += KeyboardListener_KeyPressed;
            _keyboardListener.KeyReleased += KeyboardListener_KeyReleased;
            _keyboardListener.KeyTyped += KeyboardListener_KeyTyped;

            _mouseListener.MouseDown += MouseListener_MouseDown;
            _mouseListener.MouseUp += MouseListener_MouseUp;
            _mouseListener.MouseMoved += MouseListener_MouseMoved;
            _mouseListener.MouseWheelMoved += MouseListener_MouseWheelMoved;

            _gamePadListener.ButtonDown += GamePadListener_ButtonDown;
            _gamePadListener.ButtonUp += GamePadListener_ButtonUp;

            _touchListener.TouchStarted += TouchListener_TouchStarted;
            _touchListener.TouchMoved += TouchListener_TouchMoved;
            _touchListener.TouchEnded += TouchListener_TouchEnded;
            //_touchListener.TouchCancelled += TouchListener_TouchCancelled;

            _touchGestureListener.Tap += TouchGestureListener_Tap;
            //_touchGestureListener.Hold += TouchGestureListener_Hold;
        }

        private void UnsubscribeInputDevices()
        {
            _keyboardListener.KeyPressed -= KeyboardListener_KeyPressed;
            _keyboardListener.KeyReleased -= KeyboardListener_KeyReleased;
            _keyboardListener.KeyTyped -= KeyboardListener_KeyTyped;

            _mouseListener.MouseDown -= MouseListener_MouseDown;
            _mouseListener.MouseUp -= MouseListener_MouseUp;
            _mouseListener.MouseMoved -= MouseListener_MouseMoved;
            _mouseListener.MouseWheelMoved -= MouseListener_MouseWheelMoved;

            _gamePadListener.ButtonDown -= GamePadListener_ButtonDown;
            _gamePadListener.ButtonUp -= GamePadListener_ButtonUp;

            _touchListener.TouchStarted -= TouchListener_TouchStarted;
            _touchListener.TouchMoved -= TouchListener_TouchMoved;
            _touchListener.TouchEnded -= TouchListener_TouchEnded;
            //_touchListener.TouchCancelled -= TouchListener_TouchCancelled;

            _touchGestureListener.Tap -= TouchGestureListener_Tap;
            //_touchGestureListener.Hold -= TouchGestureListener_Hold;
        }

        private void KeyboardListener_KeyPressed(object sender, KeyboardEventArgs e)
        {
            _inputReceiver.InjectKeyPress(e.Key);
        }

        private void KeyboardListener_KeyReleased(object sender, KeyboardEventArgs e)
        {
            _inputReceiver.InjectKeyRelease(e.Key);
        }

        private void KeyboardListener_KeyTyped(object sender, KeyboardEventArgs e)
        {
            _inputReceiver.InjectCharacter(e.Character.GetValueOrDefault());
        }

        private void MouseListener_MouseDown(object sender, MouseEventArgs e)
        {
            _inputReceiver.InjectMousePress(e.Button);
        }

        private void MouseListener_MouseUp(object sender, MouseEventArgs e)
        {
            _inputReceiver.InjectMouseRelease(e.Button);
        }

        private void MouseListener_MouseMoved(object sender, MouseEventArgs e)
        {
            _inputReceiver.InjectMouseMove(e.Position.X, e.Position.Y);
        }

        private void MouseListener_MouseWheelMoved(object sender, MouseEventArgs e)
        {
            _inputReceiver.InjectMouseWheel(e.ScrollWheelDelta);
        }

        private void GamePadListener_ButtonDown(object sender, GamePadEventArgs e)
        {
            if ((e.Button & Buttons.DPadUp) != 0)
                _inputReceiver.InjectCommand(Command.Up);
            else
            {
                if ((e.Button & Buttons.DPadDown) != 0)
                    _inputReceiver.InjectCommand(Command.Down);
                else
                {
                    if ((e.Button & Buttons.DPadLeft) != 0)
                        _inputReceiver.InjectCommand(Command.Left);
                    else
                    {
                        if ((e.Button & Buttons.DPadRight) != 0)
                            _inputReceiver.InjectCommand(Command.Right);
                        else
                            _inputReceiver.InjectButtonPress(e.Button);
                    }
                }
            }
        }

        private void GamePadListener_ButtonUp(object sender, GamePadEventArgs e)
        {
            _inputReceiver.InjectButtonRelease(e.Button);
        }

        private void TouchListener_TouchStarted(object sender, TouchEventArgs e)
        {
            //Debug.WriteLine("Started - x = " + e.Position.X.ToString() + " y = " + e.Position.Y.ToString());
            _inputReceiver.InjectTouchStarted(e.StartPosition.X, e.StartPosition.Y);
        }

        private void TouchListener_TouchMoved(object sender, TouchEventArgs e)
        {
            //Debug.WriteLine("Moved - x = " + e.Position.X.ToString() + " y = " + e.Position.Y.ToString());
            _inputReceiver.InjectTouchMoved(e.Position.X, e.Position.Y);
        }

        private void TouchListener_TouchEnded(object sender, TouchEventArgs e)
        {
            //Debug.WriteLine("Ended - x = " + e.Position.X.ToString() + " y = " + e.Position.Y.ToString());
            _inputReceiver.InjectTouchEnded(e.Position.X, e.Position.Y);
        }

        private void TouchListener_TouchCancelled(object sender, TouchEventArgs e)
        {
        }

        private void TouchGestureListener_Tap(object source, TouchGestureEventArgs e)
        {
            // Simulate a mouse left button press/release
            // First inject a move event to place the control in activated state
            // If there was no mouse move first (very possible with a tap gesture)
            // We must place the tablets (virtual) cursor over the control
            _inputReceiver.InjectMouseMove(e.Gesture.Position.X, e.Gesture.Position.Y);
            // Press AND release, there are (obviously) no gestures for a Tap release.
            // This is all one process
            _inputReceiver.InjectMousePress(MouseButton.Left);
            _inputReceiver.InjectMouseRelease(MouseButton.Left);

        }

        private void TouchGestureListener_Hold(object source, TouchGestureEventArgs e)
        {
        }

        /// <summary>Retrieves the input service from a service provider</summary>
        /// <param name="serviceProvider">
        ///     Service provider the service is taken from
        /// </param>
        /// <returns>The input service stored in the service provider</returns>
        private static IGuiInputService GetInputService(IServiceProvider serviceProvider)
        {
            var inputService = (IGuiInputService) serviceProvider.GetService(typeof(IGuiInputService));

            if (inputService == null)
            {
                throw new InvalidOperationException(
                    "Using the GUI with the DefaultInputCapturer requires the IInputService. " +
                    "Please either add the IInputService to Game.Services by using the " +
                    "Nuclex.Input.InputManager in your game or provide a custom IInputCapturer " +
                    "implementation for the GUI and assign it before GuiManager.Initialize() " +
                    "is called."
                );
            }

            return inputService;
        }

        /// <summary>Dummy receiver for input events</summary>
        private class DummyInputReceiver : IInputReceiver
        {
            /// <summary>Default instance of the dummy receiver</summary>
            public static readonly DummyInputReceiver Default = new DummyInputReceiver();

            /// <summary>Injects an input command into the input receiver</summary>
            /// <param name="command">Input command to be injected</param>
            public void InjectCommand(Command command)
            {
            }

            /// <summary>Called when a button on the gamepad has been pressed</summary>
            /// <param name="button">Button that has been pressed</param>
            public void InjectButtonPress(Buttons button)
            {
            }

            /// <summary>Called when a button on the gamepad has been released</summary>
            /// <param name="button">Button that has been released</param>
            public void InjectButtonRelease(Buttons button)
            {
            }

            /// <summary>Injects a mouse position update into the receiver</summary>
            /// <param name="x">New X coordinate of the mouse cursor on the screen</param>
            /// <param name="y">New Y coordinate of the mouse cursor on the screen</param>
            public void InjectMouseMove(float x, float y)
            {
            }

            /// <summary>Called when a mouse button has been pressed down</summary>
            /// <param name="button">Index of the button that has been pressed</param>
            public void InjectMousePress(MouseButton button)
            {
            }

            /// <summary>Called when a mouse button has been released again</summary>
            /// <param name="button">Index of the button that has been released</param>
            public void InjectMouseRelease(MouseButton button)
            {
            }

            /// <summary>Called when the mouse wheel has been rotated</summary>
            /// <param name="ticks">Number of ticks that the mouse wheel has been rotated</param>
            public void InjectMouseWheel(float ticks)
            {
            }

            /// <summary>Called when a key on the keyboard has been pressed down</summary>
            /// <param name="keyCode">Code of the key that was pressed</param>
            public void InjectKeyPress(Keys keyCode)
            {
            }

            /// <summary>Called when a key on the keyboard has been released again</summary>
            /// <param name="keyCode">Code of the key that was released</param>
            public void InjectKeyRelease(Keys keyCode)
            {
            }

            /// <summary>Handle user text input by a physical or virtual keyboard</summary>
            /// <param name="character">Character that has been entered</param>
            public void InjectCharacter(char character)
            {
            }

            /// <summary>Injects a TouchStart position update into the receiver</summary>
            /// <param name="x">New X coordinate of the TouchStart point on the screen</param>
            /// <param name="y">New Y coordinate of the TouchStart point on the screen</param>
            public void InjectTouchStarted(float x, float y)
            {
            }

            /// <summary>Injects a TouchMoved position update into the receiver</summary>
            /// <param name="x">New X coordinate of the TouchMoved point on the screen</param>
            /// <param name="y">New Y coordinate of the TouchMoved point on the screen</param>
            public void InjectTouchMoved(float x, float y)
            {
            }

            /// <summary>Injects a TouchEnded position update into the receiver</summary>
            /// <param name="x">New X coordinate of the TouchEnded point on the screen</param>
            /// <param name="y">New Y coordinate of the TouchEnded point on the screen</param>
            public void InjectTouchEnded(float x, float y)
            {
            }

            /// <summary>Called when a TapGesture is recognised</summary>
            /// <param name="gesture">TapGesture object returned by MonoGame/iOS</param>
            public void InjectTapGesture(GestureSample gesture)
            {
            }

            /// <summary>Called when a TapGesture is recognised</summary>
            /// <param name="gesture">TapGesture object returned by MonoGame/iOS</param>
            public void InjectHoldGesture(GestureSample gesture)
            {
            }
        }
    }
}