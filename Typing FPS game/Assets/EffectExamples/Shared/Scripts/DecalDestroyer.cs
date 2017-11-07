using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour {

	public float lifeTime = 0.2f;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
