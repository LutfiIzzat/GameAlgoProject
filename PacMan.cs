using GameAlgoProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class PacMan : GameObject
    {
        // Delegates
        public delegate void TileReachedHandler(Tile location);
        // Events
        public event TileReachedHandler TileReached;

        // Attributes
        public float Speed;
        public int StartColumn;
        public int StartRow;
        public string NavigableTileLayerName;

        // Visual Appearance
        public SpriteSheet SpriteSheet;
        public AnimatedSprite Animation;

        private enum Direction { UpLeft, Up, UpRight, Left, None, Right, DownLeft, Down, DownRight };

        private readonly int[] NextRow = { -1, -1, -1,  0, 0, 0,  1, 1, 1 };
        private readonly int[] NextCol = { -1,  0,  1, -1, 0, 1, -1, 0, 1 };

        private Direction _currDirection;
        private Direction _prevDirection;

        private Tile _currTile;
        private Vector2 _nextTilePosition;

        private TiledMap _tiledMap;
        private TiledMapTileLayer _tiledMapNavigableLayer;

        public PacMan() : base("Pacman")
        {
        }

        public override void Initialize()
        {
            LoadContent();

            // Initialize directions
            _currDirection = Direction.None;
            _prevDirection = Direction.None;

            NavigableTileLayerName = "Food";
            // Initialize animations
            Animation = new AnimatedSprite(SpriteSheet);
            Animation.OriginNormalized = Vector2.Zero;
            Animation.Play("pacmanCentre"); // switch the animation name to test.
            Animation.Update(0);
        }

        protected override void LoadContent()
        {
            SpriteSheet = _game.Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
        }

        public override void Start()
        {
            // Get graph and tiled map
            GameMap gameMap = (GameMap)GameObjectCollection.FindByName("GameMap");
            _tiledMap = gameMap.TiledMap;
            _tiledMapNavigableLayer = _tiledMap.GetLayer<TiledMapTileLayer>(NavigableTileLayerName);

            // Initialize positions
            StartColumn = Math.Max(0, Math.Min(StartColumn, _tiledMapNavigableLayer.Width - 1));
            StartRow = Math.Max(0, Math.Min(StartRow, _tiledMapNavigableLayer.Height - 1));

            _currTile = new Tile(StartColumn, StartRow);
            Position = Tile.ToPosition(_currTile, _tiledMap.TileWidth, _tiledMap.TileHeight);
            _nextTilePosition = Position;
        }

        public override void Update()
        {
            // Update direction from user input
            Direction newDirection = GetDirectionFromInput();
            UpdateDirection(newDirection);

            // Calculate a new next tile and position when Pacman reach its old next tile
            if (Position.Equals(_nextTilePosition))
            {
                /********************************************************************************
                    PROBLEM 3 : Calculate the current tile.


                    HOWTOSOLVE : 1. Copy the code below.
                                 2. Paste it below this block comment.
                                 3. Fill in the blanks.

                    // Update current tile
                    _currTile = ________;

                ********************************************************************************/

                _currTile = Tile.ToTile(Position, _tiledMap.TileWidth, _tiledMap.TileHeight);



                // Call the reach tile callback
                TileReached?.Invoke(_currTile);

                // Calculate the next tile
                Tile nextTile = _currTile;

                // Pacman always move in the current direction whenever possible.
                //   Otherwise, fall back to the previous direction.
                Direction[] directions = { _currDirection, _prevDirection };

                foreach (Direction direction in directions)
                {
                    nextTile = GetNextTileFromDirection(direction);
                    ushort col = (ushort)nextTile.Col;
                    ushort row = (ushort)nextTile.Row;

                    if (_tiledMapNavigableLayer.TryGetTile(col, row, out TiledMapTile? nextTiledMapTile))
                    {
                        // NOT BLANK: Paman found the next tile to move to
                        if (!nextTiledMapTile.Value.IsBlank)
                        { 
                            if (direction == _currDirection)
                            {
                                // Pacman can move in the current direction,
                                //   so previous direction not relevant anymore
                                _prevDirection = Direction.None;
                            }
                            break;
                        }
                        // BLANK: Pacman found the next tile non-navigable
                        else
                        {
                            // Pacman cannot move,
                            //   so should stay where it is
                            nextTile = _currTile;
                        }
                    }
                }

                // Update animation
                UpdateAnimatedSprite(_currTile, nextTile);

                // Recalculate next position to go to only if Pacman's next tile is a different tile
                if (!nextTile.Equals(_currTile))
                {
                    _nextTilePosition = Tile.ToPosition(nextTile, _tiledMap.TileWidth, _tiledMap.TileHeight);
                }
                else
                {
                    // Reset current and previous direction due to no movement.
                    _currDirection = Direction.None;
                    _prevDirection = Direction.None;
                }
            }

            // Move and animate pacman towards the next tile's position
            float elapsedSeconds = ScalableGameTime.DeltaTime;
            Position = Move(Position, _nextTilePosition, elapsedSeconds, Speed);
            Animation.Update(elapsedSeconds);
        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Animation, Position, Orientation, Scale);
            _game.SpriteBatch.End();
        }

        private Direction GetDirectionFromInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W))
            {
                return Direction.Up;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                return Direction.Down;
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                return Direction.Left;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                return Direction.Right;
            }
            else
            {
                return Direction.None;
            }
        }

        private void UpdateDirection(Direction newDirection)
        {
            // "newDirection = Direction.None" means no use input is entered
            if (newDirection != Direction.None)
            {
                if (_currDirection == Direction.None)
                {
                    _currDirection = newDirection;
                }
                else if (_prevDirection == Direction.None && newDirection != _currDirection)
                {
                    _prevDirection = _currDirection;
                    _currDirection = newDirection;
                }
            }
        }

        private Tile GetNextTileFromDirection(Direction direction)
        {
            int directionIndex = (int)direction;
            /********************************************************************************
                PROBLEM 2 : Determine the next tile from the given direction.


                HOWTOSOLVE : 1. Copy the code below.
                             2. Paste it below this block comment.
                             3. Fill in the blanks.

                Tile nextTile = new Tile(________.Col + ________[directionIndex]
                                        ,________.Row + ________[directionIndex]);
            ********************************************************************************/

            Tile nextTile = new Tile(_currTile.Col + NextCol[directionIndex]
                                    , _currTile.Row + NextRow[directionIndex]);


            return nextTile;
        }

        // Given source (src) and destination (dest) locations, and elapsed time, 
        //     try to move from source to destination at the given speed within elapsed time.
        // If cannot reach dest within the elapsed time, return the location where it will reach
        // If can reach or over-reach the dest, the return dest.
        public Vector2 Move(Vector2 src, Vector2 dest, float elapsedSeconds, float speed)
        {
            Vector2 dP = dest - src;
            float distance = dP.Length();
            float step = speed * elapsedSeconds;

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

        // Select the Pacman's animation based on:
        // (a) Which tile the Pacman is standing on (currTile)
        // (b) Which tile the Pacman is heading next (nextTile)
        //
        // Pre-conditions:
        //    The animation name is suffixed by:
        //      "UpLeft", "Up", "UpRight", "Left", "Stop", "Right", "Downleft", "Down", "DownRight"
        //
        // Example:
        //    If nextTile is on the RIGHT of currTile, the animation to play is "pacmanRight".
        public void UpdateAnimatedSprite(Tile currTile, Tile nextTile)
        {
            string[] directions = {"UpLeft", "Up", "UpRight",
                                   "Left", "Centre", "Right",
                                   "Downleft", "Down", "DownRight"};

            if (currTile == null || nextTile == null)
            {
                throw new ArgumentNullException("UpdateAnimatedSprite(): NULL in current tile or next tile.");
            }
            else
            {
                Tile difference = new Tile(nextTile.Col - currTile.Col, nextTile.Row - currTile.Row);
                int index = (difference.Col + 1) + 3 * (difference.Row + 1);

                string animationName = $"pacman{directions[index]}";
                Animation.Play(animationName);
            }
        }
    }
}
