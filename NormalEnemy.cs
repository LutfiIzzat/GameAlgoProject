using GameAlgoProject;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;

namespace ZombieGame
{
    public class NormalEnemy : Enemy
    {
        public NormalEnemy() : base()
        {

        }

        public override void Initialize()
        {
            LoadContent();
            Speed = 0f;
            MaxSpeed = 100.0f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("pacman-sprites");

            SpriteSheet = _game.Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
        }

    }
}
