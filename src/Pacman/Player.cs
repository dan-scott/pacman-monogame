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
        private Vector2 _currentDirection;
        private Vector2 _nextDirection;
        private Vector2 _startPos;
        public Vector2 Position { get; private set; }
        public Vector2 Destination { get; private set; }

        public Player(LevelGraph graph)
        {
            _graph = graph;
        }


        public void Reset(Vector2 startPos)
        {
            _currentDirection = Directions.Stopped;
            _nextDirection = Directions.Stopped;
            Position = startPos;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = Position * tileSizeVector;
            spriteBatch.DrawCircle(pos, 10, 10, Color.Yellow, 10);
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            MovePlayer(gameTime);
        }

        private void MovePlayer(GameTime gameTime)
        {
            if (_currentDirection == Directions.Stopped)
            {
                return;
            }

            if (_nextDirection == Vector2.Negate(_currentDirection))
            {
                _currentDirection = _nextDirection;
                var tmp = _startPos;
                _startPos = Destination;
                Destination = tmp;
            }

            var travelDistance = SPEED * gameTime.GetElapsedSeconds();

            var remaining = Vector2.Distance(Position, Destination);

            if (travelDistance >= remaining)
            {
                travelDistance -= remaining;

                Position = _startPos = Destination;

                if (_graph.IsPortal(Position, _currentDirection))
                {
                    Position = _startPos = _graph.PortalTo(Position, _currentDirection);
                }
                else if (_graph.HasAdjacent(Position, _nextDirection, true))
                {
                    _currentDirection = _nextDirection;
                } 
                else if (!_graph.HasAdjacent(Position, _currentDirection, true))
                {
                    _currentDirection = _nextDirection = Directions.Stopped;
                }

                Destination = _startPos + _currentDirection;

            }


            Position += _currentDirection * new Vector2(travelDistance);
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

            if (_currentDirection != Directions.Stopped) return;

            if (!_graph.HasAdjacent(Position, _nextDirection, true)) return;

            _currentDirection = _nextDirection;
            _startPos = Position;
            Destination = Position + _nextDirection;
        }

        public void LoadContent()
        {
        }

        public void UnloadContent()
        {
        }
    }
}