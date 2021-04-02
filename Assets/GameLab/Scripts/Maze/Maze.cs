using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Maze : MonoBehaviour
{   [SerializeField]
    private int _mazeRows, _mazeColumns;
    [SerializeField]
    private float _spacing = 30f;
    [SerializeField]
    private float _planetSizeMinimum, _planetSizeMaximum;
    

    private Vector3 _referencedOrigin;

    int count = 0;

    private Player[] players = PhotonNetwork.PlayerList;

    private MazeCells[,] maze;

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && count < 1)
        {
            InitializeMaze ();
            Algorithm alg = new HuntKill (maze);
            alg.CreateMaze();
            count++;
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
                    float sizeW = Random.Range(_planetSizeMinimum, _planetSizeMaximum);
                    // Instantiate using Photon
                    maze[row,col].westPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing, 0, col*_spacing - _spacing/2f), Quaternion.identity);
                    // Set random size
                    maze[row,col].westPlanet.transform.localScale = new Vector3(sizeW, sizeW, sizeW);
                    // Set inside GameObject where maze is initiated for cleaner scene hierarchy
                    maze[row,col].westPlanet.transform.parent = this.transform;
                    // Change name of object with relative coordinate attached;
                    maze[row,col].westPlanet.name = "West Planet (" + row + ", " + col + ")";
                    // Change rigid body mass
                    maze[row,col].westPlanet.GetComponent<Rigidbody>().mass = sizeW;
                }

                // Instantiate East planet

                float sizeE = Random.Range(_planetSizeMinimum, _planetSizeMaximum);
                maze[row,col].eastPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing, 0,col*_spacing + _spacing/2f), Quaternion.identity);
                maze[row,col].eastPlanet.transform.localScale = new Vector3(sizeE, sizeE, sizeE);
                maze[row,col].eastPlanet.transform.parent = this.transform;
                maze[row,col].eastPlanet.name = "East Planet (" + row + ", " + col + ")";
                maze[row,col].eastPlanet.GetComponent<Rigidbody>().mass = sizeE;

                // Instantiate North planet
                if (row == 0)
                {
                    float sizeN = Random.Range(_planetSizeMinimum, _planetSizeMaximum);
                    maze[row,col].northPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing - _spacing/2f, 0, col*_spacing), Quaternion.identity);
                    maze[row,col].northPlanet.transform.localScale = new Vector3(sizeN, sizeN, sizeN);
                    maze[row,col].northPlanet.transform.parent = this.transform;
                    maze[row,col].northPlanet.name = "North Planet (" + row + ", " + col + ")";
                    maze[row,col].northPlanet.GetComponent<Rigidbody>().mass = sizeN;
                }

                // Instantiate South planet

                float sizeS = Random.Range(_planetSizeMinimum, _planetSizeMaximum);
                maze[row,col].southPlanet = PhotonNetwork.Instantiate("BasicPlanet", this.transform.position+new Vector3 (row * _spacing + _spacing/2f, 0, col*_spacing), Quaternion.identity);
                maze[row,col].southPlanet.transform.localScale = new Vector3(sizeS, sizeS, sizeS);
                maze[row,col].southPlanet.transform.parent = this.transform;
                maze[row,col].southPlanet.name = "South Planet (" + row + ", " + col + ")";
                maze[row,col].southPlanet.GetComponent<Rigidbody>().mass = sizeS;
            }
        }
    }
}

