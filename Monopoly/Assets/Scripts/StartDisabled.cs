using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly
{
    public class StartDisabled : MonoBehaviour
    {
        // Sets disabled on start
        void Start()
        {
            this.gameObject.SetActive(false);
        }
    }
}