using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ZombieScript : MonoBehaviour
{

    public GameObject playerObject;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
