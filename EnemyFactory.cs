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
        private int _enemyCount;

        public EnemyFactory()
        {
            _random = new Random();
            _enemyCount = 0;
        }

        public GameObject CreateRandomEnemy()
        {
            _enemyCount++;
            int randomNumber = _random.Next(3);
            EnemyType randomEnemyType = (EnemyType)1;

            switch (randomEnemyType)
            {
                case EnemyType.Normal:
                    return new NormalEnemy($"NormalEnemy{_enemyCount}");
                case EnemyType.Giant:
                    return new GiantEnemy($"GiantEnemy{_enemyCount}");
                case EnemyType.Flying:
                    return new FlyingEnemy($"FlyingEnemy{_enemyCount}");
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
