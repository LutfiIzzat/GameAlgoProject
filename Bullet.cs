using GameAlgoT2310;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class Bullet : GameObject
    {

        public Texture2D Texture;
        public GameObject ParentObject;
        public float Speed;
        public Vector2 Velocity;
        public bool inverted = false;
        public Vector2 offset;
        public Vector2 mousePosition;
        private Vector2 direction;

        public Bullet(GameObject parentObject) : base()
        {
            ParentObject = parentObject;
        }

        public override void Initialize()
        {
            LoadContent();
            Origin = new(Texture.Width / 2f, Texture.Height / 2f);
            Orientation = ParentObject.Orientation;
            direction = new((float)Math.Cos(Orientation), (float)Math.Sin(Orientation));

/*            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();
            Vector2 offset = mousePosition - Position;
            Orientation = (float)Math.Atan2(offset.Y, offset.X);*/

            Speed = 100f;
        }


        protected override void LoadContent()
        {
            Texture = _game.Content.Load<Texture2D>("bullet1");
        }

        public override void Update()
        {
            //direction = new((float)Math.Cos(Orientation), (float)Math.Sin(Orientation));
            Velocity = direction * Speed;
            Position += Velocity * ScalableGameTime.DeltaTime;
        }

        public override void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(Texture, Position, null, Color.White,
                                    Orientation, Origin, 0.1f, SpriteEffects.None,
                                    0f);

            _game.SpriteBatch.End();
        }
    }
}
