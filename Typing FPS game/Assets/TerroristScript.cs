using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerroristScript : MonoBehaviour
{
  public GameObject playerObject;
  public GameObject thisObject;
  public GameObject thisCanvas;
  private PlayGunShoot playGunShoot;

  Animator animator;

  // Use this for initialization
  void Start()
  {
    animator = GetComponent<Animator>();
    animator.SetFloat("Idle", Random.Range(0f, 1f));

    playGunShoot = animator.GetBehaviour<PlayGunShoot>();

    // Set the StateMachineBehaviour's reference to an ExampleMonoBehaviour to this.
    playGunShoot.monoBehaviour = this;
  }

  // Update is called once per frame
  void Update()
  {

  }
  public void TurnToPlayer()
  {
    Vector3 targetPostition = new Vector3(playerObject.transform.position.x,
                                    this.transform.position.y,
                                    playerObject.transform.position.z);
    this.transform.LookAt(targetPostition);
    thisCanvas.SetActive(true);
    Text terrorText = (Text)thisCanvas.GetComponentInChildren<Text>();
    WordHelper script = (WordHelper)playerObject.GetComponent(typeof(WordHelper));
    terrorText.text = script.GetSentence();

    animator.SetTrigger("EnemyAppears");

    StartCoroutine(StartFire());
  }
  IEnumerator StartFire()
  {
    yield return new WaitForSeconds(Random.Range(0.5f, 3f));
    //triggers shoot
    animator.SetTrigger("Shoot");
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
    yield return new WaitForSeconds(5);
    Destroy(thisObject);
  }
}
