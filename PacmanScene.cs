using PacmanGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public class PacmanScene
    {
        public PacmanScene()
        {
            // Game map
            GameMap gameMap = new("GameMap");
            gameMap.StartColumn = 20;
            gameMap.StartRow = 11;

            // Pathfinding Tester
            //PathfindingTester pathfindingTester = new PathfindingTester("yaur");

            // Ghost
            Ghost ghost = new Ghost();
            Player player = new Player();
        }
    }
}
