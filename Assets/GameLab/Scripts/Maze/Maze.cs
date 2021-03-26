using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Maze : MonoBehaviour
{   [SerializeField]
    private int _mazeRows, _mazeColumns;
    [SerializeField]
    private GameObject[] _planets;
    [SerializeField]
    private float _spacing = 30f;
    [SerializeField]
    private int _planetSizeMinimum=2, _planetSizeMaximum=10;

    private MazeCells[,] maze;

    void Start()
    {
        InitializeMaze ();
        Algorithm alg = new HuntKill (maze);
        alg.CreateMaze();
    }

    void InitializeMaze ()
    {
        // Set size of grid
        maze = new MazeCells[_mazeRows, _mazeColumns];

        // Iterate maze rows
        for (int row = 0; row < _mazeRows; row++)
        {
            // Iterate maze columns
            for (int col = 0; col < _mazeColumns; col++)
            {
                // Set a maze cell for every element in the maze
                maze[row, col] = new MazeCells ();

                // Instantiate West planet
                if (col == 0)
                {
                    // Get a random prefab
                    GameObject planetW = _planets[Random.Range(0,_planets.Length)];
                    // Set random size given a range
                    int sizeW = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                    // Instantiate using Photon
                    maze[row,col].westPlanet = Instantiate(planetW, this.transform.position+new Vector3 (row * _spacing, 0, col*_spacing - _spacing/2f), Quaternion.identity);
                    // Set random size
                    maze[row,col].westPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeW;
                    // Set inside GameObject where maze is initiated for cleaner scene hierarchy
                    maze[row,col].westPlanet.transform.parent = this.transform;
                    // Change name of object with relative coordinate attached;
                    maze[row,col].westPlanet.name = "West Planet (" + row + ", " + col + ")";
                }

                // Instantiate East planet
                
                // Get a random prefab
                GameObject planetE = _planets[Random.Range(0,_planets.Length)];
                // Set random size given a range
                int sizeE = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                // Instantiate using Photon
                maze[row,col].eastPlanet = Instantiate(planetE, this.transform.position+new Vector3 (row * _spacing, 0,col*_spacing + _spacing/2f), Quaternion.identity);
                // Set random size
                maze[row,col].eastPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeE;
                // Set inside GameObject where maze is initiated for cleaner scene hierarchy
                maze[row,col].eastPlanet.transform.parent = this.transform;
                // Change name of object with relative coordinate attached;
                maze[row,col].eastPlanet.name = "East Planet (" + row + ", " + col + ")";

                // Instantiate North planet
                if (row == 0)
                {
                    // Get a random prefab
                    GameObject planetN = _planets[Random.Range(0,_planets.Length)];
                    // Set random size given a range
                    int sizeN = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                    // Instantiate using Photon
                    maze[row,col].northPlanet = Instantiate(planetN, this.transform.position+new Vector3 (row * _spacing - _spacing/2f, 0, col*_spacing), Quaternion.identity);
                    // Set random size
                    maze[row,col].northPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeN;
                    // Set inside GameObject where maze is initiated for cleaner scene hierarchy
                    maze[row,col].northPlanet.transform.parent = this.transform;
                    // Change name of object with relative coordinate attached;
                    maze[row,col].northPlanet.name = "North Planet (" + row + ", " + col + ")";
                }

                // Instantiate South planet
                
                // Get a random prefab
                GameObject planetS = _planets[Random.Range(0,_planets.Length)];
                // Set random size given a range
                int sizeS = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                // Instantiate using Photon
                maze[row,col].southPlanet = Instantiate(planetS, this.transform.position+new Vector3 (row * _spacing + _spacing/2f, 0, col*_spacing), Quaternion.identity);
                // Set random size
                maze[row,col].southPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeS;
                // Set inside GameObject where maze is initiated for cleaner scene hierarchy
                maze[row,col].southPlanet.transform.parent = this.transform;
                // Change name of object with relative coordinate attached;
                maze[row,col].southPlanet.name = "South Planet (" + row + ", " + col + ")";
                
            }
        }
    }
}

