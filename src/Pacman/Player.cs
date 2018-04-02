using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Pacman
{
    public class Player
    {
        private const float PLAYER_SPEED = 8f;
        private Vector2 _currentDirection;
        private Vector2 _nextDirection;
        private Vector2 _position;
        private PathEdge _currentEdge;

        public PathNode NextNode => _currentEdge.End;
        public Vector2 Position => _position;

        public void Reset(Vector2 startPos, PathEdge currentEdge)
        {
            _currentDirection = Directions.Stopped;
            _nextDirection = Directions.Stopped;
            _position = startPos;
            _currentEdge = currentEdge;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = _position * tileSizeVector;
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
                _currentEdge = _currentEdge.End[_currentDirection];
            }

            var travelDistance = PLAYER_SPEED * gameTime.GetElapsedSeconds();

            var distanceToNextNode = Vector2.Distance(_currentEdge.End.Position, _position);

            if (distanceToNextNode <= travelDistance)
            {
                travelDistance -= distanceToNextNode;

                PathEdge nextEdge;

                if ((nextEdge = _currentEdge.End[_nextDirection]) != null)
                {
                    _position = nextEdge.Start.Position;
                    _currentDirection = _nextDirection;
                    _currentEdge = nextEdge;
                }
                else if ((nextEdge = _currentEdge.End[_currentDirection]) != null)
                {
                    _position = nextEdge.Start.Position;
                    _currentEdge = nextEdge;
                }
                else
                {
                    _position = _currentEdge.End.Position;
                    travelDistance = 0;
                }
            }

            if (_currentEdge.IsPortal)
            {
                _currentEdge = _currentEdge.End[_currentDirection];
                _position = _currentEdge.Start.Position;
            }

            _position += _currentDirection * new Vector2(travelDistance);
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

            if (_currentDirection == Directions.Stopped)
            {
                var currentNode = _currentEdge.End.Position == _position ? _currentEdge.End : _currentEdge.Start;
                if (currentNode[_nextDirection] != null)
                {
                    _currentDirection = _nextDirection;
                    _currentEdge = currentNode[_currentDirection];
                }
            }
        }

        public void LoadContent()
        {
        }

        public void UnloadContent()
        {
        }
    }
}