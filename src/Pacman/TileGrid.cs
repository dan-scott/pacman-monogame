﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class TileGrid
    {
        private static readonly Vector2 DirUp = new Vector2(0, -1);
        private static readonly Vector2 DirRight = new Vector2(1, 0);
        private static readonly Vector2 DirDown = new Vector2(0, 1);
        private static readonly Vector2 DirLeft = new Vector2(-1, 0);

        private static readonly Vector2[] AdjacencyDirections = { DirDown, DirLeft, DirRight, DirUp};

        private readonly Dictionary<Vector2, LevelTile> _tiles;


        public TileGrid()
        {
            _tiles = new Dictionary<Vector2, LevelTile>();
        }

        private TileGrid(IEnumerable<KeyValuePair<Vector2, LevelTile>> tiles)
        {
            _tiles = tiles.ToDictionary(x => x.Key, x => x.Value);
        }

        public LevelTile this[int x, int y]
        {
            get => Get(x, y);
            set => Set(x, y, value);
        }

        public LevelTile this[Vector2 pos]
        {
            get => Get(pos);
            set => Set(pos, value);
        }

        public void Set(int column, int row, LevelTile tile)
            => Set(new Vector2(column, row), tile);

        public void Set(Vector2 pos, LevelTile tile)
            => _tiles[pos] = tile;

        public LevelTile Get(int column, int row)
            => Get(new Vector2(column, row));

        public LevelTile Get(Vector2 pos)
            => _tiles.TryGetValue(pos, out var tile)
                ? tile
                : LevelTile.None;

        public IEnumerable<(Vector2 pos, Vector2 dir, LevelTile tile)> GetAdjacent(Vector2 pos)
        {
            return AdjacencyDirections.Select(dir => (pos + dir, dir, Get(pos + dir)));
        }

        public Rectangle GetBounds()
        {
            var topX = (int) _tiles.Keys.Min(pos => pos.X);
            var topY = (int) _tiles.Keys.Min(pos => pos.Y);
            var bottomX = (int) _tiles.Keys.Max(pos => pos.X);
            var bottomY = (int) _tiles.Keys.Max(pos => pos.Y);

            return new Rectangle(topX, topY, bottomX - topX, bottomY - topY);
        }

        public IEnumerable<(Vector2, LevelTile)> ToScreenSpace(int tileSize)
        {
            var tileSizeVector = new Vector2(tileSize);
            var tileOffsetVector = new Vector2((float) tileSize / 2);
            return _tiles.Select(kvp => (kvp.Key * tileSizeVector - tileOffsetVector, kvp.Value));
        }

        public TileGrid Filter(params LevelTile[] types) 
            => new TileGrid(_tiles.Where(x => types.Contains(x.Value)));

        public TileGrid Map(Func<LevelTile, LevelTile> mapper) 
            => new TileGrid(_tiles.ToDictionary(x => x.Key, x => mapper(x.Value)));

        public IEnumerable<Vector2> Find(LevelTile type)
            => _tiles.Where(x => x.Value == type).Select(x => x.Key);
    }
}