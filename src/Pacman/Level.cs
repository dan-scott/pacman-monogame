using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Pacman
{
    public class Level
    {
        private const int TILE_SIZE = 20;
        public const int OFFSET_X = 120;
        public const int OFFSET_Y = 100;


        private static readonly Vector2 DirUp = new Vector2(0, -1);
        private static readonly Vector2 DirRight = new Vector2(1, 0);
        private static readonly Vector2 DirDown = new Vector2(0, 1);
        private static readonly Vector2 DirLeft = new Vector2(-1, 0);
        private readonly Vector2 _offset;
        private readonly Vector2 _nodeOffset;
        private readonly Size2 _size;


        public int Width { get; private set; }
        public int Height { get; private set; }
        public List<PathNode> Nodes { get; }

        private const string DEFAULT_MAZE = @"
0000000000000000000000000000
0************00************0
0*0000*00000*00*00000*0000*0
0+0  0*0   0*00*0   0*0  0+0
0*0000*00000*00*00000*0000*0
0**************************0
0*0000*00*00000000*00*0000*0
0*0000*00*00000000*00*0000*0
0******00****00****00******0
000000*00000 00 00000*000000
     0*00          00*0     
     0*00 ######## 00*0     
     0*00 #      # 00*0     
000000*00 #      # 00*000000
      *   #      #   *      
000000*00 #      # 00*000000
     0*00 ######## 00*0     
     0*00          00*0     
     0*00 00000000 00*0     
000000*00 00000000 00*000000
0************00************0
0*0000*00000*00*00000*0000*0
0*0000*00000*00*00000*0000*0
0+**00****************00**+0
000*00*00*00000000*00*00*000
000*00*00*00000000*00*00*000
0******00****00****00******0
0*0000000000*00*0000000000*0
0*0000000000*00*0000000000*0
0**************************0
0000000000000000000000000000
";

        private LevelTile[,] _tiles;

        public Level()
        {
            Nodes = new List<PathNode>();
            _offset = new Vector2(OFFSET_X - TILE_SIZE / 2, OFFSET_Y - TILE_SIZE / 2);
            _nodeOffset = new Vector2(OFFSET_X, OFFSET_Y);
            _size = new Size2(TILE_SIZE, TILE_SIZE);
            ParseMaze(DEFAULT_MAZE);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var pos = new Vector2(x * TILE_SIZE, y * TILE_SIZE) + _offset;
                    var color = TileColors[_tiles[x, y]];
                    spriteBatch.DrawRectangle(pos, _size, color, 10);
                }
            }

            foreach (var pathNode in Nodes)
            {
                var pos = pathNode.Position * new Vector2(TILE_SIZE) + _nodeOffset;
                spriteBatch.DrawCircle(pos, 5, 8, Color.OrangeRed, 5);
            }
        }

        private void ParseMaze(string maze)
        {
            var lines = maze.Trim().Split('\n');

            Height = lines.Length;
            Width = lines[0].Length;

            if (lines[0].EndsWith("\r"))
            {
                Width--;
            }

            _tiles = new LevelTile[Width, Height];

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _tiles[x, y] = Tile(lines[y][x]);
                }
            }

            BuildNodes();
        }

        private void BuildNodes()
        {
            Nodes.Clear();

            var startNode = new PathNode(Vector2.One);

            Nodes.Add(startNode);

            var stack = new Stack<(Vector2 dir, PathNode node)>();
            stack.Push((DirDown, startNode));
            stack.Push((DirRight, startNode));

            while (stack.Count > 0)
            {
                var (dir, node) = stack.Pop();

                if (dir == DirUp && node.Up != null)
                {
                    continue;
                }

                if (dir == DirDown && node.Down != null)
                {
                    continue;
                }

                if (dir == DirLeft && node.Left != null)
                {
                    continue;
                }

                if (dir == DirRight && node.Right != null)
                {
                    continue;
                }

                var tilePos = node.Position;

                bool isEdge;
                bool isPortal;
                do
                {
                    tilePos += dir;
                    isPortal = IsPortalTile(tilePos, dir);
                    isEdge = !isPortal && IsEdgeTile(tilePos, dir);
                } while (isEdge);

                if (Nodes.Any(n => n.At(tilePos)))
                {
                    continue;
                }

                var nextNode = new PathNode(tilePos);

                Nodes.Add(nextNode);

                node.AddEdgeTo(nextNode);

                PushToStack(tilePos, dir, nextNode);

                if (isPortal)
                {
                    Vector2 portalTile;
                    Direction portalDir;
                    if (dir == DirDown)
                    {
                        portalTile = new Vector2(tilePos.X, 0);
                        portalDir = Direction.Down;
                    }
                    else if (dir == DirLeft)
                    {
                        portalTile = new Vector2(0, tilePos.Y);
                        portalDir = Direction.Left;
                    }
                    else if (dir == DirRight)
                    {
                        portalTile = new Vector2(Width - 1, tilePos.Y);
                        portalDir = Direction.Right;
                    }
                    else
                    {
                        portalTile = new Vector2(0, tilePos.Y);
                        portalDir = Direction.Up;
                    }

                    var nextPortalNode = new PathNode(portalTile);
                    nextNode.AddPortalTo(nextPortalNode, portalDir);

                    PushToStack(tilePos, dir, nextPortalNode);
                }
            }

            void PushToStack(Vector2 tilePos, Vector2 dir, PathNode node)
            {
                if (dir != DirDown && !IsWall(tilePos + DirUp))
                {
                    stack.Push((DirUp, node));
                }

                if (dir != DirUp && !IsWall(tilePos + DirDown))
                {
                    stack.Push((DirDown, node));
                }

                if (dir != DirRight && !IsWall(tilePos + DirLeft))
                {
                    stack.Push((DirLeft, node));
                }

                if (dir != DirLeft && !IsWall(tilePos + DirRight))
                {
                    stack.Push((DirRight, node));
                }
            }
        }

        private bool IsPortalTile(Vector2 tilePos, Vector2 dir)
        {
            return dir == DirDown && tilePos.Y == Height - 1
                   || dir == DirUp && tilePos.Y == 0
                   || dir == DirRight && tilePos.X == Width - 1
                   || dir == DirLeft && tilePos.X == 0;
        }

        private bool IsEdgeTile(Vector2 pos, Vector2 dir)
        {
            var nextPos = pos + dir;

            if (IsWall(nextPos))
            {
                return false;
            }

            if (dir == DirDown || dir == DirUp)
            {
                return IsWall(pos + DirRight) && IsWall(pos + DirLeft);
            }

            return IsWall(pos + DirUp) && IsWall(pos + DirDown);
        }

        private bool IsWall(Vector2 pos)
        {
            if (pos.X < 0 || pos.X >= Width)
            {
                return true;
            }

            if (pos.Y < 0 || pos.Y >= Height)
            {
                return true;
            }

            var type = _tiles[(int) pos.X, (int) pos.Y];
            return type == LevelTile.Wall || type == LevelTile.MonsterWall;
        }

        private static LevelTile Tile(char c)
        {
            switch (c)
            {
                case '0':
                    return LevelTile.Wall;

                case '*':
                    return LevelTile.Dot;

                case '+':
                    return LevelTile.PowerPellet;

                case '#':
                    return LevelTile.MonsterWall;

                default:
                    return LevelTile.Empty;
            }
        }

        private static readonly Dictionary<LevelTile, Color> TileColors = new Dictionary<LevelTile, Color>
        {
            [LevelTile.Dot] = Color.DarkGray,
            [LevelTile.Empty] = Color.Black,
            [LevelTile.MonsterWall] = Color.Blue,
            [LevelTile.PowerPellet] = Color.Yellow,
            [LevelTile.Wall] = Color.Blue,
        };
    }
}