using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Monopoly
{
	public class House : MonoBehaviour // Also includes hotel
	{
		static Rigidbody rb;

		// Use this for initialization
		void Start () {
			rb = GetComponent<Rigidbody> ();

            StartCoroutine(MakeKinematic());
		}

		public void DestroyHouse() { // Throw house up in the air
            rb.isKinematic = false;
            float dirX = Random.Range (10, 500);
			float dirY = Random.Range (10, 500);
			float dirZ = Random.Range (10, 500);
            Vector3 currentPos = this.transform.position;
            currentPos.y = 5;
            this.transform.position = currentPos;

			rb.AddForce (transform.up * 1000);
			rb.AddTorque (dirX, dirY, dirZ);
            
            StartCoroutine(WaitBeforeDestroying());
		}

        private IEnumerator WaitBeforeDestroying() {
        
            yield return new WaitForSeconds(1);
            PhotonNetwork.Destroy(this.gameObject);
        }
        // Wait before making kinematic so it has time to fall onto the board
        private IEnumerator MakeKinematic() {
        
            yield return new WaitForSeconds(5);

            while (rb.velocity.magnitude > 1f) { // Make sure we've actually landed and stopped
                yield return new WaitForSeconds(2);
            }

            rb.isKinematic = true;
        }
	}
}