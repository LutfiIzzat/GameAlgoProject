using GameAlgoProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame
{
    public class PacmanScene
    {
        public float EnemyCount;
        public const float MaxEnemy = 20;
        private const float _spawnInterval = 1f;
        public Player Player;
        private float _spawnTimer = 0f;
        public PacmanScene()
        {
            // Game map
            GameMap gameMap = new("GameMap");
            gameMap.StartColumn = 20;
            gameMap.StartRow = 11;

            // Pathfinding Tester
            //PathfindingTester pathfindingTester = new PathfindingTester("yaur");

            // Ghost
            //Ghost ghost = new Ghost();
            Player = new Player();
            Zombie zombie = new Zombie(Player, "normalZombie");
            //GameObjectCollection.Add(zombie);
            //Zombie zombie1 = new GiantEnemy(Player, "giantZombie");
            //Zombie zombie2 = new FlyingEnemy(Player, "flyingZombie");
            _spawnTimer = 0f;
            
        }
        public void Update()
        {
            _spawnTimer += ScalableGameTime.DeltaTime;
            if (EnemyCount < MaxEnemy && _spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0f;
                EnemyFactory enemyFactory = new EnemyFactory(Player);
                enemyFactory.CreateRandomEnemy(Player);
                EnemyCount++;
            }

        }

    }
}
