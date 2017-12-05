using DigitalRuby.PyroParticles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TriggerScript : NetworkBehaviour
{
  [SyncVar]
  public string labelTag;

  DemoScript fireBallScript;
  MyManager myManager;
  public GlobalController globalReference;
  public GameObject globalControllerGameObject;

  public int nowPressEnter = 0;
  private Text commonText;
  public Text typerText;
  // Use this for initialization
  void Start()
  {
    fireBallScript = (DemoScript)gameObject.GetComponent(typeof(DemoScript));
    myManager = (MyManager)GameObject.Find("Network Manager").GetComponent(typeof(MyManager));
    commonText = GetComponentInChildren<Text>();
    labelTag = commonText.text;
  }

  // Update is called once per frame
  void Update()
  {
    commonText.text = labelTag;

    if (!isLocalPlayer)
    {
      return;
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      fireBallScript.StartCurrent();
      var playerArray = gameObject.name.ToCharArray();
      char player = playerArray[playerArray.Length - 1];
      int playerNumber = (int)Char.GetNumericValue(player);
      if (playerNumber == (-1))
      {
        playerNumber = 2;
      }
      Debug.Log("player: " + playerNumber);
      myManager.ScoreForPlayer(playerNumber);
    }

    if (Input.GetKeyDown(KeyCode.Return))
    {
      labelTag += "\nready";
      Debug.Log("Enter pressed name: " + gameObject.name);

      if (gameObject.name.EndsWith("(Clone)"))
      {
        //commonText.text += "\nready";
        CmdClientReadySign();
        Debug.Log("THIS IS A CLONE: " + gameObject.name);
        myManager.EnterPressedByClient(gameObject.name);
      }
      else
      {
        commonText.text += "\nready";
        myManager.EnterPressedByClient(gameObject.name);
      }
    }

  }

  [Command]
  public void CmdClientReadySign()
  {
    labelTag = "Player TWO\nready";
    commonText.text += labelTag;
  }

  public void ThisIsYourGlobal(GlobalController glob)
  {
    Debug.Log("kaptam egy globalt: " + glob.name);
    globalReference = glob;
  }
}
