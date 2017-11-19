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
    private bool stopNavigate = false;
    public WordHelper wordScript;
    FocusBoxSubMenu submenuScript;
    FocusBoxHandler mainMenuScript;

    private void Reset()
    {
        stopNavigate = false;
    }
    // Use this for initialization
    void Start () {
        submenuScript = (FocusBoxSubMenu)subMenu.GetComponent(typeof(FocusBoxSubMenu));
        mainMenuScript = (FocusBoxHandler)mainMenu.GetComponent(typeof(FocusBoxHandler));
        wordScript = this.GetComponent(typeof(WordHelper)) as WordHelper;
    }
	
	// Update is called once per frame
	void Update () {
        
        foreach (char c in Input.inputString)
        {
            if (choosenOne == null)
            { 
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
                    //Debug.Log(menuCharSet[correctCharNumb]);
                    
                    if (correctCharNumb < menuCharSet.Count && char.ToUpper(c) == menuCharSet[correctCharNumb])
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

        if (correctCharNumb == menuCharSet.Count && correctCharNumb!=0)
        {
            NavigateToNextScene();
        }
    }

    private void NavigateToNextScene()
    {
        Debug.Log(stopNavigate);
        if (!stopNavigate)
        {
            switch (originalString)
            {
                case "TUTORIAL": SceneManager.LoadScene("TUTORIAL"); break;
                case "SINGLE PLAYER": subMenu.SetActive(true); mainMenu.SetActive(false); break;
                case "MULTI PLAYER": SceneManager.LoadScene("TUTORIAL"); break;
                case "BACK": mainMenuScript.DisableOnRestart(); mainMenu.SetActive(true); subMenu.SetActive(false); break;
                case "FIRST": SceneManager.LoadScene("SINGLE PLAYER"); break;
                case "SECOND": SceneManager.LoadScene("SINGLE PLAYER 2"); break;
                case "THIRD": SceneManager.LoadScene("SINGLE PLAYER 3"); break;
                case "LOAD FILE": OpenFilePicker(); stopNavigate = true; break;
            }
            
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

    void OpenFilePicker()
    {
        //Application.OpenURL((Application.dataPath) + "/FilePicker");
        var newTyperText = Resources.Load("typerResource") as TextAsset;
        //Debug.Log(newTyperText.text);
        if(newTyperText == null)
        {
            GameObject child = choosenOne.transform.GetChild(0).gameObject;
            child.GetComponent<UnityEngine.UI.Text>().text = "THIS FILE IS NOT ELIGIBLE TO LOAD";
            StartCoroutine(LoadNextSceneAfterWait(child));
        }
        if (newTyperText != null && !CheckFormatting(newTyperText.text))
        {
            GameObject child = choosenOne.transform.GetChild(0).gameObject;
            child.GetComponent<UnityEngine.UI.Text>().text = "FORMAT DOES NOT MATCH THE REQUIREMENTS";
            StartCoroutine(LoadNextSceneAfterWait(child));
        }
        if (newTyperText != null && CheckFormatting(newTyperText.text))
        {
            GameObject child = choosenOne.transform.GetChild(0).gameObject;
            child.GetComponent<UnityEngine.UI.Text>().text = "<color=green>SUCCESFULL LOAD</color>";
            StartCoroutine(LoadNextSceneAfterWait(child));
        }
    }
    bool CheckFormatting(String rawText)
    {
        string[] splitString = rawText.Split('\n');
        if(splitString.Length != 4)
        { return false; }

        string[] firstArray = splitString[0].Split(',');
        string[] secondArray = splitString[1].Split(',');
        string[] thirdArray = splitString[2].Split(',');
        string[] fourthArray = splitString[3].Split(',');

        foreach (var item in firstArray)
        {
            if(item.Length >= 15)
            { return false; }
        }
        foreach (var item in secondArray)
        {
            if (item.Length >= 15)
            { return false; }
        }
        foreach (var item in thirdArray)
        {
            if (item.Length >= 15)
            { return false; }
        }
        foreach (var item in fourthArray)
        {
            if (item.Length > 20)
            { return false; }
        }

        wordScript.LoadData(firstArray, secondArray, thirdArray, fourthArray);

        return true;
    }

    IEnumerator LoadNextSceneAfterWait(GameObject child)
    {
        yield return new WaitForSeconds(3.5f);
        child.GetComponent<UnityEngine.UI.Text>().text = "";
        stopNavigate = false;
        SceneManager.LoadScene("start_scene");
    }
}
