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
      if (gameObject.name.Equals("Player1"))
      {
        CmdScoreOnClient(1);
      }
      else
      {
        CmdScoreOnClient(2);
      }
    }

    if (Input.GetKeyDown(KeyCode.Return))
    {
      labelTag += "\nready";
      Debug.Log("Enter pressed name: " + gameObject.name);

      if (gameObject.name.EndsWith("(Clone)"))
      {
        CmdClientReadySign();
        
      }
      else
      {
        commonText.text += "\nready";
        myManager.EnterPressedByClient(gameObject.name);
      }
    }

  }

  [Command]
  public void CmdScoreOnClient(int player)
  {
    myManager.ScoreForPlayer(player);
  }

  [Command]
  public void CmdClientReadySign()
  {
    labelTag = "Player TWO\nready";
    commonText.text += labelTag;

    myManager.EnterPressedByClient(gameObject.name);
  }

  public void ThisIsYourGlobal(GlobalController glob)
  {
    Debug.Log("kaptam egy globalt: " + glob.name);
    globalReference = glob;
  }
}
