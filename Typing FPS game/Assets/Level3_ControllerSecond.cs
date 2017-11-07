using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3_ControllerSecond : MonoBehaviour {

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemiesFirst;
    public GameObject[] enemiesSecond;
    public GameObject[] enemiesThird;
    public ArrayList mainStopPoints = new ArrayList();
    public GameObject currentGameObject;
    public AudioClip lastStopAudio;
    private Controller controllerScript;
    private int globalSelector = 0;

    List<ArmyScript> armyScript1 = new List<ArmyScript>();
    List<ArmyScript> armyScript2 = new List<ArmyScript>();
    List<MissileScript> missileScript = new List<MissileScript>();


    TyperHelper typerScript;

    // Use this for initialization
    void Start()
    {
        controllerScript = this.GetComponent<Controller>();
        pathPercent = controllerScript.PathToStop;


        foreach (GameObject item in enemiesFirst)
        {
            armyScript1.Add((ArmyScript)item.GetComponent(typeof(ArmyScript)));
        }
        foreach (GameObject item in enemiesSecond)
        {
            armyScript2.Add((ArmyScript)item.GetComponent(typeof(ArmyScript)));
        }
        foreach (GameObject item in enemiesThird)
        {
            missileScript.Add((MissileScript)item.GetComponent(typeof(MissileScript)));
        }


        typerScript = (TyperHelper)GameObject.Find("MainCamera").GetComponent(typeof(TyperHelper));
    }


    // Update is called once per frame
    void Update()
    {

        pathPercent = controllerScript.PathToStop;
        controllerScript.IsMoveEnable = goToNextStopPoint;
        mainStopPoints.Add(45);
        mainStopPoints.Add(66);
        mainStopPoints.Add(99);

        if (goToNextStopPoint == false)
        {
            if (typerScript.GoForward && globalSelector!=3)
            {
                goToNextStopPoint = true;
                typerScript.GoForward = false;

            }
            if (typerScript.GoForward && globalSelector == 3)
            {
                DoFinalScreen();
                typerScript.GoForward = false;

            }

        }

        if (mainStopPoints[globalSelector].Equals(pathPercent) && globalSelector != 3)
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

        goToNextStopPoint = false;

        foreach (var item in armyScript1)
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

        foreach (var item in armyScript2)
        {
            item.EnemyHere();
        }

        typerScript.LoadNewEnemies(enemiesSecond);

        globalSelector++;
    }
    void DoThirdStop()
    {
        Debug.Log("called third");
        goToNextStopPoint = false;

        foreach (var item in missileScript)
        {
            item.EnemyHere();
        }

        typerScript.LoadNewEnemies(enemiesThird);

        globalSelector++;
    }

    void DoFinalScreen()
    {
        AudioSource.PlayClipAtPoint(lastStopAudio, controllerScript.character.position, 2f);

        Text mainCameraText = GameObject.Find("mainCameraText").GetComponent<Text>();
        mainCameraText.text = "CONGRATULATION !!\nYOU WIN THIS TIME";
        StartCoroutine(LoadNextSceneAfterWait());
    }
    IEnumerator LoadNextSceneAfterWait()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("start_scene");
    }

}
