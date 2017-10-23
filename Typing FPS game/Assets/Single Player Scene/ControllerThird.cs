using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerThird : MonoBehaviour {

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemiesFirstStop;
    public ArrayList mainStopPoints = new ArrayList();
    public GameObject currentGameObject;
    private Controller controllerScript;
    private int globalSelector = 0;

    JungleCommandoScript jungleCommandoScript0;
    JungleCommandoScript jungleCommandoScript1;

    TyperHelper typerScript;

    // Use this for initialization
    void Start()
    {
        controllerScript = this.GetComponent<Controller>();
        pathPercent = controllerScript.PathToStop;
        mainStopPoints.Add(98);

        jungleCommandoScript0 = (JungleCommandoScript)enemiesFirstStop[0].GetComponent(typeof(JungleCommandoScript));
        jungleCommandoScript1 = (JungleCommandoScript)enemiesFirstStop[1].GetComponent(typeof(JungleCommandoScript));

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

                LoadNextScene();
            }
        }

        if (mainStopPoints[globalSelector].Equals(pathPercent))
        {
            switch (globalSelector)
            {
                case 0: DoFirstStop(); break;
                default: break;
            }
        }
    }

    void DoFirstStop()
    {
        goToNextStopPoint = false;
        Debug.Log("called first");
        jungleCommandoScript0.EnemyHere();
        jungleCommandoScript1.EnemyHere();
        typerScript.LoadNewEnemies(enemiesFirstStop);
        globalSelector++;
    }
    void LoadNextScene()
    {
        StartCoroutine(LoadNextSceneAfterWait());
    }
    IEnumerator LoadNextSceneAfterWait()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SINGLE PLAYER 2");
    }
}
