using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Entities;
using Sandbox.Systems;

namespace Sandbox
{
    public interface IContentLoader
    {
        string[] Extensions { get; }
        void Unload(object content);
        object Load(string filePath);
    }

    public abstract class ContentLoader<T> : IContentLoader
    {
        protected ContentLoader(params string[] extensions)
        {
            Extensions = extensions;
        }

        public string[] Extensions { get; }
        public abstract T Load(string filePath);
        public abstract void Unload(T content);
        void IContentLoader.Unload(object content) => Unload((T) content);
        object IContentLoader.Load(string filePath) => Load(filePath);
    }

    public class Texture2DContentLoader : ContentLoader<Texture2D>
    {
        private readonly GraphicsDevice _graphicsDevice;

        public Texture2DContentLoader(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public override Texture2D Load(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return Texture2D.FromStream(_graphicsDevice, stream);
            }
        }

        public override void Unload(Texture2D content)
        {
            content.Dispose();
        }
    }

    public class ContentWatcher
    {
        private readonly string _contentPath;

        public ContentWatcher(string contentPath)
        {
            _contentPath = contentPath;
        }

        private readonly Dictionary<string, IContentLoader> _contentLoadersByExtension = new Dictionary<string, IContentLoader>();

        public void RegisterContentLoader(IContentLoader loader)
        {
            foreach (var extension in loader.Extensions)
                _contentLoadersByExtension.Add(extension, loader);
        }

        public T Load<T>(string filename) where T : class 
        {
            var extension = Path.GetExtension(filename);
            var filePath = Path.Combine(_contentPath, filename);
            
            if (_contentLoadersByExtension.TryGetValue(extension, out var loader))
                return loader.Load(filePath) as T;

            return null;
        }

        public void Watch()
        {
            var watcher = new FileSystemWatcher(_contentPath)
            {
                Filter = "*.*",
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
            };
            watcher.Changed += (sender, args) =>
            {
            };
            watcher.EnableRaisingEvents = true;
        }
    }

    public class MainGame : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private World _world;
        private Texture2D _texture;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            var font = Content.Load<BitmapFont>("Sensation");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _world = new WorldBuilder()
                .AddSystem(new RainfallSystem())
                .AddSystem(new ExpirySystem())
                .AddSystem(new RenderSystem(GraphicsDevice))
                .AddSystem(new HudSystem(GraphicsDevice, font))
                .Build();

            _texture = Content.Load<Texture2D>("ballGrey");

        }

        protected override void UnloadContent()
        {
            _spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //_world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //_world.Draw(gameTime);

            GraphicsDevice.Clear(Color.DarkBlue);

            _spriteBatch.Begin(transformMatrix: Matrix.CreateScale(8), samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);
            _spriteBatch.Draw(_texture, new Vector2(10, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
