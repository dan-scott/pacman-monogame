using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class PathFinder
    {
        private readonly LevelGraph _graph;

        public PathFinder(LevelGraph graph)
        {
            _graph = graph;
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 end)
        {
            var cameFrom = new Dictionary<Vector2, Vector2>();

            var costSoFar = new Dictionary<Vector2, float>();

            var frontier = new PriorityQueue<Vector2>();

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

                foreach (var next in _graph.Adjacent(current))
                {
                    var newCost = costSoFar[current] + 1;

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        var priority = newCost + Vector2.Distance(next, end);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            return Path(start, end, cameFrom);
        }

        private List<Vector2> Path(Vector2 start, Vector2 end, Dictionary<Vector2, Vector2> cameFrom)
        {
            var dirList = new List<Vector2> { end };

            var current = cameFrom[end];

            dirList.Add(current);

            while (current != start)
            {
                current = cameFrom[current];
                dirList.Add(current);
            }

            dirList.Reverse();

            return dirList;
        }

        private class PriorityQueue<T>
        {
            private List<(float priority, T node)> _nodes;

            public PriorityQueue()
            {
                _nodes = new List<(float priority, T node)>();
            }

            public int Count => _nodes.Count;

            public void Enqueue(T node, float priority)
            {
                _nodes.Add((priority, node));

                _nodes = _nodes.OrderBy(x => x.priority).ToList();
            }

            public T Dequeue()
            {
                var last = _nodes.FirstOrDefault();
                _nodes.Remove(last);
                return last.node;
            }
        }
    }
}