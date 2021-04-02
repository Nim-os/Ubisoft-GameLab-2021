using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class CustomPropertyGenerator : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();

    private void SetCustomNumber()
    {
        System.Random rnd = new System.Random();
        int result = rnd.Next(0,999);

        _text.text = result.ToString();
        _customProperties["RandomNumber"] = result;
        PhotonNetwork.LocalPlayer.CustomProperties = _customProperties;
    }

    public void OnClickStart()
    {
        SetCustomNumber();
    }
}
