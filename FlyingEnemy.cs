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
    public class FlyingEnemy : Zombie
    {
        private Player _player;
        public Random random = new Random();
        public string enemyName;

        public FlyingEnemy(Player player, string name) : base(player, name) 
        {
            _player = player;
            ObjectName = name;
        }
        public override void Initialize()
        {
            LoadContent();
            Origin = new(Texture.Width/2, Texture.Height/2);
            MaxSpeed = 100.0f;
            Health = 1f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("Zombie");

        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            if (_player != null)
            {
                Vector2 direction = Vector2.Normalize(_player.Position - Position);

                Position += direction * MaxSpeed * ScalableGameTime.DeltaTime;
            }
        }
    }
}
