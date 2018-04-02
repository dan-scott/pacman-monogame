using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public static class PathFinder
    {
        public static List<Vector2> AStarSearch(PathNode start, PathNode end)
        {
            var cameFrom = new Dictionary<PathNode, PathNode>();
            var costSoFar = new Dictionary<PathNode, float>();

            var frontier = new PriorityQueue();

            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current == end)
                {
                    break;
                }

                foreach (var edge in current.Edges)
                {
                    var newCost = costSoFar[current] + edge.Length;

                    var next = edge.End;

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        var priority = newCost + Vector2.Distance(next.Position, end.Position);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            return ToDirections(start, end, cameFrom);
        }

        private static List<Vector2> ToDirections(PathNode start, PathNode end, Dictionary<PathNode, PathNode> cameFrom)
        {
            var dirList = new List<Vector2>();

            var current = cameFrom[end];

            dirList.Add(end.GetDirectionTo(current));

            while (current != start)
            {
                var next = cameFrom[current];
                dirList.Add(current.GetDirectionTo(next));
                current = next;
            }

            return dirList;
        }


        private class PriorityQueue
        {
            private List<(float priority, PathNode node)> _nodes;

            public PriorityQueue()
            {
                _nodes = new List<(float priority, PathNode node)>();
            }

            public int Count => _nodes.Count;

            public void Enqueue(PathNode node, float priority)
            {
                _nodes.Add((priority, node));

                _nodes = _nodes.OrderBy(x => x.priority).ToList();
            }

            public PathNode Dequeue()
            {
                var last = _nodes.FirstOrDefault();
                _nodes.Remove(last);
                return last.node;
            }
        }
    }
}