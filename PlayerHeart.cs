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
    public class PlayerHeart : GameObject
    {

        // Attributes
        public Texture2D Texture;
        public const float MaxHealth = 3f;
        public float Health;
        public float scale;

        public PlayerHeart() : base()
        {
        }

        public override void Initialize()
        {
            LoadContent();
            scale = 1.75f;
            Health = MaxHealth;
            
        }

        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("lifebar_16x16");

        }

        public override void Start()
        {
            //Position = new Vector2(750, 50);

        }



        

        public override void Update()
        {

            

        }


        public override void Draw()
        {
            // Define the source rectangle for the first quarter of the texture
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width / 2, Texture.Height / 2);

            // Define the vertical spacing between hearts
            int verticalSpacing = 11;

            // Draw the first heart
            Vector2 position = Position;
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, position, sourceRectangle, Color.White, Orientation, Origin, scale, SpriteEffects.None, 0f);
            _game.SpriteBatch.End();

            // Draw the second heart
            position.Y += Texture.Height / 2 + verticalSpacing;
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, position, sourceRectangle, Color.White, Orientation, Origin, scale, SpriteEffects.None, 0f);
            _game.SpriteBatch.End();

            // Draw the third heart
            position.Y += Texture.Height / 2 + verticalSpacing;
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, position, sourceRectangle, Color.White, Orientation, Origin, scale, SpriteEffects.None, 0f);
            _game.SpriteBatch.End();
        }

    }
}

