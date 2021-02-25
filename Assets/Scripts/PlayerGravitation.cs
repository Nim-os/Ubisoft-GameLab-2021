using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class PlayerGravitation : MonoBehaviour
{
    public List<BaseGravitation> playerGravityCheckList = new List<BaseGravitation>();
    public BaseGravitation currentSelection = null;

    private float range = 10f; // mouse position hold range

     void FixedUpdate()
    {
        if (gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine)
		{
            ToggleOnGravity();
        }
    }

    /// <summary> Adds gravitational object to playerGravityCheck list</summary>
    public void AddToGravityCheck(BaseGravitation p){
        playerGravityCheckList.Add(p);
    }

    /// <summary> Removes gravitational object from playerGravityCheck list</summary>
    public void RemoveFromGravityCheck(BaseGravitation p){
        playerGravityCheckList.Remove(p);
    }

    /// <summary> Removes gravitational object from playerGravityCheck list</summary>
    private void ToggleOnGravity(){
        // holding RMB
        if (Input.GetMouseButton(1)){
            BaseGravitation o = SelectGravitationalObject();

            // if selects an object
            if (o != null){
                o.playerSelected = true;
                currentSelection = o;
            }
        // else not holding RMB OR no objects to select
        }else {
            if (currentSelection != null){
                currentSelection.playerSelected = false;
            }
            currentSelection = null;
        }
    }

    private BaseGravitation SelectGravitationalObject(){
        // if there is only one to check && it is within distance, pick this one
        if ((playerGravityCheckList.Count == 1) && (Utils.DistanceMouseObj(playerGravityCheckList[0].gameObject) <= range)){
            return playerGravityCheckList[0];
        
        // if there are multiple to check
        }else if (playerGravityCheckList.Count > 1){
            float lowestDist = float.MaxValue;
            BaseGravitation lowestDistObj = null;
            
            // for each gravitational object to check
            foreach (BaseGravitation gravObj in playerGravityCheckList) {
                // if within range && less than current range
                var distObjMouse = Utils.DistanceMouseObj(gravObj.gameObject);
                if ((distObjMouse <= range) && (distObjMouse < lowestDist)){
                    lowestDistObj = gravObj;
                }
            }
            
            if (lowestDistObj != null){
                return lowestDistObj;
            }
        }
        return null;
    }
}
