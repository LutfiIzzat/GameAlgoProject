using GameAlgoProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Diagnostics;

namespace ZombieGame
{
    public class NavigationHCFSM : HCFSM
    {
        // FSM for navigation
        public enum State { Stop, Moving };

        // Navigation current state
        public State CurrentState;

        // Navigation
        private Tile _srcTile;
        private Tile _destTile;
        private LinkedList<Tile> _path;

        private Enemy _owner;
        private TiledMap _tiledMap;
        private GameEngine _game;
        private TileGraph _tileGraph;

        private List<Tile> m_waypoints = new List<Tile>();
        private int _currentWaypointIndex;
        private float _timeSinceStop;

        private const float StopDuration = 3f;

        public NavigationHCFSM(GameEngine game, Enemy owner, TiledMap tiledMap, TileGraph tileGraph) : base()
        {
            _game = game;
            _owner = owner;
            _tiledMap = tiledMap;
            _tileGraph = tileGraph;
            CurrentState = State.Stop;

            InitialiseWaypoints();
            _currentWaypointIndex = 0;
        }

        public override void Initialize()
        {
            // Initialize source tile from owner's current position
            _srcTile = Tile.ToTile(_owner.Position, _tiledMap.TileWidth, _tiledMap.TileHeight);

            _currentWaypointIndex = 0;
            SetNextWaypoint();
        }

        private LinkedList<Tile> Roam_GeneratePathToWaypoint(TileGraph graph, Tile srcTile, int waypointIndex)
        {
            Tile waypoint = m_waypoints[waypointIndex];

            return AStar.Compute(graph, srcTile, waypoint, AStarHeuristic.EuclideanSquared);
        }

        private void InitialiseWaypoints()
        {
            TiledMapObjectLayer waypoints = _tiledMap.GetLayer<TiledMapObjectLayer>("Waypoints");
            Debug.WriteLine("waypoints Layer: " + waypoints);
            Debug.WriteLine($"Objects count in Waypoints layer: {waypoints.Objects.Length}");
            if (waypoints != null)
            {
                foreach (TiledMapObject waypointObject in waypoints.Objects)
                {
                    float waypointX = waypointObject.Position.X;
                    float waypointY = waypointObject.Position.Y;


                    int col = (int)(waypointX / _tiledMap.TileWidth);
                    int row = (int)(waypointY / _tiledMap.TileHeight);

                    Tile waypoint = new Tile(col, row);
                    m_waypoints.Add(waypoint);
                    Debug.WriteLine($"Current waypoint pos : {waypoint}");
                }
            }
        }


        private void SetNextWaypoint()
        {
            Debug.WriteLine($"Current waypoint index: {_currentWaypointIndex}");
            if (m_waypoints.Count > 0)
            {
                if (_currentWaypointIndex < m_waypoints.Count - 1)
                {
                    _currentWaypointIndex++;
                }
                else
                {
                    _currentWaypointIndex = 0;
                }

                _destTile = m_waypoints[_currentWaypointIndex];
                _path = Roam_GeneratePathToWaypoint(_tileGraph, _srcTile, _currentWaypointIndex);
                _path.RemoveFirst();

                _owner.UpdateAnimatedSprite(_srcTile, _path.First.Value);
            }
        }

        public override void Update()
        {
            MouseState mouse = Mouse.GetState();

            int tileWidth = _tiledMap.TileWidth;
            int tileHeight = _tiledMap.TileHeight;

            // Implement Hard-Coded FSM for movement behaviour

            if (CurrentState == State.Stop)
            {
                Debug.WriteLine($"Current destination tile : {_destTile}");
                _timeSinceStop += ScalableGameTime.DeltaTime;
                // Transition to Moving state when reaching the current waypoint
                if (m_waypoints.Count > 0 )
                {
                    CurrentState = State.Moving;
                    SetNextWaypoint();
                }


            }
            else if (CurrentState == State.Moving)
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

                if (_owner.Position.Equals(Tile.ToPosition(_destTile, tileWidth, tileHeight))
                   )
                {
                    // Update source tile to destination tile
                    _srcTile = _destTile;

                    // Change to STOP state
                    CurrentState = State.Stop;
                    _timeSinceStop = 0f;
                }

                // Action to execute on the MOVING state
                else
                {
                    Tile headTile = _path.First.Value; // throw exception if path is empty

                    Vector2 headTilePosition = Tile.ToPosition(headTile, tileWidth, tileHeight);

                    if (_owner.Position.Equals(headTilePosition))
                    {
                        Debug.WriteLine($"Reach head tile (Col = {headTile.Col}, Row = {headTile.Row}).");

                        /********************************************************************************
                            PROBLEM 4(D): Update the animation based on the previous and current tile
                                            the player was in.


                            HOWTOSOLVE : 1. Copy the code below.
                                         2. Paste it below this block comment.
                                         3. Fill in the blanks.

                            Tile nextTile = _path.First.________.________;
                            UpdateAnimatedSprite(headTile, ________);

                        ********************************************************************************/

                        Tile nextTile = _path.First.Next.Value;
                        _owner.UpdateAnimatedSprite(headTile, nextTile);

                        Debug.WriteLine($"Removing head tile. Get next node from path.");
                        _path.RemoveFirst();

                        // Get the next destination position
                        headTile = _path.First.Value; // throw exception if path is empty
                    }

                    // Move the ghost to the new tile location
                    _owner.Position = _owner.Move(_owner.Position, headTilePosition, elapsedSeconds);

                    /********************************************************************************
                        PROBLEM 4(D): Update the ghost animation.


                        HOWTOSOLVE : 1. Copy the code below.
                                        2. Paste it below this block comment.
                                        3. Fill in the blanks.

                        Animation.Update(________);

                    ********************************************************************************/

                    _owner.Animation.Update(ScalableGameTime.GameTime);
                }
            }
        }
    }
}
