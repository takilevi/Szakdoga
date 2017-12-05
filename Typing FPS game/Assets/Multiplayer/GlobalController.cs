using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GlobalController : NetworkBehaviour
{
  string[] sentenceArray = {"Close But No Cigar", "Hear, Hear", "Right Out of the Gate", "Quality Time", "Burst Your Bubble", "Two Down, One to Go",
    "Dropping Like Flies", "Top Drawer", "An Arm and a Leg", "Drawing a Blank", "Man of Few Words", "Knock Your Socks Off", "Roll With the Punches",
    "Hands Down", "Gold of Fool", "Hit Below The Belt", "There is No I in Team", "Back to Square One", "A Piece of Cake", "Right Off the Bat",
    "Cup Of Joe", "In a Pickle", "Elephant in the Room", "Mouth-watering", "Quality Time"};


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

  [SyncVar]
  private float timeLeft;

  public bool displayWord;
  public bool wordDisplayed;
  public string wordToType;

  // Use this for initialization
  void Start()
  {
    playerOneReady = playerTwoReady = false;
    timeLeft = 5f;
    displayWord = false;
    wordDisplayed = false;
    wordToType = null;
  }

  // Update is called once per frame
  void Update()
  {
    globalText.text = textForEveryone;

    if(playerOneReady && playerTwoReady && !displayWord)
    {
      
      timeLeft -= Time.deltaTime;

      textForEveryone = "COUNT DOWN:\n"+timeLeft;

      if (timeLeft <= 0)
      {
        displayWord = true;
      }
    }
    if(displayWord && !wordDisplayed)
    {
      textForEveryone = sentenceArray[Random.Range(0,sentenceArray.Length-1)].ToUpper();
      wordDisplayed = true;
      wordToType = textForEveryone;
    }
  }

  public void ClientCheckedInTrigger()
  {
    clientsReady++;
  }
  public void OnChecking(int clients)
  {
    if(clients==2)
    {
      textForEveryone = "PRESS ENTER IF YOU ARE READY.";
    }
  }
  public void DisplayGlobalText(string toDisplay)
  {
    textForEveryone = toDisplay;
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
    Debug.Log("player1: " + playerOneReady + " player2: " + playerTwoReady);
  }

  public Dictionary<bool,string> TypeStart()
  {
    if (!wordDisplayed) { return null; }
    Dictionary<bool, string> dictionary = new Dictionary<bool, string>();
    dictionary.Add(wordDisplayed, wordToType);
    return dictionary;
  }


}
