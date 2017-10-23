using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JungleCommandoScript : MonoBehaviour {

    public GameObject playerObject;
    public GameObject thisObject;
    public GameObject thisCanvas;

    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnemyHere()
    {
        thisCanvas.SetActive(true);
        Text jungleText = (Text)thisCanvas.GetComponentInChildren<Text>();
        WordHelper script = (WordHelper)playerObject.GetComponent(typeof(WordHelper));
        jungleText.text = script.GetAdjective();
        jungleText.text += " " + script.GetNoun();

        StartCoroutine(StartFire());
    }
    IEnumerator StartFire()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        //triggers shoot
        animator.SetTrigger("EnemyHere");
        yield return new WaitForSeconds(1f);
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }
    public void KillToDeath()
    {
        thisCanvas.SetActive(false);
        animator.SetTrigger("DeadTrigger");
        animator.SetFloat("Death", Random.Range(0f, 1f));
        StartCoroutine(DestroyAfterDeath());
    }
    IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(6);
        Destroy(thisObject);
    }
}
