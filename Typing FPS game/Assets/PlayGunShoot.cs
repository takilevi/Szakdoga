using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGunShoot : StateMachineBehaviour
{
    public AudioClip[] enemyGunShoot;
    private GameObject player;
    public MonoBehaviour monoBehaviour;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("ResetShoot", false);

        int pistolChoice = Random.Range(0, 3);
        player = GameObject.Find("MainCamera");

        if(monoBehaviour.GetType().Equals(typeof(TerroristScript)))
        {
            AudioSource.PlayClipAtPoint(enemyGunShoot[pistolChoice], player.transform.position, 0.05f);
        }
        else
        {
            AudioSource.PlayClipAtPoint(enemyGunShoot[pistolChoice], player.transform.position, 0.1f);
        }

        this.monoBehaviour.StartCoroutine(WaitRandomSec(animator));
        
    }

    IEnumerator WaitRandomSec(Animator animator)
    {
        yield return new WaitForSeconds(Random.Range(0.5f,2f));
        animator.SetBool("ResetShoot", true);
    }

}
