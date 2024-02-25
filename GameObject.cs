﻿using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Xml.Linq;

namespace GameAlgoT2310
{
    public class GameObject
    {
        #region Object Attributes
        
        // Name is unique
        public string Name
        {
            get; private set;
        }

        // Tag is used to group game objects.
        // TOIMPROVE: Support multiple tags per object instead of one.
        private string _tag = string.Empty;
        public string Tag
        {
            get { return _tag; }
            set
            {
                // Tag is a lowercase text (e.g. "PLaYer", "pLayeR" should be "player")
                string valueLowercase = value.ToLower();
                if (GameObjectCollection.ChangeTag(this, valueLowercase))
                {
                    _tag = valueLowercase;
                }
            }
        }

        // Transformation Data
        public Vector2 Origin    = Vector2.Zero;
        public Vector2 Position  = Vector2.Zero;
        public float Orientation = 0f;
        public Vector2 Scale     = Vector2.One;

        // Draw Layer: the layer on which the game object is drawn
        private uint _layer = 0;
        public uint Layer
        {
            get { return _layer; }
            set
            {
                if (GameObjectCollection.ChangeLayer(this, value))
                {
                    _layer = value;
                }
            }

        }

        public bool Visible = true; 

        #endregion

        #region Static Object Attributes

        // Reference to game engine
        protected static GameEngine _game;

        public static void SetGame(GameEngine game)
        {
            // Allow one assignment only
            if (_game == null)
            {
                _game = game;
            }
        }

        public static uint GetFrontMostLayer()
        {
            var keys = GameObjectCollection.Layers.Keys;
            return keys.Last();
        }

        #endregion

        protected GameObject()
        {
            // Provide a default name based on the object's actual type and number of objects
            Name = $"{this.GetType().Name}_{GameObjectCollection.Count()}";
            GameObjectCollection.Add(this);
        }

        protected GameObject(string name)
        {
            Name = name;
            GameObjectCollection.Add(this);
        }

        // NOTE: This "copy constructor" has not been tested.
        protected GameObject(GameObject original)
        {
            // Clone's name cannot be the same as the original
            Name = $"{original.Name}_Clone_{GameObjectCollection.Count()}";
            Position = original.Position;
            Origin = original.Origin;
            Orientation = original.Orientation;
            Scale = original.Scale;

            GameObjectCollection.Add(this);
        }

        public virtual void Initialize() { }

        protected virtual void LoadContent() { }

        public virtual void Start() { }

        public virtual void Update() { }

        public virtual void Draw() { }
    }
}
