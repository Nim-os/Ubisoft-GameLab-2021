using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
	public TextMeshProUGUI logPanel;
	public TextMeshProUGUI roomCode;


	#region Buttons

	public void CreateRoom()
	{
		Log($"Creating Room");




		Log($"Created Room {1}");
	}

	public void JoinRoom()
	{
		Log($"Joining Room {roomCode.text}");




		Log($"Joined Room {roomCode.text}");
	}

	public void PlayGame()
	{
		Log("Starting game.");


	}

	#endregion

	private void Log(string text)
	{
		// TODO Make it more console like, messages don't overwrite but instead add themselves
		//logPanel.text = text;
		logPanel.SetText(text);
	}
}
