using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Monopoly 
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        public static GameObject LocalPlayerInstance;
        public GameObject PlayerUiPrefab;
        public int location;
        public int balance;
        public bool[] ownedProperties;
        public bool isBankrupt;
    }
}