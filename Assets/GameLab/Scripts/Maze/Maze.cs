using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Maze : MonoBehaviour
{   [SerializeField]
    private int _mazeRows, _mazeColumns;
    [SerializeField]
    private float _spacing = 30f;
    [SerializeField]
    private int _planetSizeMinimum=2, _planetSizeMaximum=10;

    private MazeCells[,] maze;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeMaze ();
            Algorithm alg = new HuntKill (maze);
            alg.CreateMaze();
        }
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
                    // Set random size given a range
                    int sizeW = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                    // Instantiate using Photon
                    maze[row,col].westPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing, 0, col*_spacing - _spacing/2f), Quaternion.identity);
                    // Set random size
                    maze[row,col].westPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeW;
                    // Set inside GameObject where maze is initiated for cleaner scene hierarchy
                    maze[row,col].westPlanet.transform.parent = this.transform;
                    // Change name of object with relative coordinate attached;
                    maze[row,col].westPlanet.name = "West Planet (" + row + ", " + col + ")";
                    // Change rigid body mass
                    float newMassW = ((float) sizeW)/2;
                    maze[row,col].westPlanet.GetComponent<Rigidbody>().mass = newMassW;
                }

                // Instantiate East planet
                
                int sizeE = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                maze[row,col].eastPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing, 0,col*_spacing + _spacing/2f), Quaternion.identity);
                maze[row,col].eastPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeE;
                maze[row,col].eastPlanet.transform.parent = this.transform;
                maze[row,col].eastPlanet.name = "East Planet (" + row + ", " + col + ")";
                float newMassE = ((float) sizeE)/2;
                maze[row,col].eastPlanet.GetComponent<Rigidbody>().mass = newMassE;

                // Instantiate North planet
                if (row == 0)
                {
                    int sizeN = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                    maze[row,col].northPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing - _spacing/2f, 0, col*_spacing), Quaternion.identity);
                    maze[row,col].northPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeN;
                    maze[row,col].northPlanet.transform.parent = this.transform;
                    maze[row,col].northPlanet.name = "North Planet (" + row + ", " + col + ")";
                    float newMassN = ((float) sizeN)/2;
                    maze[row,col].eastPlanet.GetComponent<Rigidbody>().mass = newMassN;
                }

                // Instantiate South planet
                
                int sizeS = Random.Range(_planetSizeMinimum, _planetSizeMaximum+1);
                maze[row,col].southPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing + _spacing/2f, 0, col*_spacing), Quaternion.identity);
                maze[row,col].southPlanet.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * sizeS;
                maze[row,col].southPlanet.transform.parent = this.transform;
                maze[row,col].southPlanet.name = "South Planet (" + row + ", " + col + ")";
                float newMassS = ((float) sizeS)/2;
                maze[row,col].eastPlanet.GetComponent<Rigidbody>().mass = newMassS;
            }
        }
    }
}

