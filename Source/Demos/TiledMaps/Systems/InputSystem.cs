using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;

namespace TiledMaps.Systems
{
    public class InputSystem : UpdateSystem
    {
        private readonly Game _game;
        private readonly MouseService _mouseService;
        private readonly KeyboardService _keyboardService;

        public InputSystem(Game game, MouseService mouseService, KeyboardService keyboardService)
        {
            _game = game;
            _mouseService = mouseService;
            _keyboardService = keyboardService;
        }

        public override void Update(GameTime gameTime)
        {
            _mouseService.Update(gameTime);
            _keyboardService.Update(gameTime);

            if(_keyboardService.IsKeyDown(Keys.Escape))
                _game.Exit();
        }
    }
}