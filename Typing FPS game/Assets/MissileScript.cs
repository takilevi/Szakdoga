using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileScript : MonoBehaviour {

    public GameObject playerObject;
    public GameObject thisObject;
    public GameObject thisCanvas;
    public GameObject gunEnd;

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
    }

    public void KillToDeath()
    {
        thisCanvas.SetActive(false);
        LaunchRocket launchScript = (LaunchRocket)gunEnd.GetComponent(typeof(LaunchRocket));
        launchScript.BeginEffect();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject.Destroy(thisObject);
    }
    

}
