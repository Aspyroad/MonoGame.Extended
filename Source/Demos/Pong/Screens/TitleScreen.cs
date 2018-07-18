using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Pong.Screens
{
    public class TitleScreen : GameScreen
    {
        private readonly KeyboardService _keyboardService;
        private readonly MouseService _mouseService;
        private SpriteBatch _spriteBatch;
        private Texture2D _background;

        public TitleScreen(Game game, KeyboardService keyboardService, MouseService mouseService)
            : base(game)
        {
            _keyboardService = keyboardService;
            _mouseService = mouseService;
            game.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("title-screen");
        }

        public override void Update(GameTime gameTime)
        {
            if (_keyboardService.WasKeyJustDown(Keys.Escape))
                Game.Exit();

            if (_mouseService.LeftButton == ButtonState.Pressed || _keyboardService.WasAnyKeyJustDown())
                ScreenManager.LoadScreen(new PongGameScreen(Game, _keyboardService, _mouseService), new FadeTransition(GraphicsDevice, Color.Black, 0.5f));
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.End();
        }
    }
}