using GameAlgoProject;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame
{
    public class PacmanScene : GameObject
    {
        public float EnemyCount;
        public const float MaxEnemy = 20;
        private const float _spawnInterval = 1f;
        public Player Player;
        private float _spawnTimer = 0f;
        private List<Zombie> zombies;
        public GameMap gameMap;
        public PacmanScene()
        {
            // Game map
            gameMap = new GameMap("GameMap");
            gameMap.StartColumn = 20;
            gameMap.StartRow = 11;

            // Pathfinding Tester
            //PathfindingTester pathfindingTester = new PathfindingTester("yaur");

            // Ghost
            //Ghost ghost = new Ghost();
            Player = new Player();
            PlayerHeart PlayerHeart = new PlayerHeart();
            Zombie zombie = new Zombie(Player, "normalZombie");
            GiantEnemy zombie1 = new GiantEnemy(Player, "giantZombie");
            FlyingEnemy zombie2 = new FlyingEnemy(Player, "flyingZombie");
            //GiantEnemy zombie3 = new GiantEnemy(Player, "giantZombie");

            _spawnTimer = 0f;
            
        }
        public override void Update()
        {
            //Debug.WriteLine(gameMap.Name);
            _spawnTimer += ScalableGameTime.DeltaTime;
            if (_spawnTimer >= 2 && _spawnTimer < 3)
            {
                _spawnTimer = 3f;
                //Zombie zombie4 = new Zombie(Player, "normalZombie1", gameMap);
                //FlyingEnemy zombie5 = new FlyingEnemy(Player, "flyingZombie1");
                //Zombie zombie6 = new Zombie(Player, "normalZombie2");
                //GiantEnemy zombie7 = new GiantEnemy(Player, "giantZombie1");
            }
            if (_spawnTimer >=  5 && _spawnTimer < 6)
            {
                //Zombie zombie5 = new Zombie(Player, "normalZombie5");
                //FlyingEnemy zombie6 = new FlyingEnemy(Player, "flyingZombie6");
                //GiantEnemy zombie7 = new GiantEnemy(Player, "giantZombie7");
                //_spawnTimer = 0;
            }

        }

    }
}
