using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSecond : MonoBehaviour
{

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemiesSecondStop;
    public GameObject[] enemiesFourthStop;
    public ArrayList mainStopPoints = new ArrayList();
    public AudioClip firstStopAudio;
    public AudioClip secondStopAudio;
    public AudioClip thirdStopAudio;
    public GameObject currentGameObject;
    public GameObject nextGameObject;
    private Controller controllerScript;
    private int globalSelector = 0;
    TerroristScript terroristScript0;
    TerroristScript terroristScript1;
    TerroristScript terroristScript2;
    TerroristScript terroristScript3;

    TyperHelper typerScript;

    // Use this for initialization
    void Start()
    {
        controllerScript = this.GetComponent<Controller>();
        pathPercent = controllerScript.PathToStop;
        mainStopPoints.Add(3);
        mainStopPoints.Add(33);
        mainStopPoints.Add(40);
        mainStopPoints.Add(72);
        mainStopPoints.Add(99);

        terroristScript0 = (TerroristScript)enemiesSecondStop[0].GetComponent(typeof(TerroristScript));
        terroristScript1 = (TerroristScript)enemiesSecondStop[1].GetComponent(typeof(TerroristScript));

        typerScript = (TyperHelper)GameObject.Find("MainCamera").GetComponent(typeof(TyperHelper));
    }

    // Update is called once per frame
    void Update()
    {
        pathPercent = controllerScript.PathToStop;
        controllerScript.IsMoveEnable = goToNextStopPoint;
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
                case 3: DoFourthStop(); break;
                case 4: DoFifthStop(); break;
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
        yield return new WaitForSeconds(3.3f);
        controllerScript.speed = 0.10f;
        goToNextStopPoint = true;
    }
    void DoSecondStop()
    {
        Debug.Log("called second");
        AudioSource.PlayClipAtPoint(secondStopAudio, controllerScript.character.position, 1f);
        goToNextStopPoint = false;
        StartCoroutine(DoSecondStopWait());
        globalSelector++;
    }
    IEnumerator DoSecondStopWait()
    {
        yield return new WaitForSeconds(3.5f);
        goToNextStopPoint = true;
        

    }
    void DoThirdStop()
    {
        goToNextStopPoint = false;
        terroristScript0.TurnToPlayer();
        terroristScript1.TurnToPlayer();
        typerScript.LoadNewEnemies(enemiesSecondStop);
        globalSelector++;
    }
    void DoFourthStop()
    {
        Debug.Log("called third");
        AudioSource.PlayClipAtPoint(thirdStopAudio, controllerScript.character.position, 0.7f);
        goToNextStopPoint = false;
        StartCoroutine(DoFourthStopWait());
        globalSelector++;
    }
    IEnumerator DoFourthStopWait()
    {
        yield return new WaitForSeconds(4.5f);
        controllerScript.speed = 0.08f;
        goToNextStopPoint = true;
    }
    void DoFifthStop()
    {
        Debug.Log("called fourth");
        controllerScript.speed = 0.09f;
        nextGameObject.SetActive(true);
        currentGameObject.SetActive(false);

    }

}
