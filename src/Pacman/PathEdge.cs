namespace Pacman
{
    public class PathEdge
    {
        public int Length { get; }
        public bool IsPortal { get; }

        public PathNode Start { get; }
        public PathNode End { get; }

        public PathEdge(int length, PathNode start, PathNode end, bool isPortal = false)
        {
            Length = length;
            Start = start;
            End = end;
            IsPortal = isPortal;
        }

    }
}