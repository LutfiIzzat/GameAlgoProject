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
    public class Enemy : GameObject
    {
        // FSM for navigation
        public HCFSM FSM;

        // Attributes
        public float MaxSpeed;
        public float Speed;
        public Texture2D Texture;
        public SpriteSheet SpriteSheet;
        public AnimatedSprite Animation;
        public float Health;

        // Visual appearance
        public TiledMap tiledMap;
        public TileGraph tileGraph;

        public Enemy(string name) : base()
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
            //SpriteSheet = new JsonContentLoader().Load(_game.Content,("parman-animations.sf");
        }

        public override void Start()
        {
            //fill in gamemap name
            GameMap gameMap = (GameMap)GameObjectCollection.FindByName("GameMap");
            tiledMap = gameMap.TiledMap;
            tileGraph = gameMap.TileGraph;

            Animation = new AnimatedSprite(SpriteSheet);
            Animation.OriginNormalized = Vector2.Zero;
            Animation.Play("ghostRedDown");
            Animation.Update(0);

            Random random = new Random();
            int randomColumn = random.Next(0, tiledMap.Width);
            int randomRow = random.Next(0, tiledMap.Height);
            Tile startTile = new Tile(gameMap.StartColumn, gameMap.StartRow);
            Position = Tile.ToPosition(startTile, tiledMap.TileWidth, tiledMap.TileHeight);
            //Tile randomTile = new Tile(randomColumn, randomRow);
            //Position = Tile.ToPosition(randomTile, _tiledMap.TileWidth, _tiledMap.TileHeight);

            FSM = new NavigationHCFSM(_game, this, tiledMap, tileGraph);
            FSM.Initialize();
        }

        public override void Update()
        {
            FSM.Update();
        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();

            _game.SpriteBatch.Draw(Animation, Position, Orientation, Scale);

            _game.SpriteBatch.End();
        }

        // Given source (src) and destination (dest) locations, and elapsed time, 
        //     try to move from source to destination at the given speed within elapsed time.
        // If cannot reach dest within the elapsed time, return the location where it will reach
        // If can reach or over-reach the dest, the return dest.
        public Vector2 Move(Vector2 src, Vector2 dest, float elapsedSeconds)
        {
            Vector2 dP = dest - src;
            float distance = dP.Length();
            float step = MaxSpeed * elapsedSeconds;

            if (step < distance)
            {
                dP.Normalize();
                return src + (dP * step);
            }
            else
            {
                return dest;
            }
        }

        // Select the ghost current animation based on:
        // (a) Which tile the ghost is standing on (ghostTile)
        // (b) Which tile the ghost is heading next (nextTile)
        //
        // Pre-conditions:
        //    The animation name is suffixed by:
        //      "NorthWest", "Up", "NorthEast", "Left", "Centre", "Right", "SouthWest", "Down", "SouthEast"
        //
        // Example:
        //    If nextTile is on the right of ghostTile, the animation to play is "ghostRedRight".
        public void UpdateAnimatedSprite(Tile ghostTile, Tile nextTile)
        {
            string[] directions = {"NorthWest", "Up", "NorthEast",
                                   "Left", "Centre", "Right",
                                   "SouthWest", "Down", "SouthEast"};

            if (ghostTile == null || nextTile == null)
            {
                throw new ArgumentNullException("UpdateAnimatedSprite()");
            }
            else
            {
                Tile difference = new Tile(nextTile.Col - ghostTile.Col, nextTile.Row - ghostTile.Row);
                int index = (difference.Col + 1) + 3 * (difference.Row + 1);

                //string animationName = $"ghostRed{directions[index]}";
                //Animation.Play(animationName);
            }
        }
    }

}
