using GameAlgoProject;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using System;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using Autofac.Core.Lifetime;

namespace ZombieGame
{
    public class EnemyFactory
    {
        private Random _random;

        public EnemyFactory()
        {
            _random = new Random();
        }

        public GameObject CreateRandomEnemy()
        {
            int randomNumber = _random.Next(3);
            EnemyType randomEnemyType = (EnemyType)randomNumber;

            switch (randomEnemyType)
            {
                case EnemyType.Normal:
                    return new NormalEnemy();
                case EnemyType.Giant:
                    return new GiantEnemy();
                case EnemyType.Flying:
                    return new FlyingEnemy();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public enum EnemyType
    {
        Normal,
        Giant,
        Flying
    }
}
