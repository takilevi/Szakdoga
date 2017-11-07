using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2_ControllerSecond : MonoBehaviour {

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemiesFirst;
    public GameObject[] enemiesSecond;
    public ArrayList mainStopPoints = new ArrayList();
    public AudioClip firstStopAudio;
    public GameObject currentGameObject;
    public GameObject nextGameObject;
    private Controller controllerScript;
    private int globalSelector = 0;

    List<ZombieScript> zombieScriptsFirst = new List<ZombieScript>();
    List<ZombieScript> zombieScriptsSecond = new List<ZombieScript>();


    TyperHelper typerScript;

    // Use this for initialization
    void Start()
    {
        controllerScript = this.GetComponent<Controller>();
        pathPercent = controllerScript.PathToStop;
        mainStopPoints.Add(50);
        mainStopPoints.Add(98);

        foreach (GameObject item in enemiesFirst)
        {
            zombieScriptsFirst.Add((ZombieScript)item.GetComponent(typeof(ZombieScript)));
        }
        foreach (GameObject item in enemiesSecond)
        {
            zombieScriptsSecond.Add((ZombieScript)item.GetComponent(typeof(ZombieScript)));
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
            if (typerScript.GoForward && globalSelector == 2)
            {
                LoadNextScene();
                typerScript.GoForward = false;
            }
            else if (typerScript.GoForward)
            {
                goToNextStopPoint = true;
                typerScript.GoForward = false;

            }
            
        }

        if (globalSelector <= mainStopPoints.Count-1 && mainStopPoints[globalSelector].Equals(pathPercent))
        {
            switch (globalSelector)
            {
                case 0: DoFirstStop(); break;
                case 1: DoSecondStop(); break;
                default: break;
            }
        }
    }

    void DoFirstStop()
    {
        Debug.Log("called first");
        goToNextStopPoint = false;

        foreach (var item in zombieScriptsFirst)
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

        foreach (var item in zombieScriptsSecond)
        {
            item.EnemyHere();
        }

        typerScript.LoadNewEnemies(enemiesSecond);

        globalSelector++;
    }

    void LoadNextScene()
    {
        AudioSource.PlayClipAtPoint(firstStopAudio, controllerScript.character.position, 1f);
        StartCoroutine(LoadNextSceneAfterWait());
    }
    IEnumerator LoadNextSceneAfterWait()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SINGLE PLAYER 3");
    }

}
