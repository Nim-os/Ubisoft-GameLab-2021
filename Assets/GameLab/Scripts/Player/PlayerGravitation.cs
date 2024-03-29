using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Basic gravitational script </summary>
public class PlayerGravitation : MonoBehaviour
{
    public InputSystem input;

    public List<BaseGravitation> playerGravityCheckList = new List<BaseGravitation>();
    public BaseGravitation currentSelection = null;

    private float range = 10f; // mouse position hold range
    private bool isMe, holdingRMB = false;

    private Vector2 mousePos = Vector2.zero;

    void Awake()
	{
        isMe = gameObject.GetComponent<Photon.Pun.PhotonView>().IsMine;

        input = new InputSystem();

        if (isMe)
		{
            input.Game.Secondary.performed += x => holdingRMB = true;
            input.Game.Secondary.canceled += x => holdingRMB = false;

            input.Game.MousePosition.performed += x => mousePos = x.ReadValue<Vector2>();
        }
    }

	void FixedUpdate()
    {
        if (isMe && !PauseMenu.GameIsPaused)
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
    private void ToggleOnGravity()
    {
        BaseGravitation o;

        // holding RMB
        if (holdingRMB && (o = SelectGravitationalObject()) != null) // In theory this should work - Holding RMB & No objects nearly -> Else statement executed
        {
            // if selects an object
            if (o != null){
                o.playerSelected = true;
                currentSelection = o;
                o.ShowIndicator();
            }
        }
        // else not holding RMB OR no objects to select
        else
        {
            if (currentSelection != null)
            {
                currentSelection.playerSelected = false;
                currentSelection.HideIndicator();
            }
            currentSelection = null;

        }
    }

    private BaseGravitation SelectGravitationalObject(){
        BaseGravitation selectedObj = null;
        // if there is only one to check && it is within distance, pick this one
        if ((playerGravityCheckList.Count == 1) && (Utils.DistanceMouseObj(mousePos, playerGravityCheckList[0].gameObject) <= range)){
            selectedObj = playerGravityCheckList[0];
        
        // if there are multiple to check
        }else if (playerGravityCheckList.Count > 1){
            float lowestDist = float.MaxValue;
            BaseGravitation lowestDistObj = null;
            
            // for each gravitational object to check
            foreach (BaseGravitation gravObj in playerGravityCheckList) {
                // if within range && less than current range
                var distObjMouse = Utils.DistanceMouseObj(mousePos, gravObj.gameObject);
                if ((distObjMouse <= range) && (distObjMouse < lowestDist)){
                    lowestDistObj = gravObj;
                }
            }
            
            if (lowestDistObj != null){
                selectedObj = lowestDistObj;
            }
        }
        return selectedObj != null? selectedObj : null;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
