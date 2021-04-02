using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LevelConnect : MonoBehaviourPunCallbacks
{
     private void Start()
    {
        Debug.Log(this.name + " is connecting to the server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(this.name + " successfully connected to server.");
        // Uses SERVER, not LOCAL, to print nickname
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " successfully connected to server.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        Debug.Log(this.name + " has been disconnected from server for reason " + cause.ToString());
        // Uses SERVER, not LOCAL, to print nickname
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has been disconnected from server for reason " + cause.ToString());

    }
}
