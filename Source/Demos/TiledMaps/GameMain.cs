using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
using MonoGame.Extended.Tiled.Serialization;
using Newtonsoft.Json;
using TiledMaps.Systems;

namespace TiledMaps
{
    public class MainGame : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphicsDeviceManager;
        private World _world;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            using (var reader = Content.OpenStream("test-map-1.tmx"))
            {
                var mapSerializer = new XmlSerializer(typeof(TiledMapContent));
                var map = (TiledMapContent) mapSerializer.Deserialize(reader);
                Console.WriteLine(JsonConvert.SerializeObject(map));
            }

            //var font = Content.Load<BitmapFont>("Sensation");
            var camera = new OrthographicCamera(GraphicsDevice) { Position = new Vector2(-400, 0) };
            var keyboardService = new KeyboardService();
            var mouseService = new MouseService();

            _world = new WorldBuilder()
                .AddSystem(new InputSystem(this, mouseService, keyboardService))
                .AddSystem(new MapRenderingSystem(Content, GraphicsDevice, camera))
                .AddSystem(new CameraSystem(camera, keyboardService, mouseService))
                .Build();
        }

        protected override void UnloadContent()
        {
            _world.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _world.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
