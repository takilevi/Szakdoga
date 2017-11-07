using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3_ControllerFirst : MonoBehaviour {

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemiesFirst;
    public GameObject[] enemiesSecond;
    public ArrayList mainStopPoints = new ArrayList();
    public GameObject currentGameObject;
    public GameObject nextGameObject;
    public AudioClip firstStopAudio;
    private Controller controllerScript;
    private int globalSelector = 0;

    List<LightScript> lightScript = new List<LightScript>();
    List<ArmyScript> armyScript = new List<ArmyScript>();


    TyperHelper typerScript;

    // Use this for initialization
    void Start()
    {
        controllerScript = this.GetComponent<Controller>();
        pathPercent = controllerScript.PathToStop;
        mainStopPoints.Add(2);
        mainStopPoints.Add(97);
        mainStopPoints.Add(99);

        foreach (GameObject item in enemiesFirst)
        {
            lightScript.Add((LightScript)item.GetComponent(typeof(LightScript)));
        }
        foreach (GameObject item in enemiesSecond)
        {
            armyScript.Add((ArmyScript)item.GetComponent(typeof(ArmyScript)));
        }


        typerScript = (TyperHelper)GameObject.Find("MainCamera").GetComponent(typeof(TyperHelper));
    }


    // Update is called once per frame
    void Update()
    {

        pathPercent = controllerScript.PathToStop;
        controllerScript.IsMoveEnable = goToNextStopPoint;
        //Debug.Log(pathPercent);

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

        foreach (var item in lightScript)
        {
            item.EnemyHere();
        }

        typerScript.LoadNewEnemies(enemiesFirst);

        globalSelector++;
    }
    void DoSecondStop()
    {
        Debug.Log("called second");
        goToNextStopPoint = false;

        foreach (var item in armyScript)
        {
            item.EnemyHere();
        }

        typerScript.LoadNewEnemies(enemiesSecond);

        globalSelector++;
    }
    void DoThirdStop()
    {
        Debug.Log("called third");
        nextGameObject.SetActive(true);
        currentGameObject.SetActive(false);
    }


}
