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
        private Vector2 _target;
        private List<Vector2> _path;
        private LevelGraph _graph;
        private readonly IGhostDirector _director;
        private readonly Color _color;
        private readonly int _thickness;
        private readonly PathFinder _pathFinder;
        private int _currentPathStep;
        private const float SPEED = 8f;

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 NextNode { get; private set; }


        public Ghost(LevelGraph graph, IGhostDirector director, Color color, int thickness)
        {
            _graph = graph;
            _director = director;
            _color = color;
            _thickness = thickness;
            _path = new List<Vector2>();
            _pathFinder = new PathFinder(graph);
        }


        public void Reset()
        {
            Position = _startPos = NextNode = _director.StartPos;
            Direction = Directions.Stopped;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = Position * tileSizeVector;
            spriteBatch.DrawCircle(pos, 10, 10, _color, 10);

            for (var i = 1; i < _path.Count; i++)
            {
                spriteBatch.DrawLine(_path[i-1] * tileSizeVector, _path[i] * tileSizeVector, _color, _thickness);
            }

        }

        public void Update(GameTime gameTime, Player player)
        {
            var nextTarget = _director.GetTargetNode(gameTime, this, player);
            var prevDir = Direction;

            if (_target != nextTarget)
            {
                _path.Clear();
                _target = nextTarget;
                _path = _pathFinder.FindPath(NextNode, _target, Direction);
                _currentPathStep = 1;
                Direction = NextNode - _startPos;
            }

            var travelDistance = SPEED * gameTime.GetElapsedSeconds();

            var distanceToNextNode = Vector2.Distance(NextNode, Position);

            if (distanceToNextNode <= travelDistance)
            {
                travelDistance -= distanceToNextNode;
                Position = _startPos = NextNode;

                if (_currentPathStep >= _path.Count)
                {
                    var adj = _graph.Adjacent(Position).ToList();
                    NextNode = adj.Contains(Position + Direction) ? Position + Direction : adj.First();
                    Direction = NextNode - Position;
                }
                else
                {
                    NextNode = _path[_currentPathStep++];
                    Direction = NextNode - _startPos;
                }
            }

            Position += Direction * new Vector2(travelDistance);
        }

    }
}