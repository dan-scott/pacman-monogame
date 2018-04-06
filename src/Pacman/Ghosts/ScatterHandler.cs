using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Pacman.Ghosts
{
    public class ScatterHandler
    {
        private static readonly float[] Times = {5, 20, 5, 20, 7, 20, 7};

        private readonly Stack<float> _timeStack;
        private float _sinceLastModeChange;
        private float _currentModeLength;
        private bool _paused;

        private bool _chaseMode;
        private bool _isModeFresh;

        public (bool inChaseMode, bool freshChange) GetMode() 
        {
            if (_isModeFresh)
            {
                _isModeFresh = false;
                return (_chaseMode, true);
            }

            return (_chaseMode, false);
        }

        public ScatterHandler()
        {
            _timeStack = new Stack<float>();
            Reset();
        }

        public void Pause()
        {
            _paused = !_paused;
        }

        public void Reset()
        {
            _chaseMode = false;
            _sinceLastModeChange = 0;
            _timeStack.Clear();
            _paused = false;

            foreach (var time in Times)
            {
                _timeStack.Push(time);
            }

            _currentModeLength = _timeStack.Pop();
        }

        public void Update(GameTime time)
        {
            if (!_timeStack.Any())
            {
                return;
            }

            _sinceLastModeChange += time.GetElapsedSeconds();

            if (_sinceLastModeChange < _currentModeLength)
            {
                return;
            }

            _chaseMode = !_chaseMode;
            _isModeFresh = true;
            _sinceLastModeChange = 0;
            _currentModeLength = _timeStack.Pop();
        }
    }
}