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
        private IGhostTargetPicker _targetPicker;
        private Color _color;
        private Vector2 _homePosition;
        private Vector2 _startPosition;
        private readonly ScatterHandler _scatterHandler;
        private bool _forceReverse;
        private bool _chaseMode;
        private const float SPEED = 8f;

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 NextNode { get; private set; }


        private Ghost()
        {
            _scatterHandler = new ScatterHandler();
        }

        public static Ghost Red(LevelGraph graph) => new Ghost
        {
            _graph = graph,
            _targetPicker = new RedGhostTargetPicker(),
            _color = Color.Red,
            _startPosition = new Vector2(11, 11),
            _homePosition = new Vector2(24, -2),
        };

        public static Ghost Blue(LevelGraph graph) => new Ghost
        {
            _graph = graph,
            _targetPicker = new BlueGhostTargetPicker(),
            _color = Color.Cyan,
            _startPosition = new Vector2(12, 11),
            _homePosition = new Vector2(28, 33),
        };

        public static Ghost Pink(LevelGraph graph) => new Ghost
        {
            _graph = graph,
            _targetPicker = new PinkGhostTargetPicker(),
            _color = Color.Pink,
            _startPosition = new Vector2(13, 11),
            _homePosition = new Vector2(0, -2),
        };

        public static Ghost Orange(LevelGraph graph) => new Ghost
        {
            _graph = graph,
            _targetPicker = new OrangeGhostTargetPicker(),
            _color = Color.Orange,
            _startPosition = new Vector2(13, 11),
            _homePosition = new Vector2(0, 33),
        };

        public void Reset()
        {
            Position = NextNode = _startPosition;
            Direction = Directions.Stopped;
            _scatterHandler.Reset();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSizeVector)
        {
            var pos = Position * tileSizeVector;
            spriteBatch.DrawCircle(pos, 10, 10, _color, 10);
            spriteBatch.DrawCircle(_target * tileSizeVector, 20, 20, _color, 3);
        }

        public void Update(GameTime gameTime, Player player)
        {
            _scatterHandler.Update(gameTime);

            (_chaseMode, _forceReverse) = _scatterHandler.GetMode();

            _target = _chaseMode ? _targetPicker.GetTargetNode(gameTime, this, player) : _homePosition;

            var travelDistance = SPEED * gameTime.GetElapsedSeconds();

            travelDistance = _forceReverse ? MoveWithReverse(travelDistance) : Move(travelDistance);

            Position += Direction * new Vector2(travelDistance);
        }

        private float Move(float travelDistance)
        {
            var distanceToNextNode = Vector2.Distance(NextNode, Position);

            if (distanceToNextNode > travelDistance)
            {
                return travelDistance;
            }

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

            return travelDistance;
        }

        private float MoveWithReverse(float travelDistance)
        {
            var distanceToNextNode = Vector2.Distance(NextNode, Position);

            if (distanceToNextNode > travelDistance)
            {
                return travelDistance;
            }

            travelDistance -= distanceToNextNode;

            Position = NextNode;

            Direction = Vector2.Negate(Direction);

            NextNode = Position + Direction;

            _forceReverse = false;

            return travelDistance;
        }
    }
}