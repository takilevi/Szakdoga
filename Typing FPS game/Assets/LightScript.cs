using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightScript : MonoBehaviour {

    public GameObject playerObject;
    public GameObject thisObject;
    public GameObject thisCanvas;
    public string textToType;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnemyHere()
    {
        thisCanvas.SetActive(true);
        Text lightText = (Text)thisCanvas.GetComponentInChildren<Text>();
        lightText.text = textToType;
    }
    
    public void KillToDeath()
    {
        thisCanvas.SetActive(false);
        thisObject.SetActive(false);

    }
}
