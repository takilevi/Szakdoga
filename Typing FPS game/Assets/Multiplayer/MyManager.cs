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

  void Start()
  {
    globalCounter = 0;
    
  }
  private void Update()
  {
    
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
      var playerScript = (TriggerScript)player.GetComponent<TriggerScript>();
      playerScript.ThisIsYourGlobal(globalController);

      globalController.ClientCheckedInTrigger();

      NetworkServer.AddPlayerForConnection(conn, player, (short)globalCounter);
    }
    if (globalCounter == 2)
    {
      

      Quaternion rotateClient = Quaternion.identity;
      rotateClient = spawnPlayers[1].transform.rotation;
      GameObject player = (GameObject)Instantiate(spawnPlayers[1], spawnPlayers[1].transform.position, rotateClient);
      player.name = "Player2";
      var playerScript = (TriggerScript)player.GetComponent<TriggerScript>();
      playerScript.ThisIsYourGlobal(globalController);

      globalController.ClientCheckedInTrigger();

      NetworkServer.AddPlayerForConnection(conn, player, (short)globalCounter);
    }
    else
    { return; }

  }

  public override void OnClientConnect(NetworkConnection conn)
  {
    Debug.Log("client connected with globalcounter: "+globalCounter);
    short numberOfConnections = (short)NetworkServer.connections.Count;
    if (numberOfConnections != 0)
    {
      NetworkInstanceId netId = new NetworkInstanceId(2);
      var serverDictionary = NetworkServer.objects;
      foreach (var item in serverDictionary)
      {
        if (item.Value.gameObject.name.Equals("GlobalControllerGameObject"))
        {
          Debug.Log("megtaláltam a globalcontrollert");
          netId = item.Key;
        }
      }

      GameObject tempGlobalController = NetworkServer.FindLocalObject(netId);
      Debug.Log("onclientconnect function, findobjectoftype " + tempGlobalController.name);

      globalController = (GlobalController)tempGlobalController.GetComponent<GlobalController>();
    }

    ClientScene.AddPlayer(conn, numberOfConnections);
  }

  public void EnterPressedByClient(string playerName)
  {
    switch (playerName)
    {
      case "Player1": globalController.EnterPressedByClient("Player1"); break;
      case "Player2": globalController.EnterPressedByClient("Player1"); break;
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
}
