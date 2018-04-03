using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Pacman
{
    public class Ghost
    {
        private Vector2 _startPos;
        private Vector2 _destPos;
        private Vector2 _position;
        private Vector2 _target;
        private List<Vector2> _path;
        private LevelGraph _graph;
        private Vector2 _direction;
        private PathFinder _pathFinder;
        private int _currentPathStep;
        private const float SPEED = 8f;

        public Ghost(LevelGraph graph)
        {
            _graph = graph;
            _path = new List<Vector2>();
            _pathFinder = new PathFinder(graph);
        }

        public void Reset(Vector2 startPos)
        {
            _position = _startPos = _destPos = startPos;
            _direction = Directions.Stopped;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = _position * tileSizeVector;
            spriteBatch.DrawCircle(pos, 10, 10, Color.Red, 10);

            for (var i = 1; i < _path.Count; i++)
            {
                spriteBatch.DrawLine(_path[i-1] * tileSizeVector, _path[i] * tileSizeVector, Color.Green, 4);
            }

        }

        public void Update(GameTime gameTime, Player player)
        {
            if (_target != player.Destination)
            {
                _path.Clear();
                _target = player.Destination;
                _path = _pathFinder.FindPath(_destPos, _target);
                _currentPathStep = 1;
                _direction = _destPos - _startPos;
            }

            var travelDistance = SPEED * gameTime.GetElapsedSeconds();

            var distanceToNextNode = Vector2.Distance(_destPos, _position);

            if (distanceToNextNode <= travelDistance)
            {
                travelDistance -= distanceToNextNode;
                _position = _startPos =_destPos;
                if (_currentPathStep >= _path.Count)
                {
                    _direction = Directions.Stopped;
                    _destPos = _position;
                }
                else
                {
                    _destPos = _path[_currentPathStep++];
                    _direction = _destPos - _startPos;
                }
            }

            _position += _direction * new Vector2(travelDistance);
        }

    }
}