using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace Monopoly
{
    public class WinnerScene : MonoBehaviourPunCallbacks
    {

        [SerializeField]
        private Text winnerText;
        public static string winner;

        void Start()
        {
            winnerText.text = winner;
            PhotonNetwork.LeaveRoom();
        }

        public void NavigateToStart()
        {
            winner = "";
            SceneManager.LoadScene(0);
        }
    }
}