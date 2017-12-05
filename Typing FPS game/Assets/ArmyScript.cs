using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyScript : MonoBehaviour
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

    // Set the StateMachineBehaviour's MonoBehaviour to this.
    playGunShoot.monoBehaviour = this;

  }
  public AnimationClip GetAnimationClip(string name)
  {
    if (!animator) return null; // no animator

    foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
    {
      if (clip.name == name)
      {
        return clip;
      }
    }
    return null; // no clip by that name
  }

  // Update is called once per frame
  void Update()
  {

  }
  public void EnemyHere()
  {
    Vector3 targetPostition = new Vector3(playerObject.transform.position.x,
                                    this.transform.position.y,
                                    playerObject.transform.position.z);
    this.transform.LookAt(targetPostition);

    thisCanvas.SetActive(true);
    Text armyText = (Text)thisCanvas.GetComponentInChildren<Text>();
    WordHelper script = (WordHelper)playerObject.GetComponent(typeof(WordHelper));
    int i = Random.Range(1, 4);
    switch (i)
    {
      case 1: armyText.text = script.GetVerb(); break;
      case 2: armyText.text = script.GetAdjective(); break;
      case 3: armyText.text = script.GetNoun(); break;
      default: break;
    }

    animator.SetTrigger("EnemyAppears");

    StartCoroutine(StartFire());
  }
  IEnumerator StartFire()
  {
    yield return new WaitForSeconds(Random.Range(0.5f, 5f));
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
