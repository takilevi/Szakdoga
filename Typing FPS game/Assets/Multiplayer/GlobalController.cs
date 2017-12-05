using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GlobalController : NetworkBehaviour
{

  public bool startCounter;

  [SyncVar(hook = "OnChecking")]
  public int clientsReady;

  [SyncVar(hook = "OnChangeScoreOne")]
  public int playerOneScore;
  [SyncVar(hook = "OnChangeScoreTwo")]
  public int playerTwoScore;

  [SyncVar(hook = "DisplayGlobalText")]
  public string textForEveryone;

  public bool playerOneReady;
  public bool playerTwoReady;

  public Text scoreText;
  public Text globalText;

  private const string playerOne = "Player ONE: ";
  private const string playerTwo = "Player TWO: ";

  // Use this for initialization
  void Start()
  {
    startCounter = false;
    playerOneReady = playerTwoReady = false;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void ClientCheckedInTrigger()
  {
    clientsReady++;
  }
  public void OnChecking(int clients)
  {
    if(clients==2)
    {
      if (globalText == null)
      {
        globalText = (Text)GameObject.Find("GlobalText").GetComponent(typeof(Text));
      }
      Debug.Log("enterre várunk itt, a globaltext: " + globalText);
      textForEveryone = "PRESS ENTER IF YOU ARE READY.";
    }
  }
  public void DisplayGlobalText(string toDisplay)
  {
    globalText.text = toDisplay;
  }
  public void ScoreForPlayer(int playerNumber)
  {

    Debug.Log("ScoreForPlayer kapott egy inputot: " + playerNumber);
    switch (playerNumber)
    {
      case 1: playerOneScore++; break;
      case 2: playerTwoScore++; break;
    }

  }

  void OnChangeScoreOne(int plOneScore)
  {
    playerOneScore = plOneScore;
    scoreText.text = playerOne + plOneScore + "\n" + playerTwo + playerTwoScore;
  }
  void OnChangeScoreTwo(int plTwoScore)
  {
    playerTwoScore = plTwoScore;
    scoreText.text = playerOne + playerOneScore + "\n" + playerTwo + plTwoScore;
  }

  public void EnterPressedByClient(string playerName)
  {
    switch (playerName)
    {
      case "Player1": playerOneReady = true; break;
      case "Player2": playerTwoReady = true; break;
    }

  }



}
