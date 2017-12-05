using DigitalRuby.PyroParticles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

  public bool fireIt;

  private string toType;
  private string currentlyCorrect;
  int correctCharNumb = 0;
  private List<char> currectCharSet = new List<char>();


  // Use this for initialization
  void Start()
  {
    fireBallScript = (DemoScript)gameObject.GetComponent(typeof(DemoScript));
    myManager = (MyManager)GameObject.Find("Network Manager").GetComponent(typeof(MyManager));
    commonText = GetComponentInChildren<Text>();
    typerText = (Text)GameObject.Find("TyperText").GetComponent(typeof(Text));
    labelTag = commonText.text;
    toType = "";
    currentlyCorrect = "";
    fireIt = false;
  }

  // Update is called once per frame
  void Update()
  {
    commonText.text = labelTag;

    if (!isLocalPlayer)
    {
      return;
    }

    if (myManager.TypeStart() != null && toType.Length == 0)
    {
      foreach (var item in myManager.TypeStart())
      {
        if (item.Key == true)
        {
          toType = item.Value;
          currectCharSet = new List<char>();
          correctCharNumb = 0;
          currectCharSet.AddRange(toType);
        }
      }

      Debug.Log("megkaptam azt a szart: " + toType + " és a charset: " + currectCharSet);
    }

    //Debug.Log("probalom elérni a szöveget: "+myManager.GetGlobalController().textForEveryone);
    if(myManager.TypeStart() == null && toType.Length == 0 
      && !myManager.GetGlobalController().textForEveryone.Contains("PRESS ENTER")
      && !myManager.GetGlobalController().textForEveryone.Contains("COUNT DOWN")
      && myManager.GetGlobalController().textForEveryone != null)
    {
      Debug.Log("bejutottunk az új elágazásba");

      toType = myManager.GetGlobalController().textForEveryone;
      correctCharNumb = 0;
      currectCharSet = new List<char>();
      currectCharSet.AddRange(toType);
    }

    if (toType != "" && correctCharNumb != currectCharSet.Count)
    { Debug.Log("check");
      CharCheck();
      if (myManager.GetGlobalController().textForEveryone.Contains("PRESS ENTER"))
      {
        DoTheCleanup();
      } }

    if (fireIt)
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

      //ez a visszaállítás rész!!!
      toType = "";
      typerText.text = "";
      currentlyCorrect = "";
      currectCharSet = new List<char>();
      correctCharNumb = 0;
      fireIt = false;
    }

    if (Input.GetKeyDown(KeyCode.Return))
    {
      Debug.Log("Enter pressed name: " + gameObject.name);

      if (gameObject.name.EndsWith("(Clone)"))
      {
        CmdClientReadySign();
        
      }
      else
      {
        labelTag += "\nready";
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
    labelTag += "\nready";
    commonText.text += labelTag;

    myManager.EnterPressedByClient(gameObject.name);
  }

  public void ThisIsYourGlobal(GlobalController glob)
  {
    Debug.Log("kaptam egy globalt: " + glob.name);
    globalReference = glob;
  }

  public void CharCheck()
  {
    foreach (char c in Input.inputString)
    {
      if ((c == '\n') || (c == '\r') || (c == '\b')) // enter/return/back
      {
        //do nothing
      }
      else
      {
        Debug.Log("checkouljuk az írást, de mi is a szó?: "+toType+" charset: "+currectCharSet + " eddig ennyi jó: "+currentlyCorrect);
        if (Char.ToUpper(c) == currectCharSet[correctCharNumb])
        {
          Debug.Log("korrekt");
          correctCharNumb++;
          currentlyCorrect += Char.ToUpper(c);

          typerText.text = currentlyCorrect;
        }
      }
    }

    if (correctCharNumb == currectCharSet.Count)
    {
      fireIt = true;
    }
  }


  public void DoTheCleanup()
  {
    toType = "";
    typerText.text = "";
    currentlyCorrect = "";
    currectCharSet = new List<char>();
    correctCharNumb = 0;
  }
}
