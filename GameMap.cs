using GameAlgoProject;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class GameMap : GameObject
    {
        public TiledMap TiledMap { get; private set; }
        public TiledMapRenderer TiledMapRenderer { get; private set; }
        public TileGraph TileGraph { get; private set; }

        // Defines the row and column of any navigable tile to construct the tile graph
        public ushort StartColumn;
        public ushort StartRow;

        public GameMap(string name) : base(name)
        {
        }

        public override void Initialize()
        {
            LoadContent();

            // Initialize tiled map renderer
            TiledMapRenderer = new TiledMapRenderer(_game.GraphicsDevice, TiledMap);

            // Get the Food layer from the tiled map
            TiledMapTileLayer foodLayer = TiledMap.GetLayer<TiledMapTileLayer>("Food");
            TiledMapTileLayer waypointsLayer = TiledMap.GetLayer<TiledMapTileLayer>("Waypoints");

            /********************************************************************************
                PROBLEM 4 : Construct tile graph from the food layer.

                HOWTOSOLVE : 1. Just uncomment the given code.

                TileGraph = new TileGraph();
                TileGraph.CreateFromTiledMapTileLayer(foodLayer, StartColumn, StartRow);
            ********************************************************************************/

            TileGraph = new TileGraph();
            TileGraph.CreateFromTiledMapTileLayer(foodLayer, StartColumn, StartRow);
            //TileGraph.CreateFromTiledMapTileLayer(waypointsLayer, StartColumn, StartRow);
        }

        protected override void LoadContent()
        {
            //TiledMap = _game.Content.Load<TiledMap>("pacman-with-home");
            TiledMap = _game.Content.Load<TiledMap>("map");
        }

        public override void Update()
        {
            TiledMapRenderer.Update(ScalableGameTime.GameTime);
        }

        public override void Draw()
        {
            TiledMapRenderer.Draw();
        }
    }
}
