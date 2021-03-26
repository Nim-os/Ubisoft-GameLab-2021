using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HuntKill : Algorithm
{
    private int currentRow = 0;
    private int currentCol = 0;
    private bool finishedMaze = false;

    public HuntKill(MazeCells[,] maze) : base(maze)
    {}

    public override void CreateMaze()
    {
        HuntAndKill ();
    }

    // Checking for available route from cuttent cell

    private bool PathStillAvailable(int row, int col)
    {
        int availablePaths = 0;
        if  (row > 0 && !maze[row-1,col].visited) availablePaths++;
        if  (col > 0 && !maze[row,col-1].visited) availablePaths++;
        if  (row < mazeRows-1 && !maze[row+1,col].visited) availablePaths++;
        if  (col < mazeColumns-1 && !maze[row,col+1].visited) availablePaths++;
        return availablePaths > 0;
    }

    // Check if a cell is available in grid
    private bool CellIsAvailable(int row, int col)
    {
        if  (row >= 0   &&  row < mazeRows  && col >= 0 && col < mazeColumns && !maze[row,col].visited) return true;
        else return false;
    }

    // Check if an adjacent cell is visited
    private bool AdjacentCellIsVisited(int row, int col)
    {
        int visitedCells = 0;
        if  (row > 0 && maze[row-1,col].visited) visitedCells++;
        if  (col > 0 && maze[row,col-1].visited) visitedCells++;
        if  (row < mazeRows-1 && maze[row+1,col].visited) visitedCells++;
        if  (col < mazeColumns-1 && maze[row,col+1].visited) visitedCells++;
        return visitedCells > 0;
    }

    // Destroy planets
    private void DeathStar(GameObject planet)
    {
        if (planet != null)
        {
            // Destroy;
            GameObject.Destroy(planet);
        }
    }

    // Destroy adjacent walls
    private void TheGalacticEmpire(int row, int col)
    {
        bool destroyed = false;

        while (!destroyed)
        {
            // Random direction at each step
            int direction = Random.Range(1,5);

            if (direction == 1 && row > 0 && maze[row-1, col].visited)
            {
                DeathStar(maze[row,col].northPlanet);
                DeathStar(maze[row-1,col].southPlanet); // Overkill, making sure no doubles occur.
                destroyed = true;
            } else if (direction == 2 && row < mazeRows - 2 && maze[row+1, col].visited)
            {
                DeathStar(maze[row,col].southPlanet);
                DeathStar(maze[row+1,col].northPlanet); // Overkill, making sure no doubles occur.
                destroyed = true;
            } else if (direction == 3 && col > 0 && maze[row, col-1].visited)
            {
                DeathStar(maze[row,col].westPlanet);
                DeathStar(maze[row,col-1].eastPlanet); // Overkill, making sure no doubles occur.
                destroyed = true;
            } else if (direction == 4 && col < mazeColumns - 2 && maze[row, col+1].visited)
            {
                DeathStar(maze[row,col].eastPlanet);
                DeathStar(maze[row,col+1].westPlanet); // Overkill, making sure no doubles occur.
                destroyed = true;
            }
        }
    }

    // Hunt and Kill algorithm
    private void HuntAndKill()
    {
        maze[currentRow, currentCol].visited = true;
        while(!finishedMaze)
        {
            Kill();
            Hunt();
        }
    }

    // Walks randomly destroying planets along the way. When it encounters a dead end, it moves to Hunt()
    private void Kill()
    {
        while(PathStillAvailable(currentRow, currentCol))
        {
            int direction = Random.Range(1,5);

            // Kill north
            if (direction == 1 && CellIsAvailable(currentRow - 1, currentCol))
            {
                DeathStar(maze[currentRow, currentCol].northPlanet);
                DeathStar(maze[currentRow - 1, currentCol].southPlanet);
                currentRow --;
            } else if (direction == 2 && CellIsAvailable(currentRow + 1, currentCol))
            // Kill south
            {
                DeathStar(maze[currentRow, currentCol].southPlanet);
                DeathStar(maze[currentRow + 1, currentCol].northPlanet);
                currentRow ++;
            } else if (direction == 3 && CellIsAvailable(currentRow, currentCol+1))
            {
            // Kill east
                DeathStar(maze[currentRow, currentCol].eastPlanet);
                DeathStar(maze[currentRow, currentCol+1].westPlanet);
                currentCol ++;
            } else if (direction == 4 && CellIsAvailable(currentRow, currentCol-1))
            {
            // KillWest
                DeathStar(maze[currentRow, currentCol].westPlanet);
                DeathStar(maze[currentRow, currentCol-1].eastPlanet);
                currentCol --;
            } 
            maze[currentRow, currentCol].visited = true;
        }
    }

    // Hunts for next unvisited cell with an adjacent visited cell. Ends algorithm when no more unvisited cells are left in grid.
    private void Hunt ()
    {
        finishedMaze = true;
        for (int rows = 0; rows < mazeRows; rows++)
        {
            for (int cols=0; cols < mazeColumns; cols++)
            {
                if (!maze[rows, cols].visited && AdjacentCellIsVisited(rows, cols))
                {
                    finishedMaze = false;
                    currentRow = rows;
                    currentCol = cols;
                    TheGalacticEmpire(currentRow, currentCol);
                    maze[currentRow, currentCol].visited = true;
                    return;
                }
            }
        }
    }

}