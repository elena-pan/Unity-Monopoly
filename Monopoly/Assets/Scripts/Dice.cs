using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monopoly
{
	public class Dice : MonoBehaviour
	{
		static Rigidbody rb;
		public static Vector3 diceVelocity;

		// Use this for initialization
		void Start () {
			rb = GetComponent<Rigidbody> ();
		}

		public void RollDice() {
			DiceResult.diceNum = -1;
			diceVelocity = rb.velocity;
			float dirX = Random.Range (10, 500);
			float dirY = Random.Range (10, 500);
			float dirZ = Random.Range (10, 500);
			transform.position = new Vector3 (0, 10, 0);
			transform.rotation = Quaternion.identity;
			rb.AddForce (transform.up * 150);
			rb.AddTorque (dirX, dirY, dirZ);
		}
	}
}