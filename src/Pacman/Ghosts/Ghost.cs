using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace Pacman.Ghosts
{
    public class Ghost
    {
        private static readonly List<Rectangle> Frames = new List<Rectangle>
        {
            new Rectangle(0, 0, 32, 32),
            new Rectangle(32, 0, 32, 32),
        };

        private static readonly Dictionary<string, Rectangle> Eyes = new Dictionary<string, Rectangle>
        {
            ["right"] = new Rectangle(64, 0, 32, 16),
            ["left"] = new Rectangle(64, 16, 32, 16),
            ["up"] = new Rectangle(96, 0, 32, 16),
            ["down"] = new Rectangle(96, 16, 32, 16),
        };

        private Vector2 _target;
        private LevelGraph _graph;
        private IGhostTargetPicker _targetPicker;
        private Color _color;
        private Vector2 _homePosition;
        private Vector2 _startPosition;
        private readonly ScatterHandler _scatterHandler;
        private bool _forceReverse;
        private bool _chaseMode;
        private readonly AnimatedSprite _normalSprite;
        private TextureAtlas _ghostSprites;
        private const float SPEED = 8f;

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 NextNode { get; private set; }


        private Ghost()
        {
            _scatterHandler = new ScatterHandler();

            var cm = GameServices.GetService<ContentManager>();

            _normalSprite = new AnimatedSprite("ghost", Frames, 0.2f);

            var ghostSheet = cm.Load<Texture2D>("ghost");

            _ghostSprites = new TextureAtlas("ghost", ghostSheet, Eyes);

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
            var translation = Globals.DefaultTranslation;

            
            var pos = Position * tileSizeVector - new Vector2(16);

            translation *= Matrix.CreateTranslation(pos.X, pos.Y, 0);

            spriteBatch.Begin(transformMatrix: translation);

            _normalSprite.Draw(spriteBatch, Vector2.Zero, _color);

            var eyedir = "left";

            if (Direction == Directions.Right)
            {
                eyedir = "right";
            } else if (Direction == Directions.Up)
            {
                eyedir = "up";
            } else if (Direction == Directions.Down)
            {
                eyedir = "down";
            }
            
            spriteBatch.Draw(_ghostSprites[eyedir], Vector2.Zero, Color.White);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, Player player)
        {
            _normalSprite.Update(gameTime);

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