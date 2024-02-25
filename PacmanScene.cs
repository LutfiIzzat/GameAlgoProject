
namespace PacmanGame
{
    public class PacmanScene
    {
        public PacmanScene()
        {
            // Game map
            GameMap gameMap = new GameMap("GameMap");
            gameMap.StartColumn = 18;
            gameMap.StartRow = 11;

            // Pathfinding Tester
            // PathfindingTester pathfindingTester = new PathfindingTester();

            // Ghost
            //Ghost ghost = new Ghost();

			// Pac Man
            //PacMan pacman = new PacMan();
        }
    }
}
