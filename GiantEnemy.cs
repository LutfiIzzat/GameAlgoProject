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
    public class GiantEnemy : Zombie
    {
        Player _player;
        public GiantEnemy(Player player, string name) : base(player, name) 
        {
            _player = player;
            ObjectName = name;
        }

        public override void Initialize()
        {
            LoadContent();
            Origin = new(Texture.Width/2, Texture.Height/2);
            MaxSpeed = 50.0f;
            Health = 5f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("Zombie");

            //SpriteSheet = _game.Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
        }

        public override void Start()
        {
            //GameMap gameMap = (GameMap)GameObjectCollection.FindByName("GameMap");
            //_tiledMap = gameMap.TiledMap;
            //_tileGraph = gameMap.TileGraph;

            //Animation = new AnimatedSprite(SpriteSheet);
            //Animation.OriginNormalized = Vector2.Zero;
            //Animation.Play("giantEnemy");
            //Animation.Update(0);
            base.Start();
        }
    }
}
