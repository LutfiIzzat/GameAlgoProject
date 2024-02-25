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
    public class GiantEnemy : Enemy
    {
        public GiantEnemy(string name) : base(name) 
        {
        }

        public override void Initialize()
        {
            LoadContent();
            Speed = 0f;
            MaxSpeed = 50.0f;
            Health = 5f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("pacman-sprites");

            SpriteSheet = _game.Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
        }

        public override void Start()
        {
            GameMap gameMap = (GameMap)GameObjectCollection.FindByName("GameMap");
            tiledMap = gameMap.TiledMap;
            tileGraph = gameMap.TileGraph;

            Animation = new AnimatedSprite(SpriteSheet);
            Animation.OriginNormalized = Vector2.Zero;
            Animation.Play("giantEnemy");
            Animation.Update(0);
            Tile startTile = new Tile(gameMap.StartColumn, gameMap.StartRow);
            Position = Tile.ToPosition(startTile, tiledMap.TileWidth, tiledMap.TileHeight);
            FSM = new NavigationHCFSM(_game, this, tiledMap, tileGraph);
            FSM.Initialize();
        }
    }
}
