using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class Player
    {

        private const int SIZE = 40;
        private const int HALF_SIZE = SIZE / 2;

        private const int SPEED = 50;
        
        private readonly Vector2 _offset;

        private Vector2 _direction;
        private Vector2 _position;

        private readonly Texture2D _playerSprite;

        public Direction Direction { get; private set; }

        public Player(GraphicsDevice graphicsDevice)
        {
            _playerSprite = new Texture2D(graphicsDevice, SIZE, SIZE);
            
            var color = Enumerable.Range(1, SIZE * SIZE).Select(_ => Color.White).ToArray();

            _playerSprite.SetData(color);

            _position = new Vector2(20 * 14, 20 * 23 + 10);

            _offset = new Vector2(Level.OFFSET_X - HALF_SIZE, Level.OFFSET_Y - HALF_SIZE);

            SetDirection(Direction.Stopped);
        }

        public void Update(GameTime time)
        {
            var speed = (float)time.ElapsedGameTime.TotalSeconds * SPEED;
            _position += new Vector2(_direction.X * speed, _direction.Y * speed);
        }

        public void Render(GameTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_playerSprite, _position + _offset, Color.Yellow);
        }

        public void SetDirection(Direction newDirection)
        {
            Direction = newDirection;
            _direction = DirectionVector[newDirection];
        }

        private static readonly Dictionary<Direction, Vector2> DirectionVector = new Dictionary<Direction, Vector2>
        {
            [Direction.Stopped] = Vector2.Zero,
            [Direction.Down] = new Vector2(0, 1),
            [Direction.Left] = new Vector2(-1, 0),
            [Direction.Right] = new Vector2(1, 0),
            [Direction.Up] = new Vector2(0, -1),
        };

    }

    public enum Direction
    {
        Stopped,
        Up,
        Down,
        Left,
        Right,
    }
}