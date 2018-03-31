using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

            GameServices.AddService(Content);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameLevel = new GameLevel(20);

            _gameLevel.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            _gameLevel.UnloadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(140, 60, 0));

            _gameLevel.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _gameLevel.Update(gameTime);
        }
    }
}