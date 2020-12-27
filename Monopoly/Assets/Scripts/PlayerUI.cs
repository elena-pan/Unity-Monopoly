using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly
{
    public class PlayerUI : MonoBehaviour
    {

        [SerializeField]
        private Text activityPanelLine1;
        [SerializeField]
        private Text activityPanelLine2;
        [SerializeField]
        private Text activityPanelLine3;
        [SerializeField]
        private Text activityPanelLine4;
        [SerializeField]
        private Text statsBody;

        [SerializeField]
        private Button buyPropertyButton;
        [SerializeField]
        private Button buildHouseButton;
        [SerializeField]
        private Button sellHouseButton;
        [SerializeField]
        private Button endTurnButton;
        private PlayerManager target;
        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Update()
        {
            // Update player stats
            if (statsBody != null) {
                // Remove string "(clone)" from gamepiece name
                string gamePieceName = PlayerManager.LocalPlayerInstance.name.Remove(PlayerManager.LocalPlayerInstance.name.Length - 7);
                string temp = "Balance: " + PlayerManager.balance.ToString() + "\nGamepiece: " + gamePieceName;
                if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Bankrupt"] == false) {
                    temp = temp + "\nBankrupt: " + "No";
                } else {
                    temp = temp + "\nBankrupt: " + "Yes";
                }
                statsBody.text = temp;
            }
        }

        public void AddActivityText(string text) {
            activityPanelLine4.text = activityPanelLine3.text;
            activityPanelLine3.text = activityPanelLine2.text;
            activityPanelLine2.text = activityPanelLine1.text;
            activityPanelLine1.text = text;
        }

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            //playerNameText.text = target.photonView.Owner.NickName;
        }
        public void EnableButton(string button) {
            switch (button) {
                case "buyPropertyButton":
                    buyPropertyButton.interactable = true;
                    break;
                case "buildHouseButton":
                    buildHouseButton.interactable = true;
                    break;
                case "sellPropertyButton":
                    sellHouseButton.interactable = true;
                    break;
                case "endTurnButton":
                    endTurnButton.interactable = true;
                    break;
            }
        }
        
        public void DisableButton(string button)
        {
            switch (button) {
                case "buyPropertyButton":
                    buyPropertyButton.interactable = false;
                    break;
                case "buildHouseButton":
                    buildHouseButton.interactable = false;
                    break;
                case "sellPropertyButton":
                    sellHouseButton.interactable = false;
                    break;
                case "endTurnButton":
                    endTurnButton.interactable = false;
                    break;
            }
        }
    }
}