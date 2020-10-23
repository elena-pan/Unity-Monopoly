﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class DiceScript : MonoBehaviour
{
    static Rigidbody rb;
    public static Vector3 diceVelocity;
   
=======
public class DiceScript : MonoBehaviour {
>>>>>>> 99b87de44fee19c3f4bc424a1344c85017ab694d

	static Rigidbody rb;
	public static Vector3 diceVelocity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		diceVelocity = rb.velocity;

		if (Input.GetKeyDown (KeyCode.Space)) {
			DiceNumberTextScript.diceNumber = 0;
			float dirX = Random.Range (10, 1000);
			float dirY = Random.Range (10, 1000);
			float dirZ = Random.Range (10, 1000);
			transform.position = new Vector3 (0, 1000, 0);
			transform.rotation = Quaternion.identity;
			rb.AddForce (transform.up * 5000);
			rb.AddTorque (dirX, dirY, dirZ);
		}
	}
}