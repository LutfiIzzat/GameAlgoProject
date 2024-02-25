using GameAlgoProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame
{
    public class Bullet : GameObject, ICollidable
    {

        public Texture2D Texture;
        public GameObject ParentObject;
        public float Speed;
        public Vector2 Velocity;
        public bool inverted = false;
        public Vector2 offset;
        public Vector2 mousePosition;
        private Vector2 direction;
        private Rectangle _bound;
        public float scale;

        public Bullet(GameObject parentObject) : base()
        {
            ParentObject = parentObject;
        }

        public override void Initialize()
        {
            LoadContent();
            scale = 0.1f;
            Origin = new(Texture.Width / 2f * scale, Texture.Height / 2f * scale);
            Orientation = ParentObject.Orientation;
            direction = new((float)Math.Cos(Orientation), (float)Math.Sin(Orientation));



            /*            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();
                        Vector2 offset = mousePosition - Position;
                        Orientation = (float)Math.Atan2(offset.Y, offset.X);*/

            Speed = 100f;
            _bound.Width = (int)(Texture.Width * scale);
            _bound.Height = (int)(Texture.Height * scale);
            _bound.Location = Position.ToPoint();


            _game.CollisionEngine.Listen(typeof(Bullet), typeof(Zombie), CollisionEngine.AABB);
            _game.CollisionEngine.Listen(typeof(Bullet), typeof(FlyingEnemy), CollisionEngine.AABB);
            _game.CollisionEngine.Listen(typeof(Bullet), typeof(GiantEnemy), CollisionEngine.AABB);

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
                                    Orientation, Origin, scale, SpriteEffects.None,
                                    0f);

            _game.SpriteBatch.End();
        }

        public string GetGroupName()
        {
            return this.GetType().Name;
        }

        public Rectangle GetBound()
        {
            _bound.Location = (Position - Origin).ToPoint();
            return _bound;
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
           if (collisionInfo.Other is Zombie)
            {
                GameObjectCollection.DeInstantiate(this);
            }
        }
    }
}
