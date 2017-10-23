using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject thisObject;
    public GameObject thisCanvas;
    private bool letsMove = false;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        float idleSwitch = Random.Range(0f, 1f);
        anim.SetFloat("IdleSwitch", idleSwitch);
    }
	
	// Update is called once per frame
	void Update () {
        if (letsMove)
        {
            float distance = Vector3.Distance(playerObject.transform.position, this.transform.position);
            //Debug.Log("távolság float: " + distance + "  objectneve: "+this.name);
            if(distance <= 5.5f)
            {
                AttackEnemy();
            }

            Vector3 targetPostition = new Vector3(playerObject.transform.position.x,
                                        this.transform.position.y,
                                        playerObject.transform.position.z);
            this.transform.LookAt(targetPostition);
            transform.position += transform.forward * 0.5f * Time.deltaTime;
        }
    }

    public void EnemyHere()
    {
        thisCanvas.SetActive(true);
        Text zombieText = (Text)thisCanvas.GetComponentInChildren<Text>();
        WordHelper script = (WordHelper)playerObject.GetComponent(typeof(WordHelper));
        int i = Random.Range(1, 4);
        switch(i)
        {
            case 1: zombieText.text = script.GetVerb();break;
            case 2: zombieText.text = script.GetAdjective(); break;
            case 3: zombieText.text = script.GetNoun(); break;
            default: break;
        }

        StartCoroutine(StartChase());
    }
    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        anim.SetTrigger("StartWalk");
        // itt kezdünk el felé menni
        letsMove = true;
    }
    public void AttackEnemy()
    {
        letsMove = false;
        anim.SetTrigger("DoAttack");
        anim.SetFloat("AttackSwitch", Random.Range(0f, 1f));
    }
    public void KillToDeath()
    {
        letsMove = false;
        thisCanvas.SetActive(false);
        anim.SetTrigger("DeadTrigger");
        anim.SetFloat("Death", Random.Range(0f, 1f));
        StartCoroutine(DestroyAfterDeath());
    }
    IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(6);
        Destroy(thisObject);
    }
}
