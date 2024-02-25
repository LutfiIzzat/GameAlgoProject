using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using GameAlgoProject;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections;
using MonoGame.Extended.Timers;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection;

namespace ZombieGame
{
    public class Player : GameObject
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
        public int firingRate = 5;
        public const float CoolingTime = 2f;
        public float LastFiredTime = 0f;

        // Visual appearance
        private Rectangle _ghostRect;
        private TiledMap _tiledMap;
        private TileGraph _tileGraph;

        TiledMapObjectLayer waypointsLayer;
        ArrayList waypointsList;
        int waypointIndex;
        float moveTimer;
        public TiledMapTileLayer WallLayer;
        public float scale;

        public Player() : base()
        {
        }

        public override void Initialize()
        {
            LoadContent();

            MaxSpeed = 100.0f;
            scale = 0.3f;

            Origin = new (Texture.Width/2, Texture.Height/2);
            LastFiredTime = 0f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("survivor-idle_handgun_0");

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


            Position = new Vector2(5 * _tiledMap.TileWidth, 28 * _tiledMap.TileHeight);
            WallLayer = gameMap.TiledMap.GetLayer<TiledMapTileLayer>("Wall");


        }

        public  Vector2 GetPosition()
        {
            return Position;
        }


        

        public override void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            MouseHandler();

            
            Vector2 movement = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.W))
            {
                movement.Y -= 1; 
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                movement.Y += 1; 
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                movement.X -= 1; 
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                movement.X += 1; 
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }

            Vector2 targetPosition = Position + movement * MaxSpeed * ScalableGameTime.DeltaTime;

            
            Rectangle playerBounds = new Rectangle(
                (int)(targetPosition.X - Texture.Width * scale / 2),
                (int)(targetPosition.Y - Texture.Height * scale / 2),
                (int)(Texture.Width * scale),
                (int)(Texture.Height * scale)
            );

            ushort startX = (ushort)(playerBounds.X / _tiledMap.TileWidth);
            ushort startY = (ushort)(playerBounds.Y / _tiledMap.TileHeight);

            // Calculate the range of tiles that the player may intersect with
            ushort endX = (ushort)((playerBounds.X + playerBounds.Width) / _tiledMap.TileWidth);
            if (endX >= _tiledMap.Width)
            {
                endX = (ushort)(_tiledMap.Width - 1); 
            }

            ushort endY = (ushort)((playerBounds.Y + playerBounds.Height) / _tiledMap.TileHeight);
            if (endY >= _tiledMap.Height)
            {
                endY = (ushort)(_tiledMap.Height - 1); 
            }

            TiledMapTile? tile = null;

            for (ushort y = startY; y <= endY; y++)
            {
                for (ushort x = startX; x <= endX; x++)
                {
                    if (WallLayer.TryGetTile(x, y, out tile) && !tile.Value.IsBlank)
                    {
                        // If there's a collision, prevent movement and exit the loop
                        Debug.WriteLine("Colliding with wall tile at position: " + x + ", " + y);
                        return;
                    }
                }
            }


            
            Position = targetPosition;
            Debug.WriteLine("Not colliding");

        }
    
        
        public void MouseHandler()
        {
            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();
            Vector2 offset = mousePosition - Position;
            Orientation = (float)Math.Atan2(offset.Y, offset.X);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                ShootHandler();
            }
        }

        public void ShootHandler()
        {
            if (LastFiredTime + CoolingTime <= ScalableGameTime.RealTime)
            {
                for(int i=0; i < firingRate; i++)
                {
                    Bullet bullet = new(this);
                    bullet.Initialize();
                    bullet.Position = Position;
                }

                LastFiredTime = ScalableGameTime.RealTime;
            }
        }


        public override void Draw()
        {
            // Draw the ghost at his position, extracting only the ghost image from the texture
            _game.SpriteBatch.Begin();

            _game.SpriteBatch.Draw(Texture, Position, null, Color.White, Orientation, Origin, scale, SpriteEffects.None, 0f);

            _game.SpriteBatch.End();
        }
    }
}

