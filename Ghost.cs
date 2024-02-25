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
    public class Ghost : GameObject
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

        // Visual appearance
        private Rectangle _ghostRect;
        private TiledMap _tiledMap;
        private TileGraph _tileGraph;

        TiledMapObjectLayer waypointsLayer;
        ArrayList waypointsList;
        int waypointIndex;
        float moveTimer;

        public Ghost() : base()
        {
        }

        public override void Initialize()
        {
            LoadContent();

            MaxSpeed = 100.0f;
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("pacman-sprites");

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
            
			
            _ghostRect = new Rectangle
            {
                X       = 0 * _tiledMap.TileWidth,
                Y       = 6 * _tiledMap.TileHeight,
                Width   = _tiledMap.TileWidth,
                Height  = _tiledMap.TileHeight
            };





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

         

            // Implement Hard-Coded FSM for movement behaviour
            if (_currentState == NavigationState.STOP)
            {
                moveTimer += ScalableGameTime.DeltaTime;


                // Left mouse button pressed


                if (moveTimer >= 3.0f)
                {

                    //take the next waypoint's x,y coordinates
                    var toGo = (Tuple<float, float>)waypointsList[waypointIndex];

                   //insert in nowPosition to go 
                    Vector2 nowPosition = new Vector2(toGo.Item1, toGo.Item2);
                    

                    // Get selected tile
                    _destTile = Tile.ToTile(nowPosition, tileWidth, tileHeight);


                    //increase waypoint index based on the amount of waypoints, if reaches maximum then reset to 0
                    if (waypointIndex == waypointsList.Count - 1)
                    {
                        waypointIndex = 0;
                    }
                    else
                    {
                        waypointIndex++;
                    }

                    moveTimer = 0;

                    /********************************************************************************
                        PROBLEM 2: Fill in the blanks based on the logic below:

                                    IF (1) The destination tile is in the tile graph.
                                       (2) The destination tile is not the source tile.

                        HOWTOSOLVE : 1. Fill in the blanks.

                        if (_tileGraph.________.________(________) &&
                            !________.Equals(________)
                           )
                    ********************************************************************************/

                    if (_tileGraph.Nodes.Contains(_destTile) &&
                        !_srcTile.Equals(_destTile)
                       )
                    {
                        // Transition Actions
                        // 1. Compute an A* path
                        _path = AStar.Compute(_tileGraph, _srcTile, _destTile, AStarHeuristic.EuclideanSquared);
                        // 2. Remove the source tile from the path
                        _path.RemoveFirst();

                        // Ensure the ghost animation is used.
                        // The animation to play is determined based on difference between:
                        // (a) The tile the ghost is standing on (i.e. the source tile in this case)
                        // (b) The next tile the ghost will move towards
                        //     (i.e. the first tile after the source tile in the path)


                        //UpdateAnimatedSprite(_srcTile, _path.First.Value);

                        // Change to MOVING state
                        _currentState = NavigationState.MOVING;
                    }

                } // NOTE: No state action to execute for STOP state
                

            }
            else if (_currentState == NavigationState.MOVING)
            {
                float elapsedSeconds = ScalableGameTime.DeltaTime;

            /********************************************************************************
                PROBLEM 2: Fill in the blanks based on the logic below:

                            IF (1) The A* path is empty, OR
                               (2) The ghost has reached the destination tile
                                    (i.e., both positions matched).

                HOWTOSOLVE : 1. Fill in the blanks.

                if (_path.Count == ________ ||
                    ________.Equals(Tile.ToPosition(________, tileWidth, ________))
                    )
            ********************************************************************************/

                if (_path.Count == 0 ||
                    Position.Equals(Tile.ToPosition(_destTile, tileWidth, tileHeight))
                   )
                {
                    // Update source tile to destination tile
                    _srcTile = _destTile;
                    _destTile = null;

                    // Change to STOP state
                    _currentState = NavigationState.STOP;
                }

                // Action to execute on the MOVING state
                else
                {
                    Tile headTile = _path.First.Value; // throw exception if path is empty

                    Vector2 headTilePosition = Tile.ToPosition(headTile, tileWidth, tileHeight);

                    if (Position.Equals(headTilePosition))
                    {
                        Debug.WriteLine($"Reach head tile (Col = {headTile.Col}, Row = {headTile.Row}).");

				/********************************************************************************
					PROBLEM 4(D): Update the animation based on the previous and current tile the player was in.


					HOWTOSOLVE : 1. Copy the code below.
								 2. Paste it below this block comment.
								 3. Fill in the blanks.

					Tile nextTile = _path.First.________.________;
					UpdateAnimatedSprite(headTile, ________);

				********************************************************************************/





                        Debug.WriteLine($"Removing head tile. Get next node from path.");
                        _path.RemoveFirst();

                        // Get the next destination position
                        headTile = _path.First.Value; // throw exception if path is empty
                    }

                    // Move the ghost to the new tile location
                    Position = Move(Position, headTilePosition, elapsedSeconds);

                /********************************************************************************
                    PROBLEM 4(D): Update the ghost animation.


                    HOWTOSOLVE : 1. Copy the code below.
                                    2. Paste it below this block comment.
                                    3. Fill in the blanks.

                    Animation.Update(________);

                ********************************************************************************/

                    
					
					

                }
            }
        }

        public override void Draw()
        {
            // Draw the ghost at his position, extracting only the ghost image from the texture
            _game.SpriteBatch.Begin();

            /********************************************************************************
                PROBLEM 1: Fill in the blanks based on the logic below:

                            IF (1) The destination tile is in the tile graph.
                                (2) The destination tile is not the source tile.

                HOWTOSOLVE : 1. Copy the code below.
                                2. Paste it below this block comment.
                                3. Fill in the blanks.

                // The code below should be commented when attempting Problem 4
                _game.SpriteBatch.Draw(________, Position, ________, Color.White, Orientation, Origin, Scale, SpriteEffects.None, 0f);

            ********************************************************************************/

            _game.SpriteBatch.Draw(Texture, Position, _ghostRect, Color.White, Orientation, Origin, Scale, SpriteEffects.None, 0f);


            /********************************************************************************
                PROBLEM 4(E): Fill in the blanks based on the logic below:

                            IF (1) The destination tile is in the tile graph.
                                (2) The destination tile is not the source tile.

                HOWTOSOLVE : 1. Copy the code below.
                                2. Paste it below this block comment.
                                3. Fill in the blanks.

                // The code below should be commented when attempting Problem 1 to 3 and uncommented when attempting Problem 4
                // _game.SpriteBatch.Draw(________, Position, ________, Scale);

            ********************************************************************************/



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
    }
}
