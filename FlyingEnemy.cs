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
    public class FlyingEnemy : Enemy
    {
        private Player _player;
        public Random random = new Random();

        public FlyingEnemy(string name) : base(name) 
        { 
        }
        public override void Initialize()
        {
            LoadContent();
            Speed = 0f;
            MaxSpeed = 100.0f;
            Health = 1f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("pacman-sprites");

            SpriteSheet = _game.Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
        }

        public override void Start()
        {
            _player = (Player)GameObjectCollection.FindByName("Player");
            GameMap gameMap = (GameMap)GameObjectCollection.FindByName("GameMap");
            tiledMap = gameMap.TiledMap;
            tileGraph = gameMap.TileGraph;

            Animation = new AnimatedSprite(SpriteSheet);
            Animation.OriginNormalized = Vector2.Zero;
            Animation.Play("ghostEvade");
            Animation.Update(0);
            Position = new Vector2(0, 0);
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
