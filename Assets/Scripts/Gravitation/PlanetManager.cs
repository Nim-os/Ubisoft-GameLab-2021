using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanetManager : MonoBehaviour
{
    //////////////fields//////////////
    public float Gravity_value = 6.67f;//G used in calculate gravity
    public GameObject player;//player reference
    public GameObject planet;//planet object reference
    List<Planet> planets=new List<Planet>();//all of the planets generated would be stored here


    // Start is called before the first frame update
    void Start()
    {
        generatePlanet();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //try to check whether the player is in range with any exsisted planet
        if(planets.Count!=0){
            foreach(Planet p in planets){
                float mag=Math.Abs((player.GetComponent<Planet>().rb.position-p.rb.position).magnitude);
                
                //apply third law if player is within the radius
                if(mag<p.affect_radius){
                    
                    player.GetComponent<Planet>().ThirdLaw(p, Gravity_value);
                    p.ThirdLaw(player.GetComponent<Planet>(), Gravity_value);
                }
            }
        }
    }

    

    //generate planets under certain condition
    void generatePlanet(){
        //idea: maybe generate the planet based on player's current position
        //generate it a bit off screen so they have time to react

        //testing
        GameObject plnt = Instantiate(planet, new Vector3(player.transform.position.x+10, player.transform.position.y+10, player.transform.position.z), Quaternion.identity);
        planets.Add(plnt.GetComponent<Planet>());
    }

    
}
