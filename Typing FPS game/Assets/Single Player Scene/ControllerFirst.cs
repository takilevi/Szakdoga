using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerFirst : MonoBehaviour
{

    public int pathPercent;
    public bool goToNextStopPoint = true;
    public GameObject[] enemiesSecondStop;
    public GameObject[] enemiesFourthStop;
    public ArrayList mainStopPoints = new ArrayList();
    public AudioClip introClip;
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
        mainStopPoints.Add(4);
        mainStopPoints.Add(18);
        mainStopPoints.Add(46);
        mainStopPoints.Add(82);
        mainStopPoints.Add(86);
        mainStopPoints.Add(99);

        terroristScript0 = (TerroristScript)enemiesSecondStop[0].GetComponent(typeof(TerroristScript));
        terroristScript1 = (TerroristScript)enemiesSecondStop[1].GetComponent(typeof(TerroristScript));
        terroristScript2 = (TerroristScript)enemiesFourthStop[0].GetComponent(typeof(TerroristScript));
        terroristScript3 = (TerroristScript)enemiesFourthStop[1].GetComponent(typeof(TerroristScript));

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
                Debug.Log("továbbmehetünk: " + Time.time);
            }
        }

        if (mainStopPoints[globalSelector].Equals(pathPercent))
        {
            switch (globalSelector)
            {
                case 0: DoIntro(); break;
                case 1: DoFirstStop(); break;
                case 2:
                    DoSecondStop();
                    break;
                case 3:
                    DoThirdStop();
                    break;
                case 4: DoFourthStop(); break;
                case 5: DoFifthStop(); break;
                default: break;
            }
        }
    }

    void DoIntro()
    {
        AudioSource.PlayClipAtPoint(introClip, controllerScript.character.position, 1f);
        goToNextStopPoint = false;
        StartCoroutine(DoIntoWait());
        globalSelector++;
    }
    IEnumerator DoIntoWait()
    {
        yield return new WaitForSeconds(5f);
        goToNextStopPoint = true;
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
        yield return new WaitForSeconds(5.5f);
        goToNextStopPoint = true;
    }
    void DoSecondStop()
    {
        Debug.Log("called second at time: "+ Time.time);
        AudioSource.PlayClipAtPoint(secondStopAudio, controllerScript.character.position, 1f);
        goToNextStopPoint = false;
        StartCoroutine(DoSecondStopWait());
        globalSelector++;
    }
    IEnumerator DoSecondStopWait()
    {
        yield return new WaitForSeconds(2.5f);

        terroristScript0.TurnToPlayer();
        terroristScript1.TurnToPlayer();
        typerScript.LoadNewEnemies(enemiesSecondStop);

    }
    void DoThirdStop()
    {
        Debug.Log("called third");
        AudioSource.PlayClipAtPoint(thirdStopAudio, controllerScript.character.position, 0.7f);
        goToNextStopPoint = false;
        StartCoroutine(DoThirdStopWait());
        globalSelector++;
    }
    IEnumerator DoThirdStopWait()
    {
        yield return new WaitForSeconds(4.5f);
        controllerScript.speed = 0.05f;
        goToNextStopPoint = true;
    }
    void DoFourthStop()
    {
        Debug.Log("called fourth");
        goToNextStopPoint = false;

        terroristScript2.TurnToPlayer();
        terroristScript3.TurnToPlayer();
        typerScript.LoadNewEnemies(enemiesFourthStop);

        globalSelector++;

    }
    void DoFifthStop()
    {
        Debug.Log("called fourth");
        controllerScript.speed = 0.10f;
        nextGameObject.SetActive(true);
        currentGameObject.SetActive(false);

    }

}
