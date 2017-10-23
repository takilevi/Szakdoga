using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusBoxSubMenu : MonoBehaviour {

    public GameObject firstChapter;
    public GameObject secondChapter;
    public GameObject thirdChapter;
    public GameObject back;

    private GameObject focusedGameObject;
    private Text firstChapterText;
    private Text secondChapterText;
    private Text thirdChapterText;
    private Text backText;

    private const string FIRST = "FIRST";
    private const string SECOND = "SECOND";
    private const string THIRD = "THIRD";
    private const string BACK = "BACK";
    MenuChoose menuScript;

    // Use this for initialization
    void Start()
    {
        focusedGameObject = null;
        firstChapterText = firstChapter.GetComponent<UnityEngine.UI.Text>();
        secondChapterText = secondChapter.GetComponent<UnityEngine.UI.Text>();
        thirdChapterText = thirdChapter.GetComponent<UnityEngine.UI.Text>();
        backText = back.GetComponent<UnityEngine.UI.Text>();

        firstChapter.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        secondChapter.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        thirdChapter.GetComponent<UnityEngine.UI.Shadow>().enabled = false;
        back.GetComponent<UnityEngine.UI.Shadow>().enabled = false;

        GameObject levelManager = GameObject.Find("LevelManager");
        menuScript = (MenuChoose)levelManager.GetComponent(typeof(MenuChoose));
    }

    // Update is called once per frame
    void Update()
    {

        if (focusedGameObject != null)
        {
            focusedGameObject.GetComponent<UnityEngine.UI.Shadow>().enabled = true;
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DisableFocus();
            }

        }
        else
        {
            Debug.Log("bejöttem ide ahova akkor szabad ha null");
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
        firstChapterText.text = FIRST;
        secondChapterText.text = SECOND;
        thirdChapterText.text = THIRD;
        backText.text = BACK;
    }
    public void FirstCapitalTypeing()
    {

        foreach (char c in Input.inputString)
        {
            Debug.Log("char cucc: " + c);
            if ((c == '\n') || (c == '\r') || (c == '\b')) // enter/return
            {
                //do nothing
            }
            else
            {
                if (char.ToUpper(c).Equals(char.ToUpper(firstChapterText.text.ToCharArray()[0])))
                {
                    focusedGameObject = firstChapter;
                    menuScript.TakeThisObject(firstChapter);
                }
                if (char.ToUpper(c).Equals(char.ToUpper(secondChapterText.text.ToCharArray()[0])))
                {
                    focusedGameObject = secondChapter;
                    menuScript.TakeThisObject(secondChapter);
                }
                if (char.ToUpper(c).Equals(char.ToUpper(thirdChapterText.text.ToCharArray()[0])))
                {
                    focusedGameObject = thirdChapter;
                    menuScript.TakeThisObject(thirdChapter);
                }
                if (char.ToUpper(c).Equals(char.ToUpper(backText.text.ToCharArray()[0])))
                {
                    focusedGameObject = back;
                    menuScript.TakeThisObject(back);
                }
                return;
            }
        }
    }
}
