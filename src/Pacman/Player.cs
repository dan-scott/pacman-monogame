using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Pacman
{
    public class Player
    {
        private readonly LevelGraph _graph;
        private const float SPEED = 8f;
        private Vector2 _nextDirection;
        private Vector2 _startPos;
        private readonly SpriteFont _font;
        public Vector2 Position { get; private set; }
        public Vector2 NextNode { get; private set; }
        public Vector2 Direction { get; private set; }

        public Player(LevelGraph graph)
        {
            _graph = graph;

            _font = GameServices.GetService<ContentManager>().Load<SpriteFont>("Fonts/Arial");
        }


        public void Reset(Vector2 startPos)
        {
            Direction = Directions.Stopped;
            _nextDirection = Directions.Stopped;
            Position = NextNode = startPos;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = Position * tileSizeVector;
            spriteBatch.DrawCircle(pos, 10, 10, Color.Yellow, 10);
            spriteBatch.DrawString(_font, NextNode.ToString(), new Vector2(-100, -20), Color.Red);
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            MovePlayer(gameTime);
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