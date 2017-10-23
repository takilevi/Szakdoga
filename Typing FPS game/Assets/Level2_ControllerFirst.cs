using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2_ControllerFirst : MonoBehaviour {

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemies;
    public ArrayList mainStopPoints = new ArrayList();
    public AudioClip firstStopAudio;
    public GameObject currentGameObject;
    public GameObject nextGameObject;
    private Controller controllerScript;
    private int globalSelector = 0;

    //ZombieScript here

    TyperHelper typerScript;

    // Use this for initialization
    void Start() {
        controllerScript = this.GetComponent<Controller>();
        pathPercent = controllerScript.PathToStop;
        mainStopPoints.Add(68);
        mainStopPoints.Add(98);
        mainStopPoints.Add(99);

        // zombiescript here = (TerroristScript)enemiesSecondStop[0].GetComponent(typeof(TerroristScript));

        typerScript = (TyperHelper)GameObject.Find("MainCamera").GetComponent(typeof(TyperHelper));
    }

    // Update is called once per frame
    void Update() {

        pathPercent = controllerScript.PathToStop;
        controllerScript.IsMoveEnable = goToNextStopPoint;

        Debug.Log(pathPercent);

        if (goToNextStopPoint == false)
        {
            if (typerScript.GoForward)
            {
                goToNextStopPoint = true;
                typerScript.GoForward = false;

            }
        }

        if (mainStopPoints[globalSelector].Equals(pathPercent))
        {
            switch (globalSelector)
            {
                case 0: DoFirstStop(); break;
                case 1: DoSecondStop(); break;
                case 2: DoThirdStop(); break;
                default: break;
            }
        }
    }

    void DoFirstStop()
    {
        Debug.Log("called first");
        AudioSource.PlayClipAtPoint(firstStopAudio, controllerScript.character.position, 1f);
        goToNextStopPoint = false;
        StartCoroutine(DoFirstStopWait());
        globalSelector++;
    }
    IEnumerator DoFirstStopWait()
    {
        yield return new WaitForSeconds(6.3f);
        controllerScript.speed = 0.10f;
        goToNextStopPoint = true;
    }
    void DoSecondStop()
    {
        Debug.Log("called first");
        goToNextStopPoint = false;

        //zombies start to chase

        typerScript.LoadNewEnemies(enemies);
        globalSelector++;
    }
    void DoThirdStop()
    {
        Debug.Log("called third");
        nextGameObject.SetActive(true);
        currentGameObject.SetActive(false);
    }
}
