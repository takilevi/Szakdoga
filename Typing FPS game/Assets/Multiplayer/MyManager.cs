using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyManager : NetworkManager
{
  public List<GameObject> spawnPlayers;
  public static int globalCounter;
  public static GlobalController globalController;
  public bool firstCheck;

  void Start()
  {
    globalCounter = 0;
    firstCheck = false;
    
  }
  private void Update()
  {
    if(globalController==null && ClientScene.objects.Count != 0)
    {
      globalController = (GlobalController)GameObject.Find("GlobalControllerGameObject").GetComponent<GlobalController>();
    }

    if(ClientScene.objects.Count == 4 && !firstCheck)
    {
      var clientDictionary = ClientScene.objects;

      GameObject playerOne = null;
      GameObject playerTwo = null;
      foreach (var item in clientDictionary)
      {
        //Debug.Log("Key: " + item.Key + " Value: " + item.Value);
        if(item.Value.name.Equals("Player1") || item.Value.name.Equals("player1(Clone)"))
        {
          playerOne = item.Value.gameObject;
        }
        if (item.Value.name.Equals("Player2") || item.Value.name.Equals("player2(Clone)"))
        {
          playerTwo = item.Value.gameObject;
        }
      }

      var playerScriptOne = (TriggerScript)playerOne.GetComponent<TriggerScript>();
      playerScriptOne.ThisIsYourGlobal(globalController);

      var playerScriptTwo = (TriggerScript)playerTwo.GetComponent<TriggerScript>();
      playerScriptTwo.ThisIsYourGlobal(globalController);

      globalController.ClientCheckedInTrigger();
      globalController.ClientCheckedInTrigger();

      firstCheck = true;
    }
  }
  public void SetGlobalController(GlobalController glob)
  {
    globalController = glob;
  }

  public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
  {

    globalCounter++;
    Debug.Log("added player by OnServerAddPlayer with id:" + playerControllerId + " and global count: " + globalCounter);
    if (globalCounter == 1)
    {
      Quaternion rotate = Quaternion.identity;
      rotate = spawnPlayers[0].transform.rotation;
      GameObject player = (GameObject)Instantiate(spawnPlayers[0], spawnPlayers[0].transform.position, rotate);
      player.name = "Player1";

      NetworkServer.AddPlayerForConnection(conn, player, (short)globalCounter);
    }
    if (globalCounter == 2)
    {
      Quaternion rotateClient = Quaternion.identity;
      rotateClient = spawnPlayers[1].transform.rotation;
      GameObject player = (GameObject)Instantiate(spawnPlayers[1], spawnPlayers[1].transform.position, rotateClient);
      player.name = "Player2";

      NetworkServer.AddPlayerForConnection(conn, player, (short)globalCounter);
    }
    else
    { return; }

  }

  public override void OnClientConnect(NetworkConnection conn)
  {
    Debug.Log("client connected with globalcounter: "+globalCounter);
    short numberOfConnections = (short)NetworkServer.connections.Count;

    ClientScene.AddPlayer(conn, numberOfConnections);
  }

  public void EnterPressedByClient(string playerName)
  {
    switch (playerName)
    {
      case "Player1": globalController.EnterPressedByClient("Player1"); break;
      case "player1(Clone)": globalController.EnterPressedByClient("Player1"); break;
      case "Player2": globalController.EnterPressedByClient("Player2"); break;
      case "player2(Clone)": globalController.EnterPressedByClient("Player2"); break;
    }

  }

  public void ScoreForPlayer(int playerNumber)
  {
    Debug.Log("ScoreForPlayer kapott egy inputot: " + playerNumber);
    switch (playerNumber)
    {
      case 1: globalController.ScoreForPlayer(playerNumber); break;
      case 2: globalController.ScoreForPlayer(playerNumber); break;
    }

  }

  public Dictionary<bool, string> TypeStart()
  {
    return globalController.TypeStart();
  }

  public GlobalController GetGlobalController()
  {
    return globalController;
  }
}
