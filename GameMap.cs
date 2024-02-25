﻿using GameAlgoT2310;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

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

            /********************************************************************************
                PROBLEM 4 : Construct tile graph from the food layer.

                HOWTOSOLVE : 1. Just uncomment the given code.

                TileGraph = new TileGraph();
                TileGraph.CreateFromTiledMapTileLayer(foodLayer, StartColumn, StartRow);
            ********************************************************************************/
            TileGraph = new TileGraph();
            TileGraph.CreateFromTiledMapTileLayer(foodLayer, StartColumn, StartRow);

        }

        protected override void LoadContent()
        {
            TiledMap = _game.Content.Load<TiledMap>("Project");
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
