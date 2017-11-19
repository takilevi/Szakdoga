using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusBoxHandler : MonoBehaviour {

    public GameObject singlePlayer;
    public GameObject multiPlayer;
    public GameObject tutorial;
    public GameObject loadFile;

    private GameObject focusedGameObject;
    private Text singlePlayerText;
    private Text multiPlayerText;
    private Text tutorialText;
    private Text loadFileText;

    private const string SINGLE_PLAYER = "SINGLE PLAYER";
    private const string MULTI_PLAYER = "MULTI PLAYER";
    private const string TUTORIAL = "TUTORIAL";
    private const string LOAD_FILE = "LOAD FILE";
    MenuChoose menuScript;


    // Use this for initialization
    void Start () {
        Init();
    }
    public void Init()
    {
        focusedGameObject = null;
        singlePlayerText = singlePlayer.GetComponent<UnityEngine.UI.Text>();
        multiPlayerText = multiPlayer.GetComponent<UnityEngine.UI.Text>();
        tutorialText = tutorial.GetComponent<UnityEngine.UI.Text>();
        loadFileText = loadFile.GetComponent<UnityEngine.UI.Text>();

        singlePlayer.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        multiPlayer.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        tutorial.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        loadFile.GetComponent<UnityEngine.UI.Shadow>().enabled = false;

        GameObject levelManager = GameObject.Find("LevelManager");
        menuScript = (MenuChoose)levelManager.GetComponent(typeof(MenuChoose));
    }

    // Update is called once per frame
    void Update () {

        

        if (focusedGameObject !=null)
        {
            focusedGameObject.GetComponent<UnityEngine.UI.Shadow>().enabled = true;
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DisableFocus();
            }
            
        }
        else
        {
            FirstCapitalTypeing();
        }
    }

    public void SetFocusedGameObject(GameObject focused)
    {
        focusedGameObject = focused;

    }
    public void DisableFocus()
    {
        menuScript.choosenOne = null;
        DisableOnRestart();
    }
    public void DisableOnRestart()
    {
        focusedGameObject.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        focusedGameObject = null;
        singlePlayerText.text = SINGLE_PLAYER;
        multiPlayerText.text = MULTI_PLAYER;
        tutorialText.text = TUTORIAL;
        loadFileText.text = LOAD_FILE;
    }
    public void FirstCapitalTypeing()
    {
        
        foreach (char c in Input.inputString)
        {
            if ((c == '\n') || (c == '\r') || (c == '\b')) // enter/return
            {
                //do nothing
            }
            else
            {
                if(char.ToUpper(c).Equals(char.ToUpper(singlePlayerText.text.ToCharArray()[0])))
                {
                    focusedGameObject = singlePlayer;
                    menuScript.TakeThisObject(singlePlayer);
                }
                if (char.ToUpper(c).Equals(char.ToUpper(multiPlayerText.text.ToCharArray()[0])))
                {
                    focusedGameObject = multiPlayer;
                    menuScript.TakeThisObject(multiPlayer);
                }
                if (char.ToUpper(c).Equals(char.ToUpper(tutorialText.text.ToCharArray()[0])))
                {
                    focusedGameObject = tutorial;
                    menuScript.TakeThisObject(tutorial);
                }
                if (char.ToUpper(c).Equals(char.ToUpper(loadFileText.text.ToCharArray()[0])))
                {
                    focusedGameObject = loadFile;
                    menuScript.TakeThisObject(loadFile);
                }
                return;
            }
        }
    }
}
