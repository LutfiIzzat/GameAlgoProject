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
        private int _normalEnemyCount;
        private int _giantEnemyCount;
        private int _flyingEnemyCount;
        private Player _player;
        public GameMap gameMap;

        public EnemyFactory(Player player, GameMap map) 
        {
            _player = player;
            _random = new Random();
            _normalEnemyCount = 0;
            _giantEnemyCount = 0;
            _flyingEnemyCount = 0;
            gameMap = map;
            
        }

        public GameObject CreateRandomEnemy(Player player)
        {
            int randomNumber = _random.Next(3);
            EnemyType randomEnemyType = (EnemyType)randomNumber;

            switch (randomEnemyType)
            {
                case EnemyType.Normal:
                    _normalEnemyCount++;
                    return new Zombie(player, $"NormalEnemy_{_normalEnemyCount}");
                case EnemyType.Giant:
                    _giantEnemyCount++;
                    return new GiantEnemy(player, $"GiantEnemy_{_giantEnemyCount}");
                case EnemyType.Flying:
                    _flyingEnemyCount++;
                    return new FlyingEnemy(player, $"FlyingEnemy_{_flyingEnemyCount}");
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
