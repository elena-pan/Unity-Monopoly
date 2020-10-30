using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	static Rigidbody rb;
	public static Vector3 diceVelocity;
	public int currentNum;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	public void RollDice() {
		currentNum = -1;
		diceVelocity = rb.velocity;
		float dirX = Random.Range (10, 500);
		float dirY = Random.Range (10, 500);
		float dirZ = Random.Range (10, 500);
		transform.position = new Vector3 (0, 10, 0);
		transform.rotation = Quaternion.identity;
		rb.AddForce (transform.up * 100);
		rb.AddTorque (dirX, dirY, dirZ);
	}

	void OnTriggerEnter(Collider col)
	{
		diceVelocity = rb.velocity;
		print(diceVelocity);
		if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
		{
			if (col.gameObject.name == "Centrepiece") {
				GameObject centrepiece = GameObject.Find("Centrepiece");
                DiceResult diceResult = centrepiece.GetComponent<DiceResult>();
                currentNum = diceResult.diceNum;
			}
		}
	}
}
