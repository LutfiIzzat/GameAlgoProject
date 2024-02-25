using GameAlgoProject;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections;

namespace ZombieGame
{
    public class FlyingEnemy : GameObject, ICollidable
    {
        // FSM for navigation
        enum NavigationState { STOP, MOVING };

        // Navigation current state
        private NavigationState _currentState = NavigationState.STOP;

        // Navigation
        private Tile _srcTile;
        private Tile _destTile;
        private LinkedList<Tile> _path;

        // Attributes
        public float MaxSpeed;
        public Texture2D Texture;
        public SpriteSheet SpriteSheet;
        public AnimatedSprite Animation;
        public float Health;
        public string ObjectName;
        private Rectangle _bound;

        // Visual appearance
        private Rectangle _ghostRect;
        private TiledMap _tiledMap;
        private TileGraph _tileGraph;

        TiledMapObjectLayer waypointsLayer;
        ArrayList waypointsList;
        int waypointIndex;
        float moveTimer;
        Player _player;
        public float scale;

        public FlyingEnemy() : base()
        {
        }

        public FlyingEnemy(Player player, string name) : base()
        {
            _player = player;
            ObjectName = name;
        }

        public override void Initialize()
        {
            LoadContent();
            scale = 0.25f;
            Origin = new(Texture.Width / 2 * scale, Texture.Height / 2 * scale);
            MaxSpeed = 150.0f;
            _bound.Width = (int)(Texture.Width * scale);
            _bound.Height = (int)(Texture.Height * scale);
            _bound.Location = Position.ToPoint();
            Health = 1f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("Zombie");

            /********************************************************************************
                PROBLEM 4(A): Uncomment the code below as it is.

                HOWTOSOLVE : 1. Follow the instructions accordingly.

                // Load the sprite sheet
                SpriteSheet = _game.Content.Load<SpriteSheet>("pacman-animations.sf", new JsonContentLoader());
            ********************************************************************************/




        }

        public override void Start()
        {
            GameMap gameMap = (GameMap)GameObjectCollection.FindByName("GameMap");
            _tiledMap = gameMap.TiledMap;
            _tileGraph = gameMap.TileGraph;

            /********************************************************************************
                PROBLEM 1: Determine the location of the red ghost in "pacman-sprites.png".


                HOWTOSOLVE : 1. Copy the code below.
                                2. Paste it below this block comment.
                                3. Fill in the blanks.


                _ghostRect = new Rectangle
                {
                    X       = ________ * _tiledMap.TileWidth,
                    Y       = ________ * _tiledMap.TileHeight,
                    Width   = _tiledMap.________,
                    Height  = _tiledMap.________
                };
            ********************************************************************************/





            /********************************************************************************
                PROBLEM 4(B): Uncomment the code below as it is.

                HOWTOSOLVE : 1. Follow the instructions accordingly.

                // Initialize Animations
                Animation = new AnimatedSprite(SpriteSheet);
                Animation.OriginNormalized = Vector2.Zero;
                Animation.Play("ghostRedDown");
                Animation.Update(0);
            ********************************************************************************/

            //to make waypoints

            //get the object layer

            TiledMapObjectLayer waypointsLayer = gameMap.TiledMap.GetLayer<TiledMapObjectLayer>("Waypoints");
            //TiledMapTileLayer foodLayer = TiledMap.GetLayer<TiledMapTileLayer>("Food");

            waypointsList = new ArrayList();

            //insert xy coordinates into waypointslayer
            if (waypointsLayer != null)
            {
                foreach (var obj in waypointsLayer.Objects)
                {
                    //add those IDs here boi
                    float objectX = obj.Position.X;
                    float objectY = obj.Position.Y;

                    var waypoint = new Tuple<float, float>(objectX, objectY);

                    // Adding the tuple to ArrayList waypointsList 
                    waypointsList.Add(waypoint);

                }
            }

            //using count for looping
            int waypointCount = waypointsList.Count;


            // Initialize Source Tile
            // use first waypointList position for initial position

            var InitialTile = (Tuple<float, float>)waypointsList[0];

            /*            int X = Convert.ToInt32(InitialTile.Item1 / _tiledMap.TileWidth);
                        int Y = Convert.ToInt32(InitialTile.Item2 /  _tiledMap.TileHeight);

                        Debug.WriteLine(X + ", " + Y);*/

            //initialise where the ghost is based on the first waypoint
            _srcTile = Tile.ToTile(new Vector2(InitialTile.Item1, InitialTile.Item2), _tiledMap.TileWidth, _tiledMap.TileHeight);

            // Initialize Position
            Position = Tile.ToPosition(_srcTile, _tiledMap.TileWidth, _tiledMap.TileHeight);

            //the current waypoint it is on
            waypointIndex = 1;
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();

            int tileWidth = _tiledMap.TileWidth;
            int tileHeight = _tiledMap.TileHeight;

            Vector2 playerPosition = _player.Position;
            Vector2 offset = playerPosition - Position;
            Orientation = (float)Math.Atan2(offset.Y, offset.X);


            if (_player != null)
            {
                Vector2 direction = Vector2.Normalize(_player.Position - Position);

                Position += direction * MaxSpeed * ScalableGameTime.DeltaTime;
            }

    }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();

            _game.SpriteBatch.Draw(Texture, Position, null, Color.OrangeRed, Orientation, Origin, 0.25f, SpriteEffects.None, 0f);

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

        public string GetGroupName()
        {
            return this.GetType().Name;
        }

        public Rectangle GetBound()
        {
            _bound.Location = (Position - Origin).ToPoint();
            return _bound;
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            if (collisionInfo.Other is Bullet)
            {
                if (Health > 0)
                {
                    Health -= 1;
                }
                else if (Health <= 0)
                {
                    GameObjectCollection.DeInstantiate(this);
                }
            }
        }
    }
}



