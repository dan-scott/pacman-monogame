using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Pacman
{
    public class LevelDots
    {
        private static readonly LevelTile[] DotTypes = {LevelTile.Dot, LevelTile.PowerPellet};
        private int _totalDotCount;
        private int _currentDotCount;
        private readonly List<(Vector2, LevelTile)> _dots;
        private List<(Vector2, LevelTile)> _currentLevelDots;
        private SpriteFont _font;

        public LevelDots(TileGrid tiles)
        {
            _dots = tiles.Filter(DotTypes).AsEnumerable().ToList();
            Reset();
            _font = GameServices.GetService<ContentManager>().Load<SpriteFont>("Fonts/Arial");
        }

        public void Reset()
        {
            _currentLevelDots = _dots.ToList();
            _totalDotCount = _currentDotCount = _currentLevelDots.Count;
        }

        public void Update(Player player)
        {
            var closeDots = _currentLevelDots.Where(x => Vector2.Distance(x.Item1, player.Position) <= 0.1).ToList();

            if (!closeDots.Any()) return;

            var closest = closeDots.First();
            _currentLevelDots.Remove(closest);
            _currentDotCount--;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 tileSize)
        {
            spriteBatch.Begin(transformMatrix: Globals.DefaultTranslation);

            foreach (var (pos, type) in _currentLevelDots)
            {
                var radius = type == LevelTile.Dot ? 3 : 6;
                spriteBatch.DrawCircle(pos * tileSize, radius, 10, Color.Yellow, radius);
            }

            spriteBatch.End();


            spriteBatch.Begin();

            spriteBatch.DrawString(_font, $"{_currentDotCount}/{_totalDotCount}", new Vector2(10, 40), Color.Yellow);

            spriteBatch.End();
        }
    }
}
