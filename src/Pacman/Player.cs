using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace Pacman
{
    public class Player
    {
        private static readonly List<Rectangle> Frames = new List<Rectangle>
        {
            new Rectangle(0, 0, 32, 32),
            new Rectangle(32, 0, 32, 32),
            new Rectangle(64, 0, 32, 32),
            new Rectangle(96, 0, 32, 32),
            new Rectangle(128, 0, 32, 32),
            new Rectangle(96, 0, 32, 32),
            new Rectangle(64, 0, 32, 32),
            new Rectangle(32, 0, 32, 32),
            new Rectangle(0, 0, 32, 32),
        };

        private readonly LevelGraph _graph;
        private const float SPEED = 8f;
        private Vector2 _nextDirection;
        private Vector2 _startPos;
        private readonly SpriteFont _font;
        private readonly AnimatedSprite _pacmanSprite;

        public const int RADIUS = 10;

        public Vector2 Position { get; private set; }
        public Vector2 NextNode { get; private set; }
        public Vector2 Direction { get; private set; }

        public Player(LevelGraph graph)
        {
            _graph = graph;

            var cm = GameServices.GetService<ContentManager>();

            _font = cm.Load<SpriteFont>("Fonts/Arial");

            _pacmanSprite = new AnimatedSprite("pacman", Frames, 0.05f);
        }


        public void Reset(Vector2 startPos)
        {
            Direction = Directions.Stopped;
            _nextDirection = Directions.Stopped;
            Position = NextNode = startPos;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var translation = Globals.DefaultTranslation;

            var rotation = 0f;

            if (Direction == Directions.Down)
            {
                rotation = 90;
            }
            else if (Direction == Directions.Left)
            {
                rotation = 180;
            }
            else if (Direction == Directions.Up)
            {
                rotation = 270;
            }

            if (rotation > 0)
            {
                translation = Matrix.CreateTranslation(-16, -16, 0);
                translation *= Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
                translation *= Matrix.CreateTranslation(16, 16, 0);
                translation *= Globals.DefaultTranslation;
            }


            var pos = Position * tileSizeVector - new Vector2(16);

            translation *= Matrix.CreateTranslation(pos.X, pos.Y, 0);

            spriteBatch.Begin(transformMatrix: translation);


            _pacmanSprite.Draw(spriteBatch, Vector2.Zero);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.DrawString(_font, NextNode.ToString(), new Vector2(10, 20), Color.Red);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            MovePlayer(gameTime);

            if (Direction != Directions.Stopped)
            {
                _pacmanSprite.Update(gameTime);
            }
        }

        private void MovePlayer(GameTime gameTime)
        {
            if (Direction == Directions.Stopped)
            {
                return;
            }

            if (_nextDirection == Vector2.Negate(Direction))
            {
                Direction = _nextDirection;
                var tmp = _startPos;
                _startPos = NextNode;
                NextNode = tmp;
            }

            var travelDistance = SPEED * gameTime.GetElapsedSeconds();

            var remaining = Vector2.Distance(Position, NextNode);

            if (travelDistance >= remaining)
            {
                travelDistance -= remaining;

                Position = _startPos = NextNode;

                if (_graph.IsPortal(Position, Direction))
                {
                    Position = _startPos = _graph.PortalTo(Position, Direction);
                }
                else if (_graph.HasAdjacent(Position, _nextDirection))
                {
                    Direction = _nextDirection;
                }
                else if (!_graph.HasAdjacent(Position, Direction))
                {
                    Direction = _nextDirection = Directions.Stopped;
                }

                NextNode = _startPos + Direction;

            }


            Position += Direction * new Vector2(travelDistance);
        }

        private void HandleInput()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                _nextDirection = Directions.Down;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                _nextDirection = Directions.Left;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                _nextDirection = Directions.Up;
            }

            if (state.IsKeyDown(Keys.Right))
            {
                _nextDirection = Directions.Right;
            }

            if (Direction != Directions.Stopped) return;

            if (!_graph.HasAdjacent(Position, _nextDirection)) return;

            Direction = _nextDirection;
            _startPos = Position;
            NextNode = Position + _nextDirection;
        }

        public void LoadContent()
        {
        }

        public void UnloadContent()
        {
        }
    }
}