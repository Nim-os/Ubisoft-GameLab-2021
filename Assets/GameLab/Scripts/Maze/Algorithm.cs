public abstract class Algorithm
{
    // Object-Oriented Programming implementation of algortihm class. Allows us to design and use different algorithms on a maze
    protected MazeCells[,] maze;

    protected int mazeRows, mazeColumns;

    protected Algorithm(MazeCells[,] maze) : base()
    {
        this.maze = maze;
        mazeRows = maze.GetLength(0);
        mazeColumns = maze.GetLength(1);
    }

    public abstract void CreateMaze();
}