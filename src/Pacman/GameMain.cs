using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class GameMain : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private GameLevel _gameLevel;
        private SpriteBatch _spriteBatch;

        public GameMain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _gameLevel = new GameLevel(20);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(140, 60, 0));

            _gameLevel.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();
        }
    }
}