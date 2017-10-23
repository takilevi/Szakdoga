using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuChoose : MonoBehaviour {

    public GameObject choosenOne = null;

    private const string RICHTEXT_BEGIN = "<color=#00FF00>";
    private const string RICHTEXT_END = "</color>";
    private List<char> menuCharSet = new List<char>();
    private int correctCharNumb = 0;
    private string correctString = "";
    private string originalString;
    public GameObject mainMenu;
    public GameObject subMenu;

    FocusBoxSubMenu submenuScript;
    FocusBoxHandler mainMenuScript;

    // Use this for initialization
    void Start () {
        submenuScript = (FocusBoxSubMenu)subMenu.GetComponent(typeof(FocusBoxSubMenu));
        mainMenuScript = (FocusBoxHandler)mainMenu.GetComponent(typeof(FocusBoxHandler));
    }
	
	// Update is called once per frame
	void Update () {
        
        foreach (char c in Input.inputString)
        {
            if (choosenOne == null)
            { //that was handled before
            }
            else
            {
                if ((c == '\n') || (c == '\r') || (c == '\b')) // enter/return/back
                {
                    //do nothing
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    Debug.Log(menuCharSet[correctCharNumb]);
                    
                    if (char.ToUpper(c) == menuCharSet[correctCharNumb])
                    {
                        Debug.Log("ascii code: "+(int)c);
                        correctCharNumb++;
                        sb.Append(RICHTEXT_BEGIN);
                        sb.Append(originalString.Substring(0, correctCharNumb));
                        sb.Append(RICHTEXT_END);
                        sb.Append(originalString.Substring(correctCharNumb));
                        choosenOne.GetComponent<UnityEngine.UI.Text>().text = sb.ToString();
                    }
                }
            }
            
        }

        if (correctCharNumb == menuCharSet.Count)
        {
            NavigateToNextScene();
        }
    }

    private void NavigateToNextScene()
    {
        switch (originalString)
        {
            case "TUTORIAL": SceneManager.LoadScene("TUTORIAL");break;
            case "SINGLE PLAYER": subMenu.SetActive(true); mainMenu.SetActive(false); break;
            case "MULTI PLAYER": SceneManager.LoadScene("TUTORIAL"); break;
            case "BACK": mainMenuScript.DisableOnRestart(); mainMenu.SetActive(true); subMenu.SetActive(false);  break;
            case "FIRST": SceneManager.LoadScene("SINGLE PLAYER"); break;
            case "SECOND": SceneManager.LoadScene("SINGLE PLAYER 2"); break;
            case "THIRD": SceneManager.LoadScene("SINGLE PLAYER 3"); break;
        }
        
    }

    public void TakeThisObject(GameObject menuChoosen)
    {
        menuCharSet = new List<char>();
        choosenOne = menuChoosen;
        originalString = menuChoosen.GetComponent<UnityEngine.UI.Text>().text;
        menuCharSet.AddRange(originalString);
        correctCharNumb = 1;
        FirstCapitalCorrect();

    }
    void FirstCapitalCorrect()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RICHTEXT_BEGIN);
        sb.Append(menuCharSet[0]);
        sb.Append(RICHTEXT_END);
        sb.Append(originalString.Substring(correctCharNumb));
        choosenOne.GetComponent<UnityEngine.UI.Text>().text = sb.ToString();

    }
}
