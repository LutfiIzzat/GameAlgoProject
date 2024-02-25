﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using ZombieGame;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace GameAlgoProject
{
    public class GameEngine : Game
    {
        // Graphics Device and Sprite Batch made public
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        // Engine Related
        public CollisionEngine CollisionEngine;
        public EnemyFactory EnemyFactory;
        public Random Random;
        private int _enemyCount;
        private float _spawnTimer;
        private const float SpawnInterval = 3f;
        private const int MaxEnemies = 20;

        public GameEngine()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Initialize Game Object
            GameObject.SetGame(this);

            // Initialize Engines
            CollisionEngine = new CollisionEngine();
            Random = new Random();

            // Initialize Scalable Game Time
            ScalableGameTime.TimeScale = 1f;
            EnemyFactory = new EnemyFactory();
        } 

        protected override void Initialize()
        {
            LoadContent();

            // Set back buffer
            Graphics.PreferredBackBufferWidth = 768;
            Graphics.PreferredBackBufferHeight = 768;
            Graphics.ApplyChanges();

            // Construct game objects here.           
            // CreatePacmanWorld();
            PacmanScene scene = new PacmanScene();
            Player player = new Player();
 
            _enemyCount = 0;
            _spawnTimer = 0f;
            // Initialize all game objects
            GameObjectCollection.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // [Optional] Pre-load all assets here (e.g. textures, sprite font, etc.)
            // e.g. Content.Load<Texture2D>("texture-name")

        }

        protected override void Update(GameTime gameTime)
        {
            // Compute scaled time
            ScalableGameTime.Process(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _spawnTimer += ScalableGameTime.DeltaTime;
            if (_enemyCount < 20 && _spawnTimer >= SpawnInterval)
            {
                _spawnTimer = 0f;
                Enemy randomEnemy = (Enemy)EnemyFactory.CreateRandomEnemy();
                _enemyCount++;
                GameObjectCollection.Add(randomEnemy);
            }
            // Update Game Objects
            GameObjectCollection.Update();

            // Update Collision Engine
            CollisionEngine.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameObjectCollection.Draw();
        }

        protected override void EndDraw()
        {
            base.EndDraw();
            GameObjectCollection.EndDraw();
        }
    }
}