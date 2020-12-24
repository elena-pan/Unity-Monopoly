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