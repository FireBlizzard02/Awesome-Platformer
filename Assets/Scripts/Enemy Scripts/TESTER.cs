using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTER : MonoBehaviour {

	private Rigidbody2D myBody;

	void Start () {
		myBody = GetComponent<Rigidbody2D> ();
	}

	public void DamageAndPush(float force) {
		myBody.AddForce (new Vector2 (force, Mathf.Abs(force)));
	}

}
