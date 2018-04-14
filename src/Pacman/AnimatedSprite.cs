using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;


namespace Pacman
{
    public class AnimatedSprite
    {
        private readonly TextureAtlas _atlas;
        private readonly float _speed;
        private readonly string[] _frameNames;
        private readonly int _frameCount;
        private int _currentFrame;
        private float _sinceLastFrame;

        public AnimatedSprite(string textureName, IEnumerable<Rectangle> frames, float speed)
        {
            var texture = GameServices.GetService<ContentManager>().Load<Texture2D>(textureName);

            var frameDictionary = frames
                .Select((rect, i) => (rect: rect, index: i.ToString()))
                .ToDictionary(x => x.index, x => x.rect);

            _atlas = new TextureAtlas(textureName, texture, frameDictionary);
            _frameNames = frameDictionary.Keys.ToArray();
            _frameCount = _frameNames.Length;
            _speed = speed;
            Reset();
        }

        private void Reset()
        {
            _currentFrame = 0;
            _sinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _sinceLastFrame += gameTime.GetElapsedSeconds();
            
            if (_sinceLastFrame < _speed)
            {
                return;
            }

            _sinceLastFrame -= _speed;
            _currentFrame = (_currentFrame + 1) % _frameCount;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color? color = null)
        {
            spriteBatch.Draw(_atlas[_frameNames[_currentFrame]], position, color ?? Color.White);}
    }
}
