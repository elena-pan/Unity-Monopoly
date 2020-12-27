using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly 
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        //public GameObject PlayerUi;
        // We make these variables static because we only need to know the stats of our local player
        public static GameObject LocalPlayerInstance;
        public static int location;
        public static int balance;
        public static int noMoneyAmount;
        public static bool isMoving;
        
        public float lerpSpeed = 5f;
        public float slerpSpeed = 10f;

        void Awake()
        {
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
                PlayerManager.balance = 1500;
                PlayerManager.location = 0;
                PlayerManager.noMoneyAmount = 0;
            }
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            // Continually update location of our piece
            if (photonView.IsMine && LocalPlayerInstance && isMoving) {
                Vector3 newLocation = GameManager.instance.board.locations[location].gridPoint;
                LocalPlayerInstance.transform.position = Vector3.Lerp(LocalPlayerInstance.transform.position, newLocation, lerpSpeed*Time.deltaTime);

                // Also rotate back to upright
                Quaternion upright = Quaternion.Euler(270, 270, 0);
                LocalPlayerInstance.transform.rotation = Quaternion.Slerp(LocalPlayerInstance.transform.rotation, upright, slerpSpeed*Time.deltaTime);

                if (Vector3.Distance(LocalPlayerInstance.transform.position, newLocation) < 1.0f) {
                    isMoving = false;
                }
            }
        }

        /*
        void Start()
        {
            if (PlayerUi != null)
            {
                // Send a message to set target
                PlayerUi.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }*/
    }
}