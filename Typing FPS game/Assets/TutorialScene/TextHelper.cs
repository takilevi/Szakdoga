using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class TextHelper : MonoBehaviour
{
  private string ITBullshitText;
  private List<char> bullshitCharSetFirst = new List<char>();
  private List<char> bullshitCharSetSecond = new List<char>();
  private int correctCharNumbFirst = 0;
  private int correctCharNumbSecond = 0;
  private string correctString = "";
  private string BadSubString;
  NPCTag scriptFlesh;
  NPCTag scriptPolygonCharacter;
  private string firstPartString;
  private string secondPartString;
  private bool firstDone = false;
  private bool secondDone = false;
  private bool isDone = false;
  public Text mainCameraText;
  private const float ASPECT_RATIO_MULTIPLIER = 0.519f;

  void Start()
  {
    string url = "http://itbullshit.fps.hu/api.php?f=showTheBullshit";
    WWW www = new WWW(url);

    scriptFlesh = (NPCTag)GameObject.Find("Flesh").GetComponent("NPCTag");
    scriptPolygonCharacter = (NPCTag)GameObject.Find("BombDisposal").GetComponent("NPCTag");

    StartCoroutine(WaitForRequest(www));
  }
  IEnumerator WaitForRequest(WWW www)
  {
    yield return www;

    var N = JSON.Parse(www.text);

    ITBullshitText = N["bullshit"].Value;

    BadSubString = ITBullshitText;
    StringBuilder sb = new StringBuilder();
    StringBuilder sb2 = new StringBuilder();
    int counter = 0;
    var array = ITBullshitText.Split(new[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
    foreach (var item in array)
    {
      if (counter < array.Length / 2 && counter != array.Length - 1)
      {
        sb.Append(item);
        sb.Append(" ");
      }
      if (counter == array.Length / 2 && counter != array.Length - 1)
      {
        sb.Append(item);
      }
      if (counter >= (array.Length / 2) + 1 && counter != array.Length - 1)
      {
        sb2.Append(item);
        sb2.Append(" ");
      }
      if (counter == array.Length - 1)
      {
        sb2.Append(item);
      }
      counter++;

    }
    firstPartString = sb.ToString();
    secondPartString = sb2.ToString();


    Debug.Log("egyik: " + firstPartString);
    Debug.Log("hossz: " + firstPartString.Length);
    Debug.Log("másik: " + secondPartString);
    Debug.Log("hossz: " + secondPartString.Length);

    if (firstPartString.Length > 60 || secondPartString.Length > 60)
    {
      firstPartString = "Az olyan görbéket, melyek egy görbesereg minden elemét";
      secondPartString = "merőlegesen metszik, ortogonális trajektóriának nevezzük";
    }

    bullshitCharSetFirst.AddRange(firstPartString);
    bullshitCharSetSecond.AddRange(secondPartString);
  }
  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Escape))
    {
      if (!firstDone)
      {
        scriptPolygonCharacter.npcName = firstPartString;
        correctCharNumbFirst = 0;
      }
      if (firstDone && !secondDone)
      {
        scriptFlesh.npcName = secondPartString;
        correctCharNumbSecond = 0;
      }
    }

    foreach (char c in Input.inputString)
    {
      if (!firstDone)
      {
        TypeingCorrectionFirst(c);
      }
      if (firstDone && !secondDone)
      {
        TypeingCorrectionSecond(c);
      }

      CheckFinishing();
    }
    if (firstDone && secondDone)
    {
      mainCameraText.text = "<size=50>YOU ARE READY</size>\n\n<color=cyan><size=30>PRESS ESC OR ENTER TO MAIN MENU</size></color>";
      if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Return))
      {
        SceneManager.LoadScene("start_scene");
      }
    }
  }

  private void CheckFinishing()
  {
    if (correctCharNumbFirst == bullshitCharSetFirst.Count)
    {
      firstDone = true;
      scriptPolygonCharacter.enabled = false;
      scriptFlesh.enabled = true;
    }
    if (correctCharNumbSecond == bullshitCharSetSecond.Count)
    {
      secondDone = true;
      scriptFlesh.enabled = false;
    }
  }

  void TypeingCorrectionFirst(char c)
  {
    StringBuilder sb = new StringBuilder();

    if (c == bullshitCharSetFirst[correctCharNumbFirst])
    {
      GunAim gunAim = (GunAim)GameObject.Find("GunAim").GetComponent("GunAim");
      float mouseX = Screen.width * 0.269f + UnityEngine.Random.Range(-30f, 30f);
      float mouseY = Screen.height * ASPECT_RATIO_MULTIPLIER + UnityEngine.Random.Range(-40f, 60f);
      gunAim.mouseX = mouseX;
      gunAim.mouseY = mouseY;

      GunShoot gunShoot = (GunShoot)GameObject.Find("GunShoot").GetComponent("GunShoot");
      gunShoot.FireIt = true;

      if (correctCharNumbFirst > 0)
      {
        sb.Append("<color=#00FF00><b>");
        sb.Append(firstPartString.Substring(0, correctCharNumbFirst));
      }
      else
      {
        sb.Append("<color=#00FF00><b>");
      }
      correctCharNumbFirst++;
      sb.Append(c);
      sb.Append("</b></color>");

      sb.Append("<color=white>");
      BadSubString = firstPartString.Substring(correctCharNumbFirst);
      sb.Append(BadSubString);
      sb.Append("</color>");
      correctString = sb.ToString();

      //Debug.Log(correctString);
      //description.text = correctString;
      scriptPolygonCharacter.npcName = correctString;
      scriptPolygonCharacter.IsCorrectType = true;
    }
    else
    {
      scriptPolygonCharacter.IsCorrectType = false;
    }
  }

  void TypeingCorrectionSecond(char c)
  {
    StringBuilder sb = new StringBuilder();

    if (c == bullshitCharSetSecond[correctCharNumbSecond])
    {
      GunAim gunAim = (GunAim)GameObject.Find("GunAim").GetComponent("GunAim");
      float mouseX = Screen.width * 0.670f + UnityEngine.Random.Range(-30f, 30f);
      float mouseY = Screen.height * ASPECT_RATIO_MULTIPLIER + UnityEngine.Random.Range(-26f, 30f);
      gunAim.mouseX = mouseX;
      gunAim.mouseY = mouseY;

      GunShoot gunShoot = (GunShoot)GameObject.Find("GunShoot").GetComponent("GunShoot");
      gunShoot.FireIt = true;

      if (correctCharNumbSecond > 0)
      {
        sb.Append("<color=#00FF00><b>");
        sb.Append(secondPartString.Substring(0, correctCharNumbSecond));
      }
      else
      {
        sb.Append("<color=#00FF00><b>");
      }
      correctCharNumbSecond++;
      sb.Append(c);
      sb.Append("</b></color>");

      sb.Append("<color=white>");
      BadSubString = secondPartString.Substring(correctCharNumbSecond);
      sb.Append(BadSubString);
      sb.Append("</color>");
      correctString = sb.ToString();

      //Debug.Log(correctString);
      //description.text = correctString;
      scriptFlesh.npcName = correctString;
      scriptFlesh.IsCorrectType = true;
    }

    else
    {
      scriptFlesh.IsCorrectType = false;
    }
  }
  private void OnGUI()
  {
    GUIStyle style = new GUIStyle();
    style.richText = true;

    if (correctString.Equals(""))
    {
      scriptFlesh.npcName = secondPartString;
      scriptPolygonCharacter.npcName = firstPartString;
    }

  }

}
