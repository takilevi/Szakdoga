using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerBehaviour : MonoBehaviour {

  Animator flamerAnimator;
	// Use this for initialization
	void Start () {
    flamerAnimator = GetComponent<Animator>();
  }
	
	// Update is called once per frame
	void Update () {
		
	}
  private void OnCollisionEnter(Collision collision)
  {
    Debug.Log("got hurt");
    flamerAnimator.SetFloat("hitChoice", Random.Range(0f, 1f));
    flamerAnimator.SetTrigger("hitTrigger");
  }
}
