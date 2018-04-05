using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Pacman.Ghosts
{
    public class Ghost
    {
        private Vector2 _target;
        private LevelGraph _graph;
        private readonly IGhostDirector _director;
        private readonly Color _color;
        private const float SPEED = 8f;

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 NextNode { get; private set; }


        public Ghost(LevelGraph graph, IGhostDirector director, Color color, int thickness)
        {
            _graph = graph;
            _director = director;
            _color = color;
        }


        public void Reset()
        {
            Position = NextNode = _director.StartPos;
            Direction = Directions.Stopped;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = Position * tileSizeVector;
            spriteBatch.DrawCircle(pos, 10, 10, _color, 10);
            spriteBatch.DrawCircle(_target * tileSizeVector, 20, 20, _color, 3);
        }

        public void Update(GameTime gameTime, Player player)
        {
            var nextTarget = _director.GetTargetNode(gameTime, this, player);

            _target = nextTarget;

            var travelDistance = SPEED * gameTime.GetElapsedSeconds();

            var distanceToNextNode = Vector2.Distance(NextNode, Position);

            if (distanceToNextNode <= travelDistance)
            {
                travelDistance -= distanceToNextNode;

                Position = NextNode;

                var nextNodeOptions = _graph.Adjacent(Position).Where(x => Position - x != Direction).ToList();

                if (nextNodeOptions.Any() == false)
                {
                    Direction = Vector2.Negate(Direction);
                    NextNode = Position + Direction;
                }
                else if (nextNodeOptions.Count > 1)
                {
                    NextNode = nextNodeOptions.OrderBy(pos => Vector2.Distance(pos, _target)).First();
                }
                else
                {
                    NextNode = nextNodeOptions.First();
                }

                Direction = NextNode - Position;
            }

            Position += Direction * new Vector2(travelDistance);
        }
    }
}