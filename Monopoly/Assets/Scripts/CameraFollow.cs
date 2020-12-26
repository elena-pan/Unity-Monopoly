using UnityEngine;
using Photon.Pun;

namespace Monopoly
{
    public class CameraFollow : MonoBehaviourPunCallbacks
    {
        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 5.0f;

        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float height = 3.0f;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float lerpSpeed = 2f;

        // cached transform of the target
        Transform cameraTransform;

        public static bool isFollowing = false;

        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;
        void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            if (isFollowing && photonView.IsMine) {
                Follow();
            }
        }

        /// Follow the target smoothly
        void Follow()
        {
            CameraController.viewDiceRoll = false; // Prevents both happening at once
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            Vector3 newPos = this.transform.position + cameraOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPos, lerpSpeed*Time.deltaTime);
            cameraTransform.LookAt(this.transform.position);

            // Stop following once we reach the position
            if (cameraTransform.position == newPos) {
                isFollowing = false;
            }
        }
    }
}